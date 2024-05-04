using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class ProcessParametrValue
    {
        public long IdFilm { get; set; }
        public long IdParametr { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
        public virtual ProcessParametr IdParametrNavigation { get; set; }
    }
}
