using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tds.Tests.Model;
using Tds.Tests.Model.Entities;

namespace Tds.Tests.SystemTests
{
    [TestClass]
    public class TdsClientLoadTests : SystemTestsBase
    {
        #region Test methods ------------------------------
        [TestMethod]
        public void LoadEntity_A_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var a = GetA(10);
            GetClientWithA().SaveEntity(GetEntityWithKey(a));
            DeleteEntity(a);

            // Act
            GetClientWithA().LoadEntity(GetEntityWithKey(a));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity<A>(a, a.GetCheckIfExistsExpression()), "A was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_LoadedIntoDatabase()
        {
            // Arrange
            var a = GetA(11);
            GetClientWithA().SaveEntity(GetEntityWithKey(a));
            var originalA = a.ShallowCopy();
            Alter(a);

            // Act
            GetClientWithA().LoadEntity(GetEntityWithKey(a));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity<A>(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_Missing_B_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(12, 1);
            var a = GetA(12, b);
            GetClientWithAWithB().SaveEntity(GetEntityWithKey(a));
            DeleteEntity(a);
            DeleteEntity(b);

            // Act
            GetClientWithAWithB().LoadEntity(GetEntityWithKey(a));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(a, a.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(b, b.GetCheckIfExistsExpression()), "B was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(13, 1);
            var a = GetA(13, b);
            GetClientWithAWithB().SaveEntity(GetEntityWithKey(a));
            var originalA = a.ShallowCopy();
            Alter(a);
            DeleteEntity(b);

            // Act
            GetClientWithAWithB().LoadEntity(GetEntityWithKey(a));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(b, b.GetCheckIfExistsExpression()), "B was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(14, 1);
            var a = GetA(14, b);
            GetClientWithAWithB().SaveEntity(GetEntityWithKey(a));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            Alter(a);
            Alter(b);

            // Act
            GetClientWithAWithB().LoadEntity(GetEntityWithKey(a));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_C_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(15, 1);
            var a = GetA(15, b);
            var c = GetC(15, 1, a);
            GetClientWithAWithB_C().SaveEntity(GetEntityWithKey(c));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            Alter(a);
            Alter(b);
            DeleteEntity(c);

            // Act
            GetClientWithAWithB_C().LoadEntity(GetEntityWithKey(c));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(c, c.GetCheckIfExistsExpression()), "C was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_C_ChangedProperties_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(15, 1);
            var a = GetA(15, b);
            var c = GetC(15, 1, a);
            GetClientWithAWithB_C().SaveEntity(GetEntityWithKey(c));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            var originalC = c.ShallowCopy();
            Alter(a);
            Alter(b);
            Alter(c);

            // Act
            GetClientWithAWithB_C().LoadEntity(GetEntityWithKey(c));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalC, originalC.GetCheckIfExistsExpression()), "C was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_C_Missing_C_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(16, 1);
            var a = GetA(16, b);
            var d = GetD(16, 1);
            var c = GetC(16, 1, a, d);
            GetClientWithAWithB_C_D().SaveEntity(GetEntityWithKey(c));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            Alter(a);
            Alter(b);
            DeleteEntity(c);
            DeleteEntity(d);

            // Act
            GetClientWithAWithB_C_D().LoadEntity(GetEntityWithKey(c));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(c, c.GetCheckIfExistsExpression()), "C was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(d, d.GetCheckIfExistsExpression()), "D was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_C_Missing_C_ChangedProperties_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(16, 1);
            var a = GetA(16, b);
            var d = GetD(16, 1);
            var c = GetC(16, 1, a, d);
            GetClientWithAWithB_C_D().SaveEntity(GetEntityWithKey(c));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            var originalD = d.ShallowCopy();
            Alter(a);
            Alter(b);
            Alter(d);
            DeleteEntity(c);

            // Act
            GetClientWithAWithB_C_D().LoadEntity(GetEntityWithKey(c));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(c, c.GetCheckIfExistsExpression()), "C was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalD, originalD.GetCheckIfExistsExpression()), "D was not loaded into the database.");
        }

        [TestMethod]
        public void LoadEntity_A_ChangedProperties_B_ChangedProperties_C_Missing_D_Missing_E_Missing_LoadedIntoDatabase()
        {
            // Arrange
            var b = GetB(17, 1);
            var a = GetA(17, b);
            var d = GetD(17, 1);
            var c = GetC(17, 1, a, d);
            var e = GetE(b, d, 1);
            GetClientWithAWithB_C_D_E().SaveEntity(GetEntityWithKey(c));
            var originalA = a.ShallowCopy();
            var originalB = b.ShallowCopy();
            Alter(a);
            Alter(b);
            DeleteEntity(c);
            DeleteEntity(d);
            DeleteEntity(e);

            // Act
            GetClientWithAWithB_C_D_E().LoadEntity(GetEntityWithKey(c));

            // Assert
            Assert.IsTrue(new TdsContext().CheckEntity(originalA, originalA.GetCheckIfExistsExpression()), "A was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(originalB, originalB.GetCheckIfExistsExpression()), "B was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(c, c.GetCheckIfExistsExpression()), "C was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(d, d.GetCheckIfExistsExpression()), "D was not loaded into the database.");
            Assert.IsTrue(new TdsContext().CheckEntity(e, e.GetCheckIfExistsExpression()), "E was not loaded into the database.");
        }
        #endregion ----------------------------------------
    }
}
