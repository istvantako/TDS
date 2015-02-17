using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public class A : ClonableEntityBase<A>, ISelfEnsuringEntity<A>, IEntityWithIds<A>
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal B_Id { get; set; }
        public decimal B_Id2 { get; set; }

        public Expression<Func<A, bool>> GetCheckIfExistsExpression()
        {
            return x => x.Id == Id;
        }

        public IEnumerable<Func<A, string>> GetListOfIdSelectors()
        {
            return IdSelectors;
        }

        private static List<Func<A, string>> IdSelectors = new List<Func<A, string>> 
        {
            x => x.Id.ToString("0.00")
        };
    }
}
