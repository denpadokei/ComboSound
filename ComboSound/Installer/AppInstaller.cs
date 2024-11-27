using Zenject;

namespace ComboSound.Modules
{
    internal class AppInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<SoundManager>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
