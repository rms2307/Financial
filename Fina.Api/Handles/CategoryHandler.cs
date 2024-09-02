using Fina.Api.Infra;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handles
{
    public class CategoryHandler : ICategoryHandler
    {
        private readonly AppDbContext _appDbContext;

        public CategoryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Response<Category>> CreateAsync(CreateCategoryRequest request)
        {
            Category category = new()
            {
                Description = request.Description,
                Title = request.Title,
                UserId = request.UserId,
            };

            try
            {
                await _appDbContext.Categories.AddAsync(category);
                await _appDbContext.SaveChangesAsync();
                return new Response<Category>(category, 201, "Categoria criada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Category>(null, 500, "Não foi possível criar uma categoria!");
            }
        }

        public async Task<Response<Category>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                Category? category = await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (category is null)
                    return new Response<Category>(null, 404, "Categoria não encontrada!");

                _appDbContext.Categories.Remove(category);
                await _appDbContext.SaveChangesAsync();

                return new Response<Category>(category, 204, "Categoria removida com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Category>(null, 500, "Não foi possível remover categoria!");
            }
        }

        public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoryRequest request)
        {
            try
            {
                var query = _appDbContext.Categories
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .OrderBy(x => x.Title);

                List<Category> categories = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                int count = await query.CountAsync();

                return new PagedResponse<List<Category>>(categories, count, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<Category>>(null, 500, "Não foi possível encontrar categoria!");
            }
        }

        public async Task<Response<Category>> GetByIdAsync(GetByIdCategoryRequest request)
        {
            try
            {
                Category? category = await _appDbContext.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return category is null
                    ? new Response<Category>(null, 404, "Categoria não encontrada!")
                    : new Response<Category>(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Category>(null, 500, "Não foi possível encontrar categoria!");
            }
        }

        public async Task<Response<Category>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                Category? category = await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (category is null)
                    return new Response<Category>(null, 404, "Categoria não encontrada!");

                category.Title = request.Title;
                category.Description = request.Description;

                _appDbContext.Categories.Update(category);
                await _appDbContext.SaveChangesAsync();

                return new Response<Category>(category, message: "Categoria atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Category>(null, 500, "Não foi possível atualizar categoria!");
            }
        }
    }
}
