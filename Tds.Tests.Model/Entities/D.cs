using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public class D : ClonableEntityBase<D>, ISelfEnsuringEntity<D>, IEntityWithIds<D>
    {
        public decimal Id { get; set; }
        public decimal Id2 { get; set; }
        public string Name { get; set; }

        public Expression<Func<D, bool>> GetCheckIfExistsExpression()
        {
            return x => x.Id == Id && x.Id2 == Id2;
        }

        public IEnumerable<Func<D, string>> GetListOfIdSelectors()
        {
            return IdSelectors;
        }

        private static List<Func<D, string>> IdSelectors = new List<Func<D, string>> 
        {
            x => x.Id.ToString("0.00"),
            x => x.Id2.ToString("0.00")
        };
    }
}
