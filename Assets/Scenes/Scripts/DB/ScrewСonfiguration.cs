using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public List<IConfigElement> ConfigElements
        {
            get
            {
                return ScrewElementInСonfigurations.ToList<IConfigElement>();
            }
            set
            {
                var collect = ScrewElementInСonfigurations;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new ScrewElementInСonfiguration() { IdConfigurationNavigation = this });
                }

                int i = 0;
                foreach (var element in collect) 
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<ScrewElementInСonfiguration> ScrewElementInСonfigurations { get; set; }
        public virtual ICollection<ScrewPossibleСonfiguration> ScrewPossibleСonfigurations { get; set; }
    }
}
