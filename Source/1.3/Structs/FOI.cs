using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace aRandomKiwi.RimThemes
{
    public readonly struct FOI
    {
        public readonly string field;
        public readonly BindingFlags bf;

        public FOI(string field, BindingFlags bf = BindingFlags.Public | BindingFlags.Static)
        {
            this.field = field;
            this.bf = bf;
        }
    }
}
