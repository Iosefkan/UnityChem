using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Screw : IElement
    {
        public Screw()
        {
            ScrewParametrValues = new HashSet<ScrewParametrValue>();
            ScrewPossibleСonfigurations = new HashSet<ScrewPossibleСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public List<IParametr> GetParametrs()
        {
            return ScrewParametrValues.ToList<IParametr>();
        }

        public virtual ICollection<ScrewParametrValue> ScrewParametrValues { get; set; }
        public virtual ICollection<ScrewPossibleСonfiguration> ScrewPossibleСonfigurations { get; set; }
    }
}
