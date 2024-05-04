using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class MathModel
    {
        public MathModel()
        {
            ExtruderTypes = new HashSet<ExtruderType>();
            MathModelCoefficientValues = new HashSet<MathModelCoefficientValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ExtruderType> ExtruderTypes { get; set; }
        public virtual ICollection<MathModelCoefficientValue> MathModelCoefficientValues { get; set; }
    }
}
