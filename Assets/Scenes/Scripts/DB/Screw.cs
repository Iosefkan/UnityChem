using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Screw : IElement
    {
        public Screw()
        {
            ScrewParametrValues = new HashSet<ScrewParametrValue>();
            ScrewPossibleСonfigurations = new HashSet<ScrewPossibleСonfiguration>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return ScrewParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = ScrewParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new ScrewParametrValue() { IdScrewNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<ScrewParametrValue> ScrewParametrValues { get; set; }
        public virtual ICollection<ScrewPossibleСonfiguration> ScrewPossibleСonfigurations { get; set; }
    }
}
