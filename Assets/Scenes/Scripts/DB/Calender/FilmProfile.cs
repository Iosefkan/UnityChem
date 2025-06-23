using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalenderDatabase
{
    public class FilmProfile
    {
        public long Id { get; set; }
        public long FilmProfileClusterId { get; set; }
        public List<ProfilePoint> Profile { get; set; }
        public FilmProfileCluster FilmProfileCluster { get; set; }
    }
}
