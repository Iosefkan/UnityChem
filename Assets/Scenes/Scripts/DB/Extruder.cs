using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class Extruder
    {
        public Extruder()
        {
            Scenarios = new HashSet<Scenario>();
        }

        public long Id { get; set; }
        public long IdType { get; set; }
        public long IdDie { get; set; }
        public long IdScrew1 { get; set; }
        public long IdBarrel { get; set; }
        public long? IdScrew2 { get; set; }
        public string Brand { get; set; }

        public virtual BarrelPossibleСonfiguration IdBarrelNavigation { get; set; }
        public virtual Die IdDieNavigation { get; set; }
        public virtual ScrewPossibleСonfiguration IdScrew1Navigation { get; set; }
        public virtual ScrewPossibleСonfiguration IdScrew2Navigation { get; set; }
        public virtual ExtruderType IdTypeNavigation { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
    }
}
