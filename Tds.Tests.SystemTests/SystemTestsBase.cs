using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

using Tds.Tests.Model;
using Tds.Tests.Model.Entities;
using TestDataSeeding.Client;
using TestDataSeeding.Model;

namespace Tds.Tests.SystemTests
{
    [TestClass]
    public abstract class SystemTestsBase
    {
        #region Test initialization -----------------------
        [TestInitialize]
        public void InitializeTest()
        {
            using (var ctx = new TdsContext())
            {
                ctx.TruncateAllTables();
            }

            foreach (var filePath in Directory.GetFiles(TestSettings.Storage.EntitiesLocation))
            {
                File.Delete(filePath);
            }
        }
        #endregion ----------------------------------------

        #region Helper methods ----------------------------
        /// <summary>
        /// Gets a new instance of A with the given properties.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="b">B</param>
        /// <param name="ensureInDb">Ensure in db or not, by default it is true</param>
        /// <returns></returns>
        protected A GetA(int id, B b = null, bool ensureInDb = true)
        {
            A entity;
            if (b == null)
            {
                entity = new A { Id = id, Name = string.Format("A_{0}", id), B_Id = id, B_Id2 = 1 };
            }
            else
            {
                entity = new A { Id = id, Name = string.Format("A_{0}", id), B_Id = b.Id, B_Id2 = b.Id2 };
            }

            if (ensureInDb)
            {
                using (var ctx = new TdsContext())
                {
                    entity.Ensure(ctx);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets a new B instance with the given properties.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="id2">Id2</param>
        /// <param name="ensureInDb">Ensure in db or not, by default it is true</param>
        /// <returns></returns>
        protected B GetB(int id, int id2, bool ensureInDb = true)
        {
            var entity = new B { Id = id, Id2 = id2, Name = string.Format("B_{0}_{1}", id, id2) };

            if (ensureInDb)
            {
                using (var ctx = new TdsContext())
                {
                    entity.Ensure(ctx);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets a new instance of C with the given properties.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="id2">Id2</param>
        /// <param name="a">A</param>
        /// <param name="d">D</param>
        /// <param name="ensureInDb">Ensure in db or not, by default it is true</param>
        /// <returns></returns>
        protected C GetC(int id, int id2, A a, D d = null, bool ensureInDb = true)
        {
            C entity;
            if (d == null)
            {
                entity = new C { A_Id = a.Id, Id = id, Id2 = id2, Name = string.Format("C_{0}_{1}_{2}", a.Id, id, id2), D_Id = id, D_Id2 = 1 };
            }
            else
            {
                entity = new C { A_Id = a.Id, Id = id, Id2 = id2, Name = string.Format("C_{0}_{1}_{2}", a.Id, id, id2), D_Id = d.Id, D_Id2 = d.Id2 };
            }

            if (ensureInDb)
            {
                using (var ctx = new TdsContext())
                {
                    entity.Ensure(ctx);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets a new D instance with the given properties.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="id2">Id2</param>
        /// <param name="ensureInDb">Ensure in db or not, by default it is true</param>
        /// <returns></returns>
        protected D GetD(int id, int id2, bool ensureInDb = true)
        {
            var entity = new D { Id = id, Id2 = id2, Name = string.Format("D_{0}_{1}", id, id2) };

            if (ensureInDb)
            {
                using (var ctx = new TdsContext())
                {
                    entity.Ensure(ctx);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets a new instance of E with the given properties.
        /// </summary>
        /// <param name="b">B</param>
        /// <param name="d">D</param>
        /// <param name="id">Id</param>
        /// <param name="ensureInDb">Ensure in db or not, by default it is true</param>
        /// <returns></returns>
        protected E GetE(B b, D d, int id, bool ensureInDb = true)
        {
            var entity = new E { B_Id = b.Id, B_Id2 = b.Id2, D_Id = d.Id, D_Id2 = d.Id2, Id = id };

            if (ensureInDb)
            {
                using (var ctx = new TdsContext())
                {
                    entity.Ensure(ctx);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets an entity with key instance about the given entity instance.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        protected EntityWithKey GetEntityWithKey(A entity)
        {
            return new EntityWithKey()
            {
                EntityName = "A",
                PrimaryKeyValues = entity.GetListOfIdSelectors().Select(x => x.Invoke(entity)).ToList()
            };
        }

        /// <summary>
        /// Gets an entity with key instance about the given entity instance.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        protected EntityWithKey GetEntityWithKey(C entity)
        {
            return new EntityWithKey()
            {
                EntityName = "C",
                PrimaryKeyValues = entity.GetListOfIdSelectors().Select(x => x.Invoke(entity)).ToList()
            };
        }

        /// <summary>
        /// Gets a new tds client instance with a specific configuration.
        /// </summary>
        /// <returns></returns>
        protected TdsClient GetClientWithA()
        {
            return new TdsClient(TestSettings.Storage.Location, "Structure_A_WithNoDependencies.xml");
        }

        /// <summary>
        /// Gets a new tds client instance with a specific configuration.
        /// </summary>
        /// <returns></returns>
        protected TdsClient GetClientWithAWithB()
        {
            return new TdsClient(TestSettings.Storage.Location, "Structure_A_WithB.xml");
        }

        /// <summary>
        /// Gets a new tds client instance with a specific configuration.
        /// </summary>
        /// <returns></returns>
        protected TdsClient GetClientWithAWithB_C()
        {
            return new TdsClient(TestSettings.Storage.Location, "Structure_A_WithB_C.xml");
        }

        /// <summary>
        /// Gets a new tds client instance with a specific configuration.
        /// </summary>
        /// <returns></returns>
        protected TdsClient GetClientWithAWithB_C_D()
        {
            return new TdsClient(TestSettings.Storage.Location, "Structure_A_WithB_C_D.xml");
        }

        /// <summary>
        /// Gets a new tds client instance with a specific configuration.
        /// </summary>
        /// <returns></returns>
        protected TdsClient GetClientWithAWithB_C_D_E()
        {
            return new TdsClient(TestSettings.Storage.Location, "Structure_A_WithB_C_D_E.xml");
        }

        /// <summary>
        /// Deletes the given entity from the database
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity instance</param>
        protected void DeleteEntity<T>(T entity)
            where T : class
        {
            using (var ctx = new TdsContext())
            {
                ctx.DeleteEntity(entity);
            }
        }

        /// <summary>
        /// Modifies all non key properties of the given objeD.
        /// </summary>
        /// <param name="entity"></param>
        protected void Alter(A entity)
        {
            entity.B_Id++;
            entity.B_Id2++;
            entity.Name += "modified";

            using (var ctx = new TdsContext())
            {
                ctx.EnsureEntity(entity, entity.GetCheckIfExistsExpression());
            }
        }

        /// <summary>
        /// Modifies all non key properties of the given objeD.
        /// </summary>
        /// <param name="entity"></param>
        protected void Alter(B entity)
        {
            entity.Name += "modified";

            using (var ctx = new TdsContext())
            {
                ctx.EnsureEntity(entity, entity.GetCheckIfExistsExpression());
            }
        }

        /// <summary>
        /// Modifies all non key properties of the given objeD.
        /// </summary>
        /// <param name="entity"></param>
        protected void Alter(C entity)
        {
            entity.Name += "modified";
            entity.D_Id += 1;
            entity.D_Id2 += 1;

            using (var ctx = new TdsContext())
            {
                ctx.EnsureEntity(entity, entity.GetCheckIfExistsExpression());
            }
        }

        /// <summary>
        /// Modifies all non key properties of the given objeD.
        /// </summary>
        /// <param name="entity"></param>
        protected void Alter(D entity)
        {
            entity.Name += "modified";

            using (var ctx = new TdsContext())
            {
                ctx.EnsureEntity(entity, entity.GetCheckIfExistsExpression());
            }
        }
        #endregion ----------------------------------------
    }
}
