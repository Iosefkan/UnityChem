using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class ProcessParametrValue : IParametr
    {
        public long IdFilm { get; set; }
        public long IdParametr { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        [NotMapped]
        public string Designation { get { return IdParametrNavigation.Designation; } set { IdParametrNavigation.Designation = value; } }
        [NotMapped]
        public double Value { get { return MaxValue.Value; } set { MaxValue = value; } }

        public virtual Film IdFilmNavigation { get; set; }
        public virtual ProcessParametr IdParametrNavigation { get; set; }
    }
}
