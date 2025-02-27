using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Barrel : IElement
    {
        public Barrel()
        {
            BarrelParametrValues = new HashSet<BarrelParametrValue>();
            BarrelPossibleСonfigurations = new HashSet<BarrelPossibleСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return BarrelParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = BarrelParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new BarrelParametrValue() { IdBarrelNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<BarrelParametrValue> BarrelParametrValues { get; set; }
        public virtual ICollection<BarrelPossibleСonfiguration> BarrelPossibleСonfigurations { get; set; }
    }
}
