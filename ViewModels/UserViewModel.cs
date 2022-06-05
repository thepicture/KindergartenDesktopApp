using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.ViewModels
{
    public class UserViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public UserViewModel()
        {
            Title = "Информация о сотруднике";
        }

        public UserViewModel(User employee)
        {
            Title = "Информация о сотруднике";
            Employee = employee;
        }

        public User Employee { get; set; }
    }
}