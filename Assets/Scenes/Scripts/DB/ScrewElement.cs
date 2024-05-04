using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public List<IParametr> GetParametrs()
        {
            return ScrewElementParametrValues.ToList<IParametr>() ;
        }

        public virtual ICollection<ScrewElementInСonfiguration> ScrewElementInСonfigurations { get; set; }
        public virtual ICollection<ScrewElementParametrValue> ScrewElementParametrValues { get; set; }
    }
}
