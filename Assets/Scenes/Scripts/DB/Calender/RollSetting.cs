using System.Collections.Generic;

namespace CalenderDatabase
{
    public class RollSetting
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double FirstDistance { get; set; }
        public double SecondDistance { get; set; }
        public double Elasticity { get; set; }
        public double Width { get; set; }
        public double BarrelDiameter { get; set; }
        public double NeckDiameter { get; set; }
        public double HoleDiameter { get; set; }

        public ICollection<Scenario> Scenarios { get; set; }
    }
}