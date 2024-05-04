using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class BarrelSection : IElement
    {
        public BarrelSection()
        {
            BarrelSectionInСonfigurations = new HashSet<BarrelSectionInСonfiguration>();
            BarrelSectionParametrValues = new HashSet<BarrelSectionParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public List<IParametr> GetParametrs()
        {
            return BarrelSectionParametrValues.ToList<IParametr>();
        }

        public virtual ICollection<BarrelSectionInСonfiguration> BarrelSectionInСonfigurations { get; set; }
        public virtual ICollection<BarrelSectionParametrValue> BarrelSectionParametrValues { get; set; }
    }
}
