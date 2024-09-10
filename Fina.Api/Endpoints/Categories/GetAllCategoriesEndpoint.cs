﻿using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Responses;
using Fina.Core;
using Microsoft.AspNetCore.Mvc;
using Fina.Core.Requests.Categories;
using Fina.Api.Common.Api;

namespace Fina.Api.Endpoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Categories: Get All")
            .WithSummary("Recupera todas as categorias")
            .WithDescription("Recupera todas as categorias")
            .WithOrder(5)
            .Produces<PagedResponse<List<Category>>>();

        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            GetAllCategoryRequest request = new()
            {
                UserId = ApiConfiguration.UserId,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            PagedResponse<List<Category>> result = await handler.GetAllAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
