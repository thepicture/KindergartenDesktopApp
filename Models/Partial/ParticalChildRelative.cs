using CommunityToolkit.Mvvm.ComponentModel;
using KindergartenDesktopApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KindergartenDesktopApp.Models.Entities
{
    public partial class ChildRelative : ObservableObject, IDataErrorInfo
    {
        public bool IsNotSealed { get; set; }

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

        public ObservableCollection<RelativeRole> RolesProvider
        {
            get
            {
                using (var context = Ioc.Instance.GetService<IContextFactoryService>().GetInstance())
                {
                    var roles = context.RelativeRoles.ToList();
                    if (RelativeRoleId > 0)
                    {
                        RelativeRole = roles.First(r => r.Id == RelativeRoleId);
                    }
                    return new ObservableCollection<RelativeRole>(roles);
                }
            }
        }
    }
}
