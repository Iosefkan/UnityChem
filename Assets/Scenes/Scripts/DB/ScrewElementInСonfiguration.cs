using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Database
{
    public partial class ScrewElementInСonfiguration : IConfigElement
    {
        public long Id { get; set; }
        public long IdConfiguration { get; set; }
        public long IdElement { get; set; }
        public long Number { get; set; }

        [NotMapped]
        public string Name { get { return IdElementNavigation.Name; } set { IdElementNavigation.Name = value; } }

        public virtual ScrewСonfiguration IdConfigurationNavigation { get; set; }
        public virtual ScrewElement IdElementNavigation { get; set; }
    }
}
