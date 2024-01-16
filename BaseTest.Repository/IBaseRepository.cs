using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<IEnumerable<TEntity>> GetAllAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate = null);
        Task<IEnumerable<TEntity>> GetAllAsync(string include, Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity,bool>> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, params Object[] keyValues);
        Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities);
        Task<TEntity> GetByIDAsync(int id);
        Task<TEntity> GetByIDAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> ExecuteStoredProcedureAsync(string spName, params object[] parameters);
        Task<List<TEntity>> ExecuteStoredProcedureWithSqlParamListAsync(string spName, List<SqlParameter> parameters);
        Task<IQueryable<TEntity>> SqlQueryAsync(string query, params object[] parameters);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(string include, Expression<Func<TEntity, bool>> predicate = null);
        void ClearTrackedChanges();
        Task<T> ExecuteStoredProcedureScalarAsync<T>(string procedureName, List<SqlParameter> parameters);
        Task ExecuteStoredProcedureNoReturnAsync(string spName, params object[] parameters);
    }
}
