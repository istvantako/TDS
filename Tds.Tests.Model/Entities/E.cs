using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public class E : ClonableEntityBase<E>, ISelfEnsuringEntity<E>, IEntityWithIds<E>
    {
        public decimal B_Id { get; set; }
        public decimal B_Id2 { get; set; }
        public decimal D_Id { get; set; }
        public decimal D_Id2 { get; set; }
        public decimal Id { get; set; }

        public Expression<Func<E, bool>> GetCheckIfExistsExpression()
        {
            return x => x.B_Id == B_Id && x.B_Id2 == B_Id2 &&
                x.D_Id == D_Id && x.D_Id2 == D_Id2 && x.Id == Id;
        }

        public System.Collections.Generic.IEnumerable<Func<E, string>> GetListOfIdSelectors()
        {
            return IdSelectors;
        }

        private static List<Func<E, string>> IdSelectors = new List<Func<E, string>> 
        {
            x => x.B_Id.ToString("0.00"),
            x => x.B_Id2.ToString("0.00"),
            x => x.D_Id.ToString("0.00"),
            x => x.D_Id2.ToString("0.00"),
            x => x.Id.ToString("0.00")
        };
    }
}
