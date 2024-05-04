using System;
using System.Collections.Generic;
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

        public List<IParametr> GetParametrs()
        {
            return DieElementParametrValues.ToList<IParametr>();
        }

        public virtual ICollection<DieElementInСonfiguration> DieElementInСonfigurations { get; set; }
        public virtual ICollection<DieElementParametrValue> DieElementParametrValues { get; set; }
    }
}
