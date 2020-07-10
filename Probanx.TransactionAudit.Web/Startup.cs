using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.Core.Services;

namespace Probanx.TransactionAudit.Web
{
    public class Startup
    {
        const string ELASTIC_HOST_URL = "ELASTIC_HOST_URL";
        const string ELASTIC_INDEX = "ELASTIC_INDEX";
        const string RABBIT_MQ_HOST_NAME = "RABBIT_MQ_HOST_NAME";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string elasticHostUrl = Environment.GetEnvironmentVariable(ELASTIC_HOST_URL) ?? "http://host.docker.internal:9200";
            string elasticIndex = Environment.GetEnvironmentVariable(ELASTIC_INDEX) ?? "transactions";
            string rabbitMqHostName = Environment.GetEnvironmentVariable(RABBIT_MQ_HOST_NAME) ?? "host.docker.internal";

            services
                .AddRabbitMQ(rabbitMqHostName)
                .AddMessageDispatcher<Message>()
                .AddElasticSearch(elasticHostUrl, elasticIndex)
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
