using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shop.Web.Data;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;

namespace Shop.Web
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
            services.AddIdentity<User, IdentityRole>(cfg=>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<DataContext>();

            //Para Autenticacion con Tokens de seguridad para el API ProductsController
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });

            //Servicio de Conexion a SQL Server
            services.AddDbContext<DataContext>(cfg =>
           {
               cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
           });

                       

            //SERVICIO DEL Seeder
            services.AddTransient<SeedDb>();

            //iNYECCION DEL REPOSITORIO INTERMEDIO PARA CONEXION A BD
            //iMPORTANTE PARA CREAR PRUEBAS UNITARIAS CON DATOS FALSOS SIN BD
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();

            services.AddScoped<IUserHelper, UserHelper>();

            //Para controlar Redireccion de vistas NoAuthorized por seguridad
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}"); //Para manejar redirerccion ERROR 404
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
