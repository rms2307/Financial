using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Transactions
{
    public class GetByIdTransactionRequest : Request
    {
        [Required(ErrorMessage = "Id inválido.")]
        public long Id { get; set; }
    }
}
