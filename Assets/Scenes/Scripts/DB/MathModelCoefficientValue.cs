using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class MathModelCoefficientValue
    {
        public long IdModel { get; set; }
        public long IdCoefficient { get; set; }
        public long IdPolymer { get; set; }
        public double Value { get; set; }

        public virtual MathModelCoefficient IdCoefficientNavigation { get; set; }
        public virtual MathModel IdModelNavigation { get; set; }
        public virtual Polymer IdPolymerNavigation { get; set; }
    }
}
