namespace Transaction.Repository
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Models.Transaction>> GetAllTransactions();

        Task<IEnumerable<Models.Transaction>> GetUserTransactions(int userID);

        void ReceiveTransaction(Models.Transaction transaction);
    }
}
