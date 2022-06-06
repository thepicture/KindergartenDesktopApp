using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Linq;
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
                if (columnName == nameof(Passport))
                    if (string.IsNullOrWhiteSpace(Passport)
                        || Passport.Count(c =>
                        {
                            return char.IsDigit(c);
                        }) != 10)
                        return nameof(Passport);
                if (columnName == nameof(PhoneNumber))
                    if (string.IsNullOrWhiteSpace(PhoneNumber)
                        || PhoneNumber.Count(n => char.IsDigit(n)) != 11)
                        return nameof(PhoneNumber);
                if (columnName == nameof(Address))
                    if (string.IsNullOrWhiteSpace(Address))
                        return nameof(Address);
                if (columnName == nameof(Gender))
                    if (Gender == null)
                        return "Выберите пол";
                if (columnName == nameof(FullName) && string.IsNullOrWhiteSpace(FullName))
                    return "Введите ФИО";
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
