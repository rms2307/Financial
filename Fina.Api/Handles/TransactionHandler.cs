using Fina.Api.Infra;
using Fina.Core.Common;
using Fina.Core.Enums;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handles
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly AppDbContext _context;

        public TransactionHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Transaction>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: TransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                Transaction transaction = new()
                {
                    UserId = request.UserId,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return new Response<Transaction>(transaction, 201, "Transação criada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Transaction>(null, 500, "Não foi possível criar sua transação");
            }
        }

        public async Task<Response<Transaction>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: TransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                Transaction? transaction = await _context
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction>(null, 404, "Transação não encontrada");

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();

                return new Response<Transaction>(transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Transaction>(null, 500, "Não foi possível atualizar sua transação");
            }
        }

        public async Task<Response<Transaction>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                Transaction? transaction = await _context
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction>(null, 404, "Transação não encontrada");

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                return new Response<Transaction>(transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Transaction>(null, 500, "Não foi possível remover sua transação");
            }
        }

        public async Task<Response<Transaction>> GetByIdAsync(GetByIdTransactionRequest request)
        {
            try
            {
                Transaction? transaction = await _context
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transaction>(null, 404, "Transação não encontrada")
                    : new Response<Transaction>(transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response<Transaction>(null, 500, "Não foi possível recuperar sua transação");
            }
        }

        public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDayMonth();
                request.EndDate ??= DateTime.Now.GetLastDayMonth();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<Transaction>>(null, 500, "Não foi possível determinar a data de início ou término");
            }

            try
            {
                var query = _context
                    .Transactions
                    .AsNoTracking()
                    .Where(x =>
                        x.PaidOrReceivedAt >= request.StartDate &&
                        x.PaidOrReceivedAt <= request.EndDate &&
                        x.UserId == request.UserId)
                    .OrderBy(x => x.PaidOrReceivedAt);

                List<Transaction> transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction>>(transactions, count, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<List<Transaction>>(null, 500, "Não foi possível obter as transações");
            }
        }
    }
}
