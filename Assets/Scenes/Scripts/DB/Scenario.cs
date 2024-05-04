using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class Scenario
    {
        public long Id { get; set; }
        public long IdFilm { get; set; }
        public long IdExtruder { get; set; }
        public string Name { get; set; }
        public double Throughput { get; set; }

        public virtual Extruder IdExtruderNavigation { get; set; }
        public virtual Film IdFilmNavigation { get; set; }
    }
}
