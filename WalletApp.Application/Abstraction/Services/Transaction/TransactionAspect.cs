using Castle.DynamicProxy;

namespace WalletApp.Application.Abstraction.Services.Transaction
{
    public class TransactionAspect : IInterceptor
    {
        private readonly ITransactionService _transactionService;

        public TransactionAspect(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                _transactionService.Begin();

                invocation.Proceed();
                _transactionService.Commit();
            }
            catch (Exception)
            {
                _transactionService.Rollback();
                throw;
            }
        }
    }
}
