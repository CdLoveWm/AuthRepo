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

                #region Swagger����bearer token������ Authorize��ť
                // Bearer ��scheme����
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //���������ͷ��
                    In = ParameterLocation.Header,
                    //ʹ��Authorizeͷ��
                    Type = SecuritySchemeType.Http,
                    //����Ϊ�� bearer��ͷ
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                // �����з�������Ϊ����bearerͷ����Ϣ
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
                // ע�ᵽswagger��
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
                #endregion
            });

            #region JWT��֤
            // �����ط�ע��ʱʹ��
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            // �����õ�ʵ��
            JwtSettings jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            // �����֤����
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // ����ΪJWT��Schema
            })
            // ����JWT
            // JWTBearer�ĵ���https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer?view=aspnetcore-5.0
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = JwtHandler.GetTokenValidParamConfig(jwtSettings);
                //opt.TokenValidationParameters = JwtRsaHandler.GetTokenValidParamConfig(jwtSettings);
                opt.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {

                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            #region ������������Ȩ

            //services.AddAuthorization(options =>
            //{
            //    // 1����������ʾ����ӵ����Ϊ EmpName ��Claim���ܼ�Ȩ�ɹ�
            //    options.AddPolicy("RequireEmpName", policy => policy.RequireClaim("empname"));
            //    // 2����������ʾ����ӵ����Ϊ EmpName ��Claim�����Ҷ�Ӧ�����ݱ���Ϊ��emp1��emp2���е�һ�����ܼ�Ȩ�ɹ�
            //    options.AddPolicy("SpecialEmpName", policy => policy.RequireClaim("empname", new string[] { "emp1", "emp2" }));
            //});

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
