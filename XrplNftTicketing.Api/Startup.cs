using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using XrplNftTicketing.Business.Interfaces;
using XrplNftTicketing.Business.Services;
using XrplNftTicketing.Entities.Configurations;

namespace XrplNftTicketing.Api
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


            // Configure Configs
            services.Configure<IpfsSettings>(Configuration.GetSection("IpfsSettings"));
            services.Configure<XrplSettings>(Configuration.GetSection("XrplSettings"));
            
            var ipfsConfig = Configuration.GetSection("IpfsSettings").Get<IpfsSettings>();
            services.AddScoped<IIpfsService>(x => new IpfsPinataService(ipfsConfig.Key, ipfsConfig.Secret, ipfsConfig.HttpViewerUrl));

            var xrplConfig = Configuration.GetSection("XrplSettings").Get<XrplSettings>();
            services.AddScoped<IXrplService>(x => new XrplService(xrplConfig.WssUrl));



            services.AddControllers();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "Content")),
                RequestPath = "/content"
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }


}
