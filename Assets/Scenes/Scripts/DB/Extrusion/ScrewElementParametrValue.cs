using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class ScrewElementParametrValue : IParametr
    {
        public long IdElement { get; set; }
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

        public virtual ScrewElement IdElementNavigation { get; set; }
        public virtual ScrewElementParametr IdParametrNavigation { get; set; }
    }
}
