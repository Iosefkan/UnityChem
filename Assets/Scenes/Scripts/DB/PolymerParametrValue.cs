using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class PolymerParametrValue : IParametr
    {
        public long IdPolymer { get; set; }
        public long IdParametr { get; set; }
        public double Value { get; set; }

        [NotMapped]
        public string Designation 
        {
            get
            {
                return IdParametrNavigation.Designation;
            }
            set
            {
                IdParametrNavigation.Designation = value;
            }
        }

        public virtual PolymerParametr IdParametrNavigation { get; set; }
        public virtual Polymer IdPolymerNavigation { get; set; }
    }
}
