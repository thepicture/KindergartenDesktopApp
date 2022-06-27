using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight;
using KindergartenDesktopApp.Services;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public abstract class KindergartenViewModelBase : ViewModelBase
    {
        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        public string Title { get; set; }
        public IContextFactoryService ContextFactory => Ioc.Instance.GetService<IContextFactoryService>();
        public ISessionService Session => Ioc.Instance.GetService<ISessionService>();
        public INavigationService Navigator => Ioc.Instance.GetService<INavigationService>();
        public IExceptionInformerService ExceptionInformerService => Ioc.Instance.GetService<IExceptionInformerService>();
        public IChildDocumentsService DocumentsService => Ioc.Instance.GetService<IChildDocumentsService>();
        public IMessageBoxService MessageBox => Ioc.Instance.GetService<IMessageBoxService>();

        private RelayCommand goBackCommand;

        public ICommand GoBackCommand
        {
            get
            {
                if (goBackCommand == null)
                {
                    goBackCommand = new RelayCommand(GoBack);
                }

                return goBackCommand;
            }
        }

        private void GoBack()
        {
            Navigator.Back();
        }


        public double MenuDesiredWidth { get; set; }

        private RelayCommand toggleMenuDesiredWidthCommand;

        public ICommand ToggleMenuDesiredWidthCommand
        {
            get
            {
                if (toggleMenuDesiredWidthCommand == null)
                {
                    toggleMenuDesiredWidthCommand = new RelayCommand(ToggleMenuDesiredWidth);
                }

                return toggleMenuDesiredWidthCommand;
            }
        }

        private void ToggleMenuDesiredWidth()
        {
            MenuDesiredWidth = double.IsNaN(MenuDesiredWidth) ? 0 : double.NaN;
        }


        private RelayCommand exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(Exit);
                }

                return exitCommand;
            }
        }

        private void Exit()
        {
            Navigator.ToRoot();
        }


        private RelayCommand goToAccountCommand;

        public ICommand GoToAccountCommand
        {
            get
            {
                if (goToAccountCommand == null)
                {
                    goToAccountCommand = new RelayCommand(GoToAccount);
                }

                return goToAccountCommand;
            }
        }

        private void GoToAccount()
        {
            Navigator.Go<AccountViewModel>();
        }



        private RelayCommand goToSettingsCommand;

        public ICommand GoToSettingsCommand
        {
            get
            {
                if (goToSettingsCommand == null)
                {
                    goToSettingsCommand = new RelayCommand(GoToSettings);
                }

                return goToSettingsCommand;
            }
        }

        private void GoToSettings()
        {
            Navigator.Go<SettingsViewModel>();
        }

        private RelayCommand goToContactsCommand;
        private bool isBusy;

        public ICommand GoToContactsCommand
        {
            get
            {
                if (goToContactsCommand == null)
                    goToContactsCommand = new RelayCommand(GoToContacts);

                return goToContactsCommand;
            }
        }

        private void GoToContacts()
        {
            Navigator.Go<ContactsViewModel>();
        }
    }
}
