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
        public IElement Element { get { return null; } set { Debug.Log("в Database.Die не реализован set для Element"); } }
        [NotMapped]
        public IConfig Config { get { return this; } set { Debug.Log("в Database.Die не реализован set для Config"); } }

        public List<IConfigElement> GetConfigElements()
        {
            return DieElementInСonfigurations.ToList<IConfigElement>();
        }

        public virtual ICollection<DieElementInСonfiguration> DieElementInСonfigurations { get; set; }
        public virtual ICollection<Extruder> Extruders { get; set; }
    }
}
