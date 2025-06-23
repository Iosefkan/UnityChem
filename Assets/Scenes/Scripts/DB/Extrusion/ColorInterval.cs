using System.Collections;
using System.Collections.Generic;

namespace Database
{
    public partial class ColorInterval
    {
        public long Id { get; set; }
        public long FilmId { get; set; }
        public float MinDelE { get; set; }
        public float MaxDelE { get; set; }
        public bool IsBaseColor { get; set; }

        public float L {  get; set; }
        public float a { get; set; }
        public float b { get; set; }
        public virtual Film Film { get; set; }
    }
}
