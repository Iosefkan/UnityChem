using System.Collections.Generic;

namespace CalenderDatabase
{
    public class Film
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<FilmProfileCluster> FilmProfileClusters { get; set; }
    }
}

