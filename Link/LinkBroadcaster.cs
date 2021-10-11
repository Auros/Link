using ChatCore;
using ChatCore.Interfaces;
using SiraUtil.Tools;
using System;
using Zenject;

namespace Link
{
    internal class LinkBroadcaster : IInitializable, IDisposable
    {
        private readonly SiraLog _siraLog;
        private readonly ISongLinkManager _songLinkManager;
        private readonly ChatCoreInstance _chatCoreInstance;
        private readonly IBeatmapStateManager _beatmapStateManager;
        private IChatService? _chatService;

        public LinkBroadcaster(SiraLog siraLog, ISongLinkManager songLinkManager, IBeatmapStateManager beatmapStateManager)
        {
            _siraLog = siraLog;
            _songLinkManager = songLinkManager;
            _beatmapStateManager = beatmapStateManager;
            _chatCoreInstance = ChatCoreInstance.Create();
        }

        public void Initialize()
        {
            _chatService = _chatCoreInstance.RunAllServices();
            _chatService.OnTextMessageReceived += ChatService_OnTextMessageReceived;
        }

        private async void ChatService_OnTextMessageReceived(IChatService service, IChatMessage msg)
        {
            if (msg.Message.ToLower().StartsWith("!link"))
            {
                _siraLog.Info("Detected a link command, attempting to fetch a map key.");
                if (_beatmapStateManager.ActiveBeatmap == null && _beatmapStateManager.LastBeatmap == null)
                {
                    _siraLog.Info("No beatmap has been played recently. Ignoring command.");
                    return;
                }
                try
                {
                    if (_beatmapStateManager.ActiveBeatmap == null)
                    {
                        _siraLog.Info("The player is not actively playing a map. Let's use the last beatmap they played.");
                        string? link = await _songLinkManager.GetSongLink(_beatmapStateManager.LastBeatmap!);
                        service.SendTextMessage(Format(msg, link is null ? "Could not find a link for the last played map." : $"The most recently played map was {link}"), msg.Channel);
                    }
                    else
                    {
                        _siraLog.Info("The player is actively playing a map. Trying to find it's link.");
                        string? link = await _songLinkManager.GetSongLink(_beatmapStateManager.ActiveBeatmap!);
                        service.SendTextMessage(Format(msg, link is null ? "Could not find a link for the current map." : $"The currently played map is {link}"), msg.Channel);
                    }
                }
                catch (Exception e)
                {
                    _siraLog.Error("An error occurred while trying to fetch the beatmap link.");
                    _siraLog.Error(e);
                }
            }
        }

        private string Format(IChatMessage msg, string message) => $"! {msg.Sender.DisplayName}, {message}";
        
        public void Dispose()
        {
            if (_chatService != null)
                _chatService.OnTextMessageReceived -= ChatService_OnTextMessageReceived;

            _chatCoreInstance.StopAllServices();
        }
    }
}