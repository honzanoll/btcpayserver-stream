using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Business.Extensions;
using BTCPayServer.Stream.Common.Extensions;
using BTCPayServer.Stream.Data.DALs;
using BTCPayServer.Stream.Data.Extensions;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.Extensions;
using BTCPayServer.Stream.Repository.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using honzanoll.MVC.NetCore.Extensions;
using honzanoll.Storage.NetCore.Extensions;
using System;

namespace BTCPayServer.Stream.Portal
{
    public class Startup
    {
        #region Fields

        private readonly IWebHostEnvironment webHostEnvironment;

        #endregion

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;

            this.webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region Public methods

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDataProtection()
            //    .SetApplicationName("BTCPayServer.Stream-portal")
            //    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(webHostEnvironment.ContentRootPath, Path.Combine("artifacts", "af-keys"))));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            services.AddCommonConfigurations(Configuration);

            services.AddDatabaseContext(Configuration);
            services.AddRepositories();

            services.AddDefaultIdentity<ApplicationUser>()
               .AddRoles<IdentityRole<Guid>>()
               .AddEntityFrameworkStores<SqlContext>();

            services.AddStreamlabsServices(Configuration);
            services.AddBtcPayServerServices(Configuration);
            services.AddPriceStoreService(Configuration);

            services.AddLocalStorage<StorageType>(Configuration);

            services.AddCache();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            services.AddAuthentication();

            services.AddViewRenderer();

            services.AddFileMiddlewares();

            services
                .AddControllersWithViews()
                .AddViewLocalization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SqlContext sqlContext)
        {
            // Apply last database migration
            sqlContext.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders();

            app.UseLocalization();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapFileMiddlewares();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }

        #endregion
    }
}
