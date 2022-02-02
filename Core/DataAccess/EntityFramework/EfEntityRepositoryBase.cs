using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>: IEntityRepository<TEntity>
        where TEntity: class, IEntity, new()
        where TContext: DbContext, new()
    {
        //NuGet
        public void Add(TEntity entity)
        {
            //IDispossable pattern implementation of c#
            //using : using yapısı bittiği anda garbage collector belleği hızlıca temizler.
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity); //gönderdiğim product'ı veri kaynağı ile ilişkilendir
                addedEntity.State = EntityState.Added;   //referansı alınan product'ı ekleme durumuna getir
                context.SaveChanges();                   //ve ekle şimdi
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);  //filtreyi oraya uygula
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null                                   //filtre verilmediyse
                    ? context.Set<TEntity>().ToList()                   //Product tablosuna yerleş ve listeye çevir.
                    : context.Set<TEntity>().Where(filter).ToList();    //verildiyse filtreleyip listele
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
