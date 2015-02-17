using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public class B : ClonableEntityBase<B>, ISelfEnsuringEntity<B>, IEntityWithIds<B>
    {
        public decimal Id { get; set; }
        public decimal Id2 { get; set; }
        public string Name { get; set; }

        public Expression<Func<B, bool>> GetCheckIfExistsExpression()
        {
            return x => x.Id == Id && x.Id2 == Id2;
        }

        public IEnumerable<Func<B, string>> GetListOfIdSelectors()
        {
            return IdSelectors;
        }

        private static List<Func<B, string>> IdSelectors = new List<Func<B, string>> 
        {
            x => x.Id.ToString("0.00"),
            x => x.Id2.ToString("0.00")
        };
    }
}
