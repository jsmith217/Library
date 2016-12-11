using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb
{
    public class Pair
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Condition { get; set; }

        public Pair(string Key, string condiiton, string value)
        {
            this.Key = Key;
            this.Value = value;
            this.Condition = condiiton;
        }

        public string BuildCondition()
        {
            return String.Concat(this.Key, this.Condition, this.Value);
        }
    }
}