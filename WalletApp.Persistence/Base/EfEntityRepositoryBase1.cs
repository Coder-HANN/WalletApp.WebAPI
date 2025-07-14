namespace WalletApp.Persistence.Base
{
    public class EfEntityRepositoryBase<T1, T2>
    {
        private WalletDbContext context;
        public EfEntityRepositoryBase(WalletDbContext context)
        {
            this.context = context;
        }
    }
}