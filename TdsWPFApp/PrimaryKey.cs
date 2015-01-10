using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TdsWPFApp
{
    internal class PrimaryKey
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public PrimaryKey(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
