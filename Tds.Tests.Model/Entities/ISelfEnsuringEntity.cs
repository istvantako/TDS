using System;
using System.Linq.Expressions;

namespace Tds.Tests.Model.Entities
{
    public interface ISelfEnsuringEntity<T>
        where T : class
    {
        Expression<Func<T, bool>> GetCheckIfExistsExpression();
    }
}
