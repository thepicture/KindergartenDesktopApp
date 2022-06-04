namespace KindergartenDesktopApp.Services
{
    public interface INavigationService
    {
        bool IsCanGoBack { get; }

        void Back();
        void ToRoot();
        void Go<T, TParam>(TParam param);
        void Go<T>();
    }
}