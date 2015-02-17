using System;
using System.Collections.Generic;

namespace Tds.Tests.Model.Entities
{
    public interface IEntityWithIds<T>
        where T : class
    {
        IEnumerable<Func<T, string>> GetListOfIdSelectors();
    }
}
