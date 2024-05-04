using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class BarrelParametrValue : IParametr
    {
        public long IdBarrel { get; set; }
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

        public virtual Barrel IdBarrelNavigation { get; set; }
        public virtual BarrelParametr IdParametrNavigation { get; set; }
    }
}
