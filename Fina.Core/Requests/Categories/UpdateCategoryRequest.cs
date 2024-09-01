using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Transactions
{
    public class UpdateCategoryRequest : Request
    {
        [Required(ErrorMessage = "Id inválido.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Título inválido.")]
        [MaxLength(80, ErrorMessage = "O título deve conter até 80 caracteres.")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
