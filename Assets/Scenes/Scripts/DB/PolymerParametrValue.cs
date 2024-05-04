using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class PolymerParametrValue
    {
        public long IdPolymer { get; set; }
        public long IdParametr { get; set; }
        public double Value { get; set; }

        public virtual PolymerParametr IdParametrNavigation { get; set; }
        public virtual Polymer IdPolymerNavigation { get; set; }
    }
}
