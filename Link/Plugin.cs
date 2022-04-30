using IPA;
using IPA.Logging;
using SiraUtil.Zenject;

namespace Link
{
    [NoEnableDisable, Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(Logger logger, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.Install(Location.App, container =>
            {
                container.BindInterfacesTo<LinkBroadcaster>().AsSingle();
                container.BindInterfacesTo<SongLinkManager>().AsSingle();
                container.BindInterfacesTo<BeatmapStateManager>().AsSingle();
            });
            zenjector.Install(Location.Player, container => container.BindInterfacesTo<BeatmapCollector>().AsSingle());
        }
    }
}