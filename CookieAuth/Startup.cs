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
            .AddRazorRuntimeCompilation() // cshtml�������
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;//�������ֶδ���ǰ��Сд����
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//�����˷����������ı�����
            });

            // �����֤
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options => {
                options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme; // Cookie��key
                options.LoginPath = "/Account/login"; // δ��֤ʱ�ĵ�¼��ַ
                options.AccessDeniedPath = "/Account/login"; // ��֤ʧ����ת��URL
                options.ExpireTimeSpan = TimeSpan.FromSeconds(10); // ����Cookie����Чʱ��
                options.SlidingExpiration = true; // ��Cookie��Чʱ����룬Ȼ������������ʱ�򣬽�ԭ��Cookie���£���֤һֱʹ�õ�ʱ�򲻻���ΪCookieʧЧ�˳�
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

            // ʹ����֤��Ȩ
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
