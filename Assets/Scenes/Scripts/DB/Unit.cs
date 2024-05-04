using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class Unit
    {
        public Unit()
        {
            BarrelParametrs = new HashSet<BarrelParametr>();
            BarrelSectionParametrs = new HashSet<BarrelSectionParametr>();
            DieElementParametrs = new HashSet<DieElementParametr>();
            MathModelCoefficients = new HashSet<MathModelCoefficient>();
            PolymerParametrs = new HashSet<PolymerParametr>();
            ProcessParametrs = new HashSet<ProcessParametr>();
            ScrewElementParametrs = new HashSet<ScrewElementParametr>();
            ScrewParametrs = new HashSet<ScrewParametr>();
        }

        public long Id { get; set; }
        public string Designation { get; set; }

        public virtual ICollection<BarrelParametr> BarrelParametrs { get; set; }
        public virtual ICollection<BarrelSectionParametr> BarrelSectionParametrs { get; set; }
        public virtual ICollection<DieElementParametr> DieElementParametrs { get; set; }
        public virtual ICollection<MathModelCoefficient> MathModelCoefficients { get; set; }
        public virtual ICollection<PolymerParametr> PolymerParametrs { get; set; }
        public virtual ICollection<ProcessParametr> ProcessParametrs { get; set; }
        public virtual ICollection<ScrewElementParametr> ScrewElementParametrs { get; set; }
        public virtual ICollection<ScrewParametr> ScrewParametrs { get; set; }
    }
}
