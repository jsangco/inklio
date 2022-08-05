using Autofac;

public class ArtDependencyModule : Module
{
    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MyService>().AsSelf();
    }
}