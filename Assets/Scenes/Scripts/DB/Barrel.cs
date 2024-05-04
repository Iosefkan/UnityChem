using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Barrel : IElement
    {
        public Barrel()
        {
            BarrelParametrValues = new HashSet<BarrelParametrValue>();
            BarrelPossibleСonfigurations = new HashSet<BarrelPossibleСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public List<IParametr> GetParametrs()
        {
            return BarrelParametrValues.ToList<IParametr>();
        }

        public virtual ICollection<BarrelParametrValue> BarrelParametrValues { get; set; }
        public virtual ICollection<BarrelPossibleСonfiguration> BarrelPossibleСonfigurations { get; set; }
    }
}
