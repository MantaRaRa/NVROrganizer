﻿using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int Id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
    }
}