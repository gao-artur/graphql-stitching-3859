using System;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Remote
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
            services
                .AddGraphQLServer()
                .AddHttpRequestInterceptor<DefaultHttpRequestInterceptor>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .ModifyRequestOptions(options => options.ExecutionTimeout = TimeSpan.FromHours(1));
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
