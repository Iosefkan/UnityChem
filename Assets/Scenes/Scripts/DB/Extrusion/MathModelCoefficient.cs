using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class MathModelCoefficient
    {
        public MathModelCoefficient()
        {
            MathModelCoefficientValues = new HashSet<MathModelCoefficientValue>();
        }

        public long Id { get; set; }
        public long IdUnit { get; set; }
        public long Designation { get; set; }
        public string Name { get; set; }

        public virtual Unit IdUnitNavigation { get; set; }
        public virtual ICollection<MathModelCoefficientValue> MathModelCoefficientValues { get; set; }
    }
}
