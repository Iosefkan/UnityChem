using System.Collections.Generic;

namespace CalenderDatabase
{
    public class FilmProfileCluster
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double CurveStart { get; set; }
        public double CrossStart { get; set; }

        public Film Film { get; set; }
        public long FilmId { get; set; }
        public ICollection<Scenario> Scenarios { get; set; }
        public ICollection<FilmProfile> FilmProfiles { get; set; }
    }
}
