using System;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("remote", (sp, client) =>
            {
                client.BaseAddress = new Uri("http://localhost:15000/graphql");
            });

            services
                .AddGraphQLServer()
                .AddRemoteSchema("remote")
                .AddType(new AnyType("PropertiesDictionary"));

            services
                .AddGraphQLServer("remote")
                .AddType(new AnyType("PropertiesDictionary"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL(path: "/graphql");
            });
        }
    }
}
