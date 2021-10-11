using IPA;
using SiraUtil;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace Link
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            zenjector.On<PCAppInit>().Pseudo(Container =>
            {
                Container.BindLoggerAsSiraLogger(logger);
                Container.BindInterfacesTo<LinkBroadcaster>().AsSingle();
                Container.BindInterfacesTo<SongLinkManager>().AsSingle();
                Container.BindInterfacesTo<BeatmapStateManager>().AsSingle();
            });
            zenjector.On<GameplayCoreInstaller>().Pseudo(Container => Container.BindInterfacesTo<BeatmapCollector>().AsSingle());
        }

        [OnEnable]
        public void OnEnable()
        {

        }

        [OnDisable]
        public void OnDisable()
        {

        }
    }
}