using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text;

namespace KindergartenDesktopApp.Models.Entities
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class User : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FullName) && string.IsNullOrWhiteSpace(FullName))
                    return "Введите ФИО";
                if (columnName == nameof(Age) && Age <= 18)
                    return "Введите корректный возраст от 18 лет";
                if (RoleId == UserRoles.EmployeeId && columnName == nameof(Groups) && Groups == null)
                    return "Выберите группу";
                if (columnName == nameof(Gender) && Gender == null)
                    return "Выберите пол";
                if (columnName == nameof(Login) && string.IsNullOrWhiteSpace(Login))
                    return "Введите логин";
                if (columnName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
                    return "Введите пароль";
                return null;
            }
        }

        public bool IsAdmin => RoleId == 1;
        public bool IsEmployee => RoleId == 2;

        public string Error
        {
            get
            {
                StringBuilder errorsBuilder = new StringBuilder();
                foreach (var property in GetType()
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (this[property.Name] != null)
                    {
                        errorsBuilder.AppendLine(property.Name + ": " + this[property.Name]);
                    }
                }
                return errorsBuilder.ToString();
            }
        }
    }
}
