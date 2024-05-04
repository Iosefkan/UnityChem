using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class ScrewParametrValue : IParametr
    {
        public long IdScrew { get; set; }
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

        public virtual ScrewParametr IdParametrNavigation { get; set; }
        public virtual Screw IdScrewNavigation { get; set; }
    }
}
