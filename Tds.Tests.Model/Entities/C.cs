using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public class C : ClonableEntityBase<C>, ISelfEnsuringEntity<C>, IEntityWithIds<C>
    {
        public decimal A_Id { get; set; }
        public decimal Id { get; set; }
        public decimal Id2 { get; set; }
        public string Name { get; set; }
        public decimal D_Id { get; set; }
        public decimal D_Id2 { get; set; }

        public Expression<Func<C, bool>> GetCheckIfExistsExpression()
        {
            return x => x.A_Id == A_Id && x.Id == Id && x.Id2 == Id2;
        }

        public IEnumerable<Func<C, string>> GetListOfIdSelectors()
        {
            return IdSelectors;
        }

        private static List<Func<C, string>> IdSelectors = new List<Func<C, string>>
        {
            x => x.A_Id.ToString("0.00"),
            x => x.Id.ToString("0.00"),
            x => x.Id2.ToString("0.00")
        };
    }
}
