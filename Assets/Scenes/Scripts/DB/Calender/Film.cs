using System.Collections.Generic;

namespace CalenderDatabase
{
    public class Film
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double CrossMin { get; set; }
        public double CrossMax { get; set; }
        public double CrossDelta { get; set; }

        public double CurveMin { get; set; }
        public double CurveMax { get; set; }
        public double CurveDelta { get; set; }
        public ICollection<FilmProfileCluster> FilmProfileClusters { get; set; }
    }
}

