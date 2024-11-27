using ComboSound.Views;
using Zenject;

namespace ComboSound.Installer
{
    internal class MenuInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {

            _ = this.Container.BindInterfacesAndSelfTo<SettingView>().FromNewComponentAsViewController().AsCached().NonLazy();
        }
    }
}
