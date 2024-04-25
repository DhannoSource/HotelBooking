
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Transaction.Data;
using Transaction.Models;

namespace Transaction.Repository
{
    public class TransactionRespository : ITransactionRepository
    {
        private readonly DbContextOptions<TransactionDbContext> _dbContextOptions;

        public TransactionRespository(DbContextOptions<TransactionDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public async Task<IEnumerable<Models.Transaction>> GetAllTransactions()
        {
            using (var _transactionDbContext = new TransactionDbContext(_dbContextOptions))
            {
                return await _transactionDbContext.Transaction.ToListAsync();
            }
        }

        public async Task<IEnumerable<Models.Transaction>> GetUserTransactions(int userID)
        {
            using (var _transactionDbContext = new TransactionDbContext(_dbContextOptions))
            {
                return await _transactionDbContext.Transaction.Where(t => t.UserId == userID).ToListAsync();
            }
        }

        public void ReceiveTransaction(Models.Transaction transaction)
        {
            using (var _transactionDbContext = new TransactionDbContext(_dbContextOptions))
            {
                _transactionDbContext.Transaction.Add(transaction);
                _transactionDbContext.SaveChanges();
            }
                           
        }
    }
}
