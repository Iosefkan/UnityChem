using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class BarrelPossibleСonfiguration : IPossibleConfig
    {
        public BarrelPossibleСonfiguration()
        {
            Extruders = new HashSet<Extruder>();
        }

        public long Id { get; set; }
        public long IdBody { get; set; }
        public long? IdConfiguration { get; set; }

        [NotMapped]
        public IElement Element { get { return IdBodyNavigation; } set { IdBodyNavigation = value as Barrel; } }
        [NotMapped]
        public IConfig Config { get { return IdConfigurationNavigation; } set { IdConfigurationNavigation = value as BarrelСonfiguration; } }

        public virtual Barrel IdBodyNavigation { get; set; }
        public virtual BarrelСonfiguration IdConfigurationNavigation { get; set; }
        public virtual ICollection<Extruder> Extruders { get; set; }
    }
}
