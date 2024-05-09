using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Database
{
    public partial class Die : IPossibleConfig, IConfig
    {
        public Die()
        {
            DieElementInСonfigurations = new HashSet<DieElementInСonfiguration>();
            Extruders = new HashSet<Extruder>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string ValName
        {
            get
            {
                return "Configuration";
            }
            set
            {
                Debug.Log($"Попытка присвоить значение {value} к ValName");
            }
        }

        [NotMapped]
        public IElement Element { get { return null; } set { Debug.Log("в Database.Die не реализован set для Element"); } }
        [NotMapped]
        public IConfig Config { get { return this; } set { Debug.Log("в Database.Die не реализован set для Config"); } }

        [NotMapped]
        public List<IConfigElement> ConfigElements
        {
            get
            {
                return DieElementInСonfigurations.ToList<IConfigElement>();
            }
            set
            {
                var collect = DieElementInСonfigurations;
                while (collect.Count > value.Count)
                {
                    collect.Remove(collect.Last());
                }
                while (collect.Count < value.Count)
                {
                    collect.Add(new DieElementInСonfiguration() { IdDieNavigation = this });
                }

                int i = 0;
                foreach (var element in collect)
                {
                    value[i++].CopyTo(element);
                }
            }
        }

        public virtual ICollection<DieElementInСonfiguration> DieElementInСonfigurations { get; set; }
        public virtual ICollection<Extruder> Extruders { get; set; }
    }
}
