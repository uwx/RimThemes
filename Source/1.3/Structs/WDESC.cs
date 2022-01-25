using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace aRandomKiwi.RimThemes
{
    internal readonly struct WDESC
    {
        public readonly int type;
        public readonly int wid;

        public WDESC(int type, int wid)
        {
            this.type = type;
            this.wid = wid;
        }
    }
}
