namespace CalenderDatabase
{
    public class Scenario
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public int AveragedProfilesCount { get; set; }
        public double AveragedProfileWeight { get; set; }
        public double LastProfileWeight { get; set; }

        public int Minutes { get; set; }
        public bool IsRange { get; set; }
        public double? ThicknessMax { get; set; }

        public long FilmProfileClusterId { get; set; }
        public FilmProfileCluster FilmProfileCluster { get; set; }
        public long RollSettingsId {  get; set; }
        public RollSetting RollSettings { get; set; }
    }
}
