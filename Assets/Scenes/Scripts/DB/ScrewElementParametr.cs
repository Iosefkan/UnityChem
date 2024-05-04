using System;
using System.Collections.Generic;

#nullable disable

namespace Database
{
    public partial class ScrewElementParametr
    {
        public ScrewElementParametr()
        {
            ScrewElementParametrValues = new HashSet<ScrewElementParametrValue>();
            //ScrewElements = new HashSet<ScrewElement>();
        }

        public long Id { get; set; }
        public long IdUnit { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        
        public virtual Unit IdUnitNavigation { get; set; }
        public virtual ICollection<ScrewElementParametrValue> ScrewElementParametrValues { get; set; }
        //public virtual ICollection<ScrewElement> ScrewElements { get; set; }
    }
}
