using ComboSound.Modules;
using Zenject;

namespace ComboSound.Installer
{
    public class ComboSoundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ComboSoundController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}
