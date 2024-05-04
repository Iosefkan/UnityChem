using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class Polymer
    {
        public Polymer()
        {
            Films = new HashSet<Film>();
            MathModelCoefficientValues = new HashSet<MathModelCoefficientValue>();
            PolymerParametrValues = new HashSet<PolymerParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Film> Films { get; set; }
        public virtual ICollection<MathModelCoefficientValue> MathModelCoefficientValues { get; set; }
        public virtual ICollection<PolymerParametrValue> PolymerParametrValues { get; set; }
    }
}
