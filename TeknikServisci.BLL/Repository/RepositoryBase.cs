using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.DAL;

namespace TeknikServisci.BLL.Repository
{
    public abstract class RepositoryBase<T, TId> : IDisposable where T : class
    {
        internal static MyContext DbContext;
        private static DbSet<T> DbObject;

        protected RepositoryBase()
        {
            DbContext = DbContext ?? new MyContext();
            TimeSpan dd = DateTime.Now - DbContext.InstanceDate;
            if (IsDisposed) DbContext = new MyContext();
            if (dd.TotalMinutes > 30) DbContext = new MyContext();
            DbObject = DbContext.Set<T>();
        }

        public List<T> GetAll()
        {
            return DbObject.ToList();
        }

        public List<T> GetAll(Func<T, bool> predicate)
        {
            return DbObject.Where(predicate).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DbObject.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Func<T, bool> predicate)
        {
            return await DbObject.Where(predicate).AsQueryable().ToListAsync();
        }

        public T GetById(params object[] keys)
        {
            return DbObject.Find(keys);
        }

        public async Task<T> GetByIdAsync(params object[] keys)
        {
            return await DbObject.FindAsync(keys);
        }

        public int Insert(T entity)
        {
            DbObject.Add(entity);
            return DbContext.SaveChanges();
        }

        public void InsertForMark(T entity)
        {
            DbObject.Add(entity);
        }

        public async Task<int> InsertAsync(T entity)
        {
            DbObject.Add(entity);
            return await DbContext.SaveChangesAsync();
        }

        public int Delete(T entity)
        {
            DbObject.Remove(entity);
            return DbContext.SaveChanges();
        }

        public void DeleteForMark(T entity)
        {
            DbObject.Remove(entity);
        }

        public async Task<int> DeleteAsync(T entity)
        {
            DbObject.Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public int Update(T entity)
        {
            DbObject.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            //entity.UpdatedDate = DateTime.Now;
            return this.Save();
        }

        public IQueryable<T> Queryable()
        {
            return DbObject;
        }

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.IsDisposed = true;
        }
    }
}
