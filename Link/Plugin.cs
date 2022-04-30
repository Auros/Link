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
            zenjector.Install(Location.App, Container =>
            {
                Container.BindInterfacesTo<LinkBroadcaster>().AsSingle();
                Container.BindInterfacesTo<SongLinkManager>().AsSingle();
                Container.BindInterfacesTo<BeatmapStateManager>().AsSingle();
            });
            zenjector.Install(Location.Player, container => container.BindInterfacesTo<BeatmapCollector>().AsSingle());
        }
    }
}