using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class ScrewElement : IElement
    {
        public ScrewElement()
        {
            ScrewElementInСonfigurations = new HashSet<ScrewElementInСonfiguration>();
            ScrewElementParametrValues = new HashSet<ScrewElementParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return ScrewElementParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = ScrewElementParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new ScrewElementParametrValue() { IdElementNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<ScrewElementInСonfiguration> ScrewElementInСonfigurations { get; set; }
        public virtual ICollection<ScrewElementParametrValue> ScrewElementParametrValues { get; set; }
    }
}
