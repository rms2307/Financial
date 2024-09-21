using Fina.Api.Handles;
using Fina.Api.Infra;
using Fina.Core;
using Fina.Core.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Logging.Console;

namespace Fina.Api.Common.Api
{
    public static class BuildExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
            Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x => x.CustomSchemaIds(n => n.FullName));
        }

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {
            builder
                .Services
                .AddDbContext<AppDbContext>(x =>
                {
                    x.UseSqlite(ApiConfiguration.ConnectionString);
                });
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(
                opt => opt.AddPolicy(ApiConfiguration.CorsPolicyName, policy =>
                    policy.WithOrigins([Configuration.BackendUrl, Configuration.FrontendUrl,])
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            //builder.Services.AddCors(
            //    opt => opt.AddPolicy(ApiConfiguration.CorsPolicyName, policy =>
            //        policy.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder
                .Services
                .AddTransient<ICategoryHandler, CategoryHandler>()
                .AddTransient<ITransactionHandler, TransactionHandler>();
        }
    }
}
