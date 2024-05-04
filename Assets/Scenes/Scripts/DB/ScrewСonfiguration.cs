using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class ScrewСonfiguration : IConfig
    {
        public ScrewСonfiguration()
        {
            ScrewElementInСonfigurations = new HashSet<ScrewElementInСonfiguration>();
            ScrewPossibleСonfigurations = new HashSet<ScrewPossibleСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public List<IConfigElement> GetConfigElements()
        {
            return ScrewElementInСonfigurations.ToList<IConfigElement>();
        }

        public virtual ICollection<ScrewElementInСonfiguration> ScrewElementInСonfigurations { get; set; }
        public virtual ICollection<ScrewPossibleСonfiguration> ScrewPossibleСonfigurations { get; set; }
    }
}
