using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class DieElementParametr : IPar
    {
        public DieElementParametr()
        {
            DieElementParametrValues = new HashSet<DieElementParametrValue>();
        }

        public long Id { get; set; }
        public long IdUnit { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }

        public virtual Unit IdUnitNavigation { get; set; }
        public virtual ICollection<DieElementParametrValue> DieElementParametrValues { get; set; }
    }
}
