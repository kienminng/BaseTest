using BaseTest.Repository.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BaseTest.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {

        #region Private Variables
        protected IDBContext _IdbContext = null;
        protected DbSet<TEntity> _dbSet;
        protected DbContext _dbContext;

        #endregion Private Variables

        #region Public/Protected Properties
        protected DbSet<TEntity> DBSet
        {
            get
            {
                if (_dbSet == null)
                {
                     _dbSet = _dbContext.Set<TEntity>() as DbSet<TEntity>;
                }

                return _dbSet;
            }
        }
        #endregion

        #region Public Methods

        public BaseRepository(IDBContext dbContext)
        {
            _IdbContext = dbContext;
            _dbContext = (DbContext)dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = predicate != null ? DBSet.Where(predicate) : DBSet;
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = BuildQueryable(includes, predicate);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(string include, Expression<Func<TEntity, bool>> predicate = null)
        {
            List<string> includes = new List<string>();
            includes.Add(include);
            IQueryable<TEntity> query;
            if (!string.IsNullOrWhiteSpace(include))
            {
                query = BuildQueryable(includes , predicate);
                return await query.ToListAsync();
            } else
            {
                return await GetAllAsync(include, predicate);
            }

        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ? DBSet.Where(predicate) : DBSet.AsQueryable();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity,bool>> predicate)
        {
            return await DBSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            DBSet.Add(entity);
            await _IdbContext.CommitChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entity)
        {
            DBSet.AddRange(entity);
            await _IdbContext.CommitChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIDAsync(id);
            DBSet.Remove(entity);
            await _IdbContext.CommitChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await GetAsync(predicate);
            DBSet.RemoveRange(entity);
            await _IdbContext.CommitChangesAsync();
            return true;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _IdbContext.CommitChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(int id, TEntity entity)
        {
            var data = GetByIDAsync(id);
            if (data == null) return entity;
            return await UpdateAsync(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, params object[] keyValues)
        {
            var data = await DBSet.FindAsync(keyValues);
            if (data != null)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _IdbContext.CommitChangesAsync();
                return entity;
            }
            return entity;
        }

        public async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            await _IdbContext.CommitChangesAsync();
            return entities;
        }

        public async Task<TEntity> GetByIDAsync(int id)
        {
            return await DBSet.FindAsync(id);
        }

        public async Task<TEntity> GetByIDAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = BuildQueryable(includes , predicate);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> ExecuteStoredProcedureAsync(string spName, params object[] parameters)
        {
            var resultSet = _dbContext.Set<TEntity>().FromSqlRaw(spName, parameters);
            await _IdbContext.CommitChangesAsync();
            return await resultSet.ToListAsync();
        }

        public async Task<List<TEntity>> ExecuteStoredProcedureWithSqlParamListAsync(string spName, List<SqlParameter> parameters)
        {
            var resultSet = _dbContext.Set<TEntity>().FromSqlRaw(spName, parameters);
            await _IdbContext.CommitChangesAsync();

            return await resultSet.ToListAsync();
        }

        public async Task<IQueryable<TEntity>> SqlQueryAsync(string query, params object[] parameters)
        {
            var resultSet = DBSet.FromSqlRaw(query, parameters);
            await _IdbContext.CommitChangesAsync();

            return resultSet.AsQueryable();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = predicate != null ? DBSet.Where(predicate) : DBSet;
            return await query.CountAsync();
        }

        public async Task<int> CountAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = BuildQueryable(includes, predicate);
            return await query.CountAsync();
        }

        public async Task<int> CountAsync(string include, Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query;
            if (!string.IsNullOrWhiteSpace(include))
            {
                query = BuildQueryable(new List<string> { include }, predicate);
                return await query.CountAsync();
            }
            else
            {
                return await CountAsync(predicate);
            }
        }

        public void ClearTrackedChanges()
        {
            var changedEntriesCopy = _dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }
        }

        public async Task ExecuteStoredProcedureNoReturnAsync(string spName, params object[] parameters)
        {
            _dbContext.Database.ExecuteSqlRaw(spName, parameters);
            await _IdbContext.CommitChangesAsync();
        }

        public async Task<T> ExecuteStoredProcedureScalarAsync<T>(string procedureName, List<SqlParameter> parameters)
        {
            var connection = _dbContext.Database.GetDbConnection();

            SqlCommand procedure = new SqlCommand(procedureName, (SqlConnection)connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (SqlParameter parameter in parameters)
            {
                procedure.Parameters.Add(parameter.ParameterName, parameter.SqlDbType);
                procedure.Parameters[parameter.ParameterName].Value = parameter.Value;
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            T result = (T)procedure.ExecuteScalar();
            connection.Close();

            return result;
        }

        #endregion Public Methods

        protected IQueryable<TEntity> BuildQueryable(List<string> includes, Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = DBSet.AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null && includes.Count > 0)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include.Trim());
                }
            }

            return query;
        }


    }

}
