namespace ContainVirus.Models
{
    public class VirusStepLog
    {
        public int DayNumber { get; set; }
        public int ChosenRegionWalls { get; set; }
        public List<(int r, int c)> ChosenRegionCells { get; set; } = new();
        public List<(int r, int c)> SpreadCells { get; set; } = new();
        public int[][] GridSnapshot { get; set; }  
    }
}
