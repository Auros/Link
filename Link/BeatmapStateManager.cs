namespace Link
{
    internal interface IBeatmapStateManager
    {
        IDifficultyBeatmap? LastBeatmap { get; set; }
        IDifficultyBeatmap? ActiveBeatmap { get; set; }
    }

    internal class BeatmapStateManager : IBeatmapStateManager
    {
        public IDifficultyBeatmap? LastBeatmap { get; set; }
        public IDifficultyBeatmap? ActiveBeatmap { get; set; }
    }
}