using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Film : IElement
    {
        public Film()
        {
            ProcessParametrValues = new HashSet<ProcessParametrValue>();
            Scenarios = new HashSet<Scenario>();
        }

        public long Id { get; set; }
        public long? IdPolymer { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public string Name { get { return Type; } set { Type = value; } }
        [NotMapped]
        public List<IParametr> Parametrs
        {
            get
            {
                return ProcessParametrValues.ToList<IParametr>();
            }
            set
            {
                var collect = ProcessParametrValues;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new ProcessParametrValue() { IdFilmNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual Polymer IdPolymerNavigation { get; set; }
        public virtual ICollection<ProcessParametrValue> ProcessParametrValues { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
    }
}
