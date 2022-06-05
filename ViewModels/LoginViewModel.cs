using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class LoginViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public LoginViewModel()
        {
            Title = "Окно авторизации";
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
            IsIncorrectLoginOrPassword = false;
            await Task.Delay(
                TimeSpan.FromSeconds(1));
            using (var context = ContextFactory.GetInstance())
            {
                if (await context.Users
                        .FirstOrDefaultAsync(u =>
                            u.Login == User.Login && u.Password == User.Password)
                        is User loggedInUser)
                {
                    Session.Login(loggedInUser);
                    if (loggedInUser.IsAdmin)
                    {
                        Navigator.Go<UsersManagementViewModel>();
                    }
                    else
                    {
                        Navigator.Go<ChildrenManagementViewModel>();
                    }
                }
                else
                {
                    IsIncorrectLoginOrPassword = true;
                }
            }
        }
    }
}