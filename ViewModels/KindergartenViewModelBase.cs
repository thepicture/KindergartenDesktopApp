using GalaSoft.MvvmLight;
using KindergartenDesktopApp.Services;

namespace KindergartenDesktopApp.ViewModels
{
    public abstract class KindergartenViewModelBase : ViewModelBase
    {
        public string Title { get; set; }
        public IContextFactoryService ContextFactory => Ioc.Instance.GetService<IContextFactoryService>();
        public ISessionService Session => Ioc.Instance.GetService<ISessionService>();
        public INavigationService Navigator => Ioc.Instance.GetService<INavigationService>();
        public IExceptionFeedbacker ExceptionFeedbacker => Ioc.Instance.GetService<IExceptionFeedbacker>();
    }
}
