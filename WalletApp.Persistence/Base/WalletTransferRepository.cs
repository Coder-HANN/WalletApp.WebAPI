﻿using System.Linq.Expressions;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Domain.Base;

namespace WalletApp.Persistence.Base
{
    public class WalletTransferRepository : IWalletTransferRepository
    {
        public WalletTransfer Add(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransfer> AddAsync(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }

        public WalletTransfer Delete(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransfer> DeleteAsync(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }

        public WalletTransfer Get(Expression<Func<WalletTransfer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WalletTransfer> GetAll(Expression<Func<WalletTransfer, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WalletTransfer>> GetAllAsync(Expression<Func<WalletTransfer, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransfer> GetAsync(Expression<Func<WalletTransfer, bool>> predicate, Func<IQueryable<WalletTransfer>, IQueryable<WalletTransfer>> include = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WalletTransfer> Query()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public WalletTransfer Update(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransfer> UpdateAsync(WalletTransfer entity)
        {
            throw new NotImplementedException();
        }
    }

}