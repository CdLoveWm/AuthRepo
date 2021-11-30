using Auth.IServices;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace CookieAuth
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
            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation() // cshtml保存编译
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;//解决后端字段传到前端小写问题
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//解决后端返回数据中文被编码
            });

            // 添加认证
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options => {
                options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme; // Cookie的key
                options.LoginPath = "/Account/login"; // 未认证时的登录地址
                options.AccessDeniedPath = "/Account/login"; // 认证失败跳转的URL
                options.ExpireTimeSpan = TimeSpan.FromSeconds(10); // 设置Cookie的有效时间
                options.SlidingExpiration = true; // 当Cookie有效时间过半，然后有请求来的时候，将原有Cookie更新：保证一直使用的时候不会因为Cookie失效退出
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // 使用认证授权
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
