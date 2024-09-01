using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories
{
    public class GetByIdCategoryRequest : Request
    {
        [Required(ErrorMessage = "Id inválido.")]
        public long Id { get; set; }
    }
}
