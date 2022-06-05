using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace KindergartenDesktopApp.Models.Entities
{
    public partial class Child : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FullName) && string.IsNullOrWhiteSpace(FullName))
                {
                    return "Введите ФИО";
                }
                if (columnName == nameof(Group) && Group == null)
                {
                    return "Выберите группу";
                }
                if (columnName == nameof(Gender) && Gender == null)
                {
                    return "Выберите пол";
                }
                return null;
            }
        }

        /// <summary>
        /// Do not use, use this[string columnName] instead. 
        /// Will throw not implemented exception.
        /// </summary>
        public string Error => throw new System.NotImplementedException();
    }
}
