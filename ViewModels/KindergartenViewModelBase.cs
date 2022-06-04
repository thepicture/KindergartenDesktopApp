using GalaSoft.MvvmLight;
using KindergartenDesktopApp.Services;

namespace KindergartenDesktopApp.ViewModels
{
    public class KindergartenViewModelBase : ViewModelBase
    {
        public IContextFactoryService Context => Ioc.Instance.GetService<IContextFactoryService>();
        public ISession Session => Ioc.Instance.GetService<ISession>();
    }
}
