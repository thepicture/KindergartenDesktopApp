using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KindergartenDesktopApp.Models.Entities
{
    public partial class ChildRelative : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FullName))
                    if (string.IsNullOrWhiteSpace(FullName))
                        return nameof(FullName);
                if (columnName == nameof(PhoneNumber))
                    if (string.IsNullOrWhiteSpace(PhoneNumber)
                        || PhoneNumber.Count(n => char.IsDigit(n)) != 11)
                        return nameof(PhoneNumber);
                if (columnName == nameof(Year))
                    if (!Year.HasValue || Year.Value >= DateTime.Now.Year)
                        return nameof(Year);
                if (columnName == nameof(RelativeRole))
                    if (RelativeRole == null && RelativeRoleId == 0)
                        return nameof(RelativeRole);
                return null;
            }
        }

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
