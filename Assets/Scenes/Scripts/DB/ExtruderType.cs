using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class ExtruderType
    {
        public ExtruderType()
        {
            Extruders = new HashSet<Extruder>();
        }

        public long Id { get; set; }
        public long? IdModel { get; set; }
        public string Name { get; set; }

        public virtual MathModel IdModelNavigation { get; set; }
        public virtual ICollection<Extruder> Extruders { get; set; }
    }
}
