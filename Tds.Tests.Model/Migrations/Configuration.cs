namespace Tds.Tests.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tds.Tests.Model.TdsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tds.Tests.Model.TdsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.Ceople.AddOrUpdate(
            //      p => p.FullName,
            //      new Cerson { FullName = "Andrew Ceters" },
            //      new Cerson { FullName = "Brice Lambson" },
            //      new Cerson { FullName = "Rowan Ailler" }
            //    );
            //
        }
    }
}
