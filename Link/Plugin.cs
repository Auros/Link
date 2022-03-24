using IPA;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace Link
{
    [NoEnableDisable, Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.Install(Location.App, Container =>
            {
                Container.BindInterfacesTo<LinkBroadcaster>().AsSingle();
                Container.BindInterfacesTo<SongLinkManager>().AsSingle();
                Container.BindInterfacesTo<BeatmapStateManager>().AsSingle();
            });
            zenjector.Install(Location.Player, Container => Container.BindInterfacesTo<BeatmapCollector>().AsSingle());
        }
    }
}