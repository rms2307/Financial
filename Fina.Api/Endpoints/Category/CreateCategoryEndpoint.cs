using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Requests.Categories;

namespace Fina.Api.Endpoints.Category
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            throw new NotImplementedException();
        }

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, CreateCategoryRequest request)
        {
            var response = await handler.CreateAsync(request);
            response.IsSuccess
        }
    }
}
