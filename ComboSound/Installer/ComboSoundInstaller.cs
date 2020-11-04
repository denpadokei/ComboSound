using ComboSound.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using SiraUtil;

namespace ComboSound.Installer
{
    public class ComboSoundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ComboSoundController>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
