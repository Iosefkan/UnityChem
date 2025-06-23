using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public List<IConfigElement> ConfigElements
        {
            get
            {
                return BarrelSectionInСonfigurations.ToList<IConfigElement>();
            }
            set
            {
                var collect = BarrelSectionInСonfigurations;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new BarrelSectionInСonfiguration() { IdConfigurationNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<BarrelPossibleСonfiguration> BarrelPossibleСonfigurations { get; set; }
        public virtual ICollection<BarrelSectionInСonfiguration> BarrelSectionInСonfigurations { get; set; }
    }
}
