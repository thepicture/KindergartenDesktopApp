namespace KindergartenDesktopApp.Services
{
    public interface INavigationService
    {
        bool IsCanGoBack { get; }

        void GoBack();
        void GoToRoot();
        void Navigate<T, TParam>(TParam param);
        void Navigate<T>();
    }
}