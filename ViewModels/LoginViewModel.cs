using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System.Data.Entity;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class LoginViewModel : KindergartenViewModelBase
    {
        public LoginViewModel()
        {
            User = new User();
        }

        public User User { get; set; }

        public bool IsIncorrectLoginOrPassword { get; set; }

        private RelayCommand loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new RelayCommand(LoginAsync);
                }

                return loginCommand;
            }
        }

        private async void LoginAsync()
        {
            using (var context = Context.GetInstance())
            {
                if (await context.Users
                        .FirstOrDefaultAsync(u =>
                            u.Login == User.Login && u.Password == User.Password)
                        is User loggedInUser)
                {
                    Session.Login(loggedInUser);
                    IsIncorrectLoginOrPassword = false;
                }
                else
                {
                    IsIncorrectLoginOrPassword = true;
                }
            }
        }
    }
}