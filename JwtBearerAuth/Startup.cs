using Auth.IServices;
using Auth.Models.Jwt;
using Auth.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using JwtBearerAuth.Utility;
using JwtBearerAuth.Policy;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using JwtBearerAuth.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace JwtBearerAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IUserServices, UserServices>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtBearerAuth", Version = "v1" });

                #region Swagger增加bearer token参数， Authorize按钮
                // Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.Http,
                    //内容为以 bearer开头
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                // 把所有方法配置为增加bearer头部信息
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                    }
                };
                // 注册到swagger中
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
                #endregion
            });


            #region JWT认证
            // 其他地方注入时使用
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            // 绑定配置到实例
            JwtSettings jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            // 添加认证服务
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // 这里为JWT的Schema

                #region 自定义Schema

                options.DefaultForbidScheme = nameof(FailAuthResponseHandler);
                options.DefaultChallengeScheme = nameof(FailAuthResponseHandler);

                #endregion
            })
            // 配置JWT
            // JWTBearer文档：https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer?view=aspnetcore-5.0
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = JwtHandler.GetTokenValidParamConfig(jwtSettings);
                //opt.TokenValidationParameters = JwtRsaHandler.GetTokenValidParamConfig(jwtSettings);

                #region JWT事件
                //opt.Events = new JwtBearerEvents()
                //{
                //    OnMessageReceived = context =>
                //    {
                //        return Task.CompletedTask;
                //    },
                //    OnTokenValidated = context =>
                //    {

                //        return Task.CompletedTask;
                //    },
                //    OnChallenge = context =>
                //    {

                //        return Task.CompletedTask;
                //    }
                //};
                #endregion
            })
            // 添加自定义Handler
            .AddScheme<AuthenticationSchemeOptions, FailAuthResponseHandler>(nameof(FailAuthResponseHandler), o => { });

            #endregion

            #region 基于声明的授权

            services.AddAuthorization(options =>
            {
                // 1、该声明表示必须拥有名为 EmpName 的Claim才能鉴权成功
                options.AddPolicy("RequireEmpName", policy => policy.RequireClaim("empname"));
                // 2、该声明表示必须拥有名为 EmpName 的Claim，而且对应的数据必须为【emp1、emp2】中的一个才能鉴权成功
                options.AddPolicy("SpecialEmpName", policy => policy.RequireClaim("empname", new string[] { "emp1", "emp2" }));
            });

            #endregion

            #region 基于策略--自定义策略

            services.AddAuthorization(options =>
            {
                // 策略和策略处理类合并
                options.AddPolicy("specialNamePolicy", policy => policy.AddRequirements(new SpecialNameRequirement()));
                // 策略和策略处理类分开
                options.AddPolicy(nameof(SpecialNameSignalRequirement), policy => policy.AddRequirements(new SpecialNameSignalRequirement()));
            });
            // 策略和策略处理类分开时，需要注册策略处理类
            services.AddScoped<IAuthorizationHandler, SpecialNameSignalRequirementHandler>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JwtBearerAuth v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
