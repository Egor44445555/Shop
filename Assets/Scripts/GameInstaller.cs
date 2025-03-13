using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Container.Bind<IPlayer>().To<Player>().AsSingle();
    }
}