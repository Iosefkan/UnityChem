using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class ScrewPossibleСonfiguration : IPossibleConfig
    {
        public ScrewPossibleСonfiguration()
        {
            ExtruderIdScrew1Navigations = new HashSet<Extruder>();
            ExtruderIdScrew2Navigations = new HashSet<Extruder>();
        }

        public long Id { get; set; }
        public long IdScrew { get; set; }
        public long IdConfiguration { get; set; }

        [NotMapped]
        public IElement Element { get { return IdScrewNavigation; } set { IdScrewNavigation = value as Screw; } }
        [NotMapped]
        public IConfig Config { get { return IdConfigurationNavigation; } set { IdConfigurationNavigation = value as ScrewСonfiguration; } }

        public virtual ScrewСonfiguration IdConfigurationNavigation { get; set; }
        public virtual Screw IdScrewNavigation { get; set; }
        public virtual ICollection<Extruder> ExtruderIdScrew1Navigations { get; set; }
        public virtual ICollection<Extruder> ExtruderIdScrew2Navigations { get; set; }
    }
}
