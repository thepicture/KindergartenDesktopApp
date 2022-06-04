namespace KindergartenDesktopApp
{
    /// <summary>
    /// Implements a static property 
    /// for <see langword="CommunityToolkit.Mvvm.DependencyInjection.Ioc"/>
    /// to prevent namespace import.
    /// </summary>
    public static class Ioc
    {
        public static CommunityToolkit.Mvvm.DependencyInjection.Ioc Instance =>
            CommunityToolkit.Mvvm.DependencyInjection.Ioc.Default;
    }
}
