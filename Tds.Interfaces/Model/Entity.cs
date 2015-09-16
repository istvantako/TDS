﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Interfaces.Model
{
    public class Entity : IEntity
    {
        public string Name { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}
