using GalaSoft.MvvmLight;
using KindergartenDesktopApp.Services;

namespace KindergartenDesktopApp.ViewModels
{
    public class KindergartenViewModelBase : ViewModelBase
    {
        public string Title { get; set; }
        public IContextFactoryService Context => Ioc.Instance.GetService<IContextFactoryService>();
        public ISessionService Session => Ioc.Instance.GetService<ISessionService>();
        public INavigationService Navigator => Ioc.Instance.GetService<INavigationService>();
    }
}
