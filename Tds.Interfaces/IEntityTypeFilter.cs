﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Interfaces
{
    public interface IEntityTypeFilter
    {
        bool IsSkipped(string entityName);
    }
}
