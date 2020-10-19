using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {

        protected readonly MeuDbContext _db;

        /// <summary>
        /// Atalho para DbSet
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(MeuDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            DbSet = _db.Set<TEntity>();
        }

        public async Task<List<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            ///Sempre que se le um objeto ele cria um tracking para monitorar alteracoes.
            ///Se eu nao for alterar o objeto, AsNoTracking aumenta performance.
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        } //Buscar

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        } //ObterPorId

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        } //ObterTodos


        public virtual async Task Adicionar(TEntity entity)
        {
            //_db.Set<TEntity>().Add(entity);
            DbSet.Add(entity);
            await SaveChanges();
        } //Adicionar



        public virtual async Task Atualizar(TEntity entity)
        {
            //try
            {
                _db.Entry(entity).State = EntityState.Modified;
                DbSet.Update(entity);
                await SaveChanges();
            }
            //catch (System.InvalidOperationException e)
            {
            //    var x = e;
                //var originalEntity = Find(entity.GetType(), ((IEntity)entity).Id);
                //Entry(originalEntity).CurrentValues.SetValues(entity);
                //return Entry((TEntity)originalEntity);
            }
        }


        public virtual async Task Remover(Guid id)
        {

            ///O argumento new() na declaracao eh que permite a criacao de um new TEntity 
            TEntity entity = new TEntity { Id = id };
            DbSet.Remove(entity);
            await SaveChanges();
            
        } //Remover

        public async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        } //SaveChanges

        public virtual void Dispose()
        {
            _db?.Dispose();
        } //Dispose


    }
}
