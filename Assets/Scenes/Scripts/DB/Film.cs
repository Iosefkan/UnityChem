using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class Film
    {
        public Film()
        {
            ProcessParametrValues = new HashSet<ProcessParametrValue>();
            Scenarios = new HashSet<Scenario>();
        }

        public long Id { get; set; }
        public long? IdPolymer { get; set; }
        public string Type { get; set; }

        public virtual Polymer IdPolymerNavigation { get; set; }
        public virtual ICollection<ProcessParametrValue> ProcessParametrValues { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
    }
}
