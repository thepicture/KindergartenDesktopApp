using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

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
                {
                    return "Введите фамилию";
                }
                if (columnName == nameof(Group) && Group == null)
                {
                    return "Выберите группу";
                }
                if (columnName == nameof(Login) && string.IsNullOrWhiteSpace(Login))
                {
                    return "Введите логин";
                }
                if (columnName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
                {
                    return "Введите пароль";
                }
                return null;
            }
        }

        public bool IsAdmin => RoleId == 1;
        public bool IsEmployee => RoleId == 2;

        /// <summary>
        /// Do not use, use this[string columnName] instead. 
        /// Will throw not implemented exception.
        /// </summary>
        public string Error => throw new System.NotImplementedException();
    }
}
