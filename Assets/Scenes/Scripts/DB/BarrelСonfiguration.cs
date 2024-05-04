using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class BarrelСonfiguration : IConfig
    {
        public BarrelСonfiguration()
        {
            BarrelPossibleСonfigurations = new HashSet<BarrelPossibleСonfiguration>();
            BarrelSectionInСonfigurations = new HashSet<BarrelSectionInСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public List<IConfigElement> GetConfigElements()
        {
            return BarrelSectionInСonfigurations.ToList<IConfigElement>();
        }

        public virtual ICollection<BarrelPossibleСonfiguration> BarrelPossibleСonfigurations { get; set; }
        public virtual ICollection<BarrelSectionInСonfiguration> BarrelSectionInСonfigurations { get; set; }
    }
}
