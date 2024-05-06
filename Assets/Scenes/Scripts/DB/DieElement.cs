using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class DieElement : IElement
    {
        public DieElement()
        {
            DieElementInСonfigurations = new HashSet<DieElementInСonfiguration>();
            DieElementParametrValues = new HashSet<DieElementParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return DieElementParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = DieElementParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new DieElementParametrValue() { IdElementNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<DieElementInСonfiguration> DieElementInСonfigurations { get; set; }
        public virtual ICollection<DieElementParametrValue> DieElementParametrValues { get; set; }
    }
}
