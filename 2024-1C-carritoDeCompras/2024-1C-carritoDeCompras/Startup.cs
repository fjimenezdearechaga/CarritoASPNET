using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _2024_1C_carritoDeCompras
{
    public static class Startup
    {
        public static WebApplication InicializarApp(String[] args) {
         
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);

            return app;
        }
        private static void ConfigureServices(WebApplicationBuilder builder) {

            //builder.Services.AddDbContext<CarritoDeComprasContext>(options => options.UseInMemoryDatabase("CarritoDeComprasDb"));
            builder.Services.AddDbContext<CarritoDeComprasContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CarritoDeComprasDBCS")));
           
            #region Identity
            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<CarritoDeComprasContext>();
            builder.Services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireDigit = false;
                opciones.Password.RequiredLength = 5;
            }
            );


            #endregion
            

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opciones =>
                {
                    opciones.LoginPath = "/Account/IniciarSesion";
                    opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                    opciones.Cookie.Name = "IdentidadCarritoDeComprasApp";

                });
            builder.Services.AddControllersWithViews();
        }

        public static void Configure(WebApplication app) {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
