﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsyncRecursive(TEntity entity);
    }
}
