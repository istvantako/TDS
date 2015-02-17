using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;

using KellermanSoftware.CompareNetObjects;

using Tds.Tests.Model.Entities;

namespace Tds.Tests.Model
{
    public class TdsContext : DbContext
    {
        #region Constructor
        public TdsContext()
            : base("name=tds-context")
        { }
        #endregion

        #region DbSets
        public DbSet<A> A { get; set; }
        public DbSet<B> B { get; set; }
        public DbSet<C> C { get; set; }
        public DbSet<D> D { get; set; }
        public DbSet<E> BD { get; set; }
        #endregion

        #region Overridden mthods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<A>().HasKey(x => x.Id);
            modelBuilder.Entity<B>().HasKey(x => new { x.Id, x.Id2 });
            modelBuilder.Entity<C>().HasKey(x => new { x.A_Id, x.Id, x.Id2 });
            modelBuilder.Entity<D>().HasKey(x => new { x.Id, x.Id2 });
            modelBuilder.Entity<E>().HasKey(x => new { x.B_Id, x.B_Id2, x.D_Id, x.D_Id2, x.Id });
        }
        #endregion

        #region Cublic helper methods
        /// <summary>
        /// Makes sure the desired entity is in the database wit the given properties.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="anySelector">Selector expression which determines whether the desired entity is already in the database or not.</param>
        public void EnsureEntity<T>(T entity, Expression<Func<T, bool>> anySelector)
            where T : class, ISelfEnsuringEntity<T>
        {
            var entitySet = Set<T>();
            entitySet.Attach(entity);

            var entityEntry = Entry<T>(entity);
            if (entitySet.Any(anySelector))
            {
                entityEntry.State = EntityState.Modified;
            }
            else
            {
                entityEntry.State = EntityState.Added;
            }
            SaveChanges();
        }

        /// <summary>
        /// Checks whether the given entity exists in the database or not.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity instance to be checked</param>
        /// <param name="anySelector">Selector expression which selects the desired entity instance from the database.</param>
        /// <returns>Logical value indicating whether the given entity exists in the database or not</returns>
        public bool CheckEntity<T>(T entity, Expression<Func<T, bool>> selector)
            where T : class, ISelfEnsuringEntity<T>
        {
            var entitySet = Set<T>();
            var entityQuery = entitySet.Where(selector);

            if (!entityQuery.Any())
            {
                return false;
            }

            var entityFromDb = entityQuery.First();
            var compareResult = new CompareLogic().Compare(entity, entityFromDb);

            return compareResult.AreEqual;
        }

        /// <summary>
        /// Deletes the given entity from the database
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        public void DeleteEntity<T>(T entity)
            where T : class
        {
            var entitySet = Set<T>();
            entitySet.Attach(entity);
            entitySet.Remove(entity);
            SaveChanges();
        }

        public void TruncateAllTables()
        {
            using (var cmd = this.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "EXEC sp_MSForEachTable 'TRUNCATE TABLE ?'";

                if (this.Database.Connection.State != ConnectionState.Open)
                {
                    this.Database.Connection.Open();
                }

                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
