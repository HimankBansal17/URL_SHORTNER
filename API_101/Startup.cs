using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using API_101.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_101
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
            services.AddDbContext<API_Context>(options => options.UseSqlite("Data Source=UrlDB.db"));

            //services.AddDbContext<API_Context>(opt =>
           // opt.UseInMemoryDatabase("APIList"));
            services.AddControllers();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,API_Context aPI_Context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            aPI_Context.Database.EnsureCreated();

            

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
           
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=APIModels}/{action=GetAPIModels}/{Name}");

            });
        }
    }
}
