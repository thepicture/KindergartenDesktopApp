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
        public IExceptionInformerService ExceptionInformerService => Ioc.Instance.GetService<IExceptionInformerService>();
        public IChildDocumentsService DocumentsService => Ioc.Instance.GetService<IChildDocumentsService>();
        public IMessageBoxService MessageBox => Ioc.Instance.GetService<IMessageBoxService>();
    }
}
