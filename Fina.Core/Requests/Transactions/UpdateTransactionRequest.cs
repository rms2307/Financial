﻿using Fina.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories
{
    public class UpdateTransactionRequest : Request
    {
        [Required(ErrorMessage = "Id inválido.")]
        public long Id { get; set; }
        [Required(ErrorMessage = "Título inválido.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo inválido.")]
        public TransactionType Type { get; set; } = TransactionType.Withdraw;

        [Required(ErrorMessage = "Valor inválido.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Categoria inválida.")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Data inválida.")]
        public DateTime PaidOrReceivedAt { get; set; }
    }
}
