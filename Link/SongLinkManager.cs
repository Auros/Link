using SongDetailsCache;
using SongDetailsCache.Structs;
using System.Threading.Tasks;

namespace Link
{
    internal interface ISongLinkManager
    {
        Task<string?> GetSongLink(IDifficultyBeatmap difficultyBeatmap);
    }

    internal class SongLinkManager : ISongLinkManager
    {
        private SongDetails? _songDetails;
        private const string _customLevelPrefix = "custom_level_";

        public async Task<string?> GetSongLink(IDifficultyBeatmap difficultyBeatmap)
        {
            if (_songDetails is null)
                _songDetails = await SongDetails.Init();

            if (!difficultyBeatmap.level.levelID.StartsWith(_customLevelPrefix))
                return null;

            string hashToLookFor = difficultyBeatmap.level.levelID.Replace(_customLevelPrefix, string.Empty);
            return _songDetails.songs.FindByHash(hashToLookFor, out Song song) ? $"https://beatsaver.com/maps/{song.key}" : null;
        }
    }
}