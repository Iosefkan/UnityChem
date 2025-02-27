using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class BarrelSection : IElement
    {
        public BarrelSection()
        {
            BarrelSectionInСonfigurations = new HashSet<BarrelSectionInСonfiguration>();
            BarrelSectionParametrValues = new HashSet<BarrelSectionParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return BarrelSectionParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = BarrelSectionParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new BarrelSectionParametrValue() { IdElementNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<BarrelSectionInСonfiguration> BarrelSectionInСonfigurations { get; set; }
        public virtual ICollection<BarrelSectionParametrValue> BarrelSectionParametrValues { get; set; }
    }
}
