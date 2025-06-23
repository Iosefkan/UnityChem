using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Polymer : IElement
    {
        public Polymer()
        {
            Films = new HashSet<Film>();
            MathModelCoefficientValues = new HashSet<MathModelCoefficientValue>();
            PolymerParametrValues = new HashSet<PolymerParametrValue>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return PolymerParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = PolymerParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new PolymerParametrValue() { IdPolymerNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<Film> Films { get; set; }
        public virtual ICollection<MathModelCoefficientValue> MathModelCoefficientValues { get; set; }
        public virtual ICollection<PolymerParametrValue> PolymerParametrValues { get; set; }
    }
}
