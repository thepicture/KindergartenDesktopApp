using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KindergartenDesktopApp.Models.Entities
{
    public partial class Child : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FullName) && string.IsNullOrWhiteSpace(FullName))
                    return nameof(FullName);
                if (columnName == nameof(Gender) && Gender == null)
                    return nameof(Gender);
                if (columnName == nameof(Group) && Group == null)
                    return nameof(Group);
                if (columnName == nameof(Year))
                    if (!Year.HasValue
                        || Year.Value < DateTime.Now.Year - 10
                        || Year.Value >= DateTime.Now.Year)
                        return nameof(Year);
                if (columnName == nameof(Citizenship) && string.IsNullOrWhiteSpace(Citizenship))
                    return nameof(Citizenship);
                if (columnName == nameof(Nationality) && string.IsNullOrWhiteSpace(Nationality))
                    return nameof(Nationality);
                if (columnName == nameof(Address) && string.IsNullOrWhiteSpace(Address))
                    return nameof(Address);
                if (columnName == nameof(HealthPolicyNumber))
                    if (string.IsNullOrWhiteSpace(HealthPolicyNumber)
                        || HealthPolicyNumber.Count(n => char.IsDigit(n)) != 16)
                        return nameof(HealthPolicyNumber);
                if (columnName == nameof(FamilyStatus) && string.IsNullOrWhiteSpace(FamilyStatus))
                    return nameof(FamilyStatus);
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
                foreach (var relative in ChildRelatives)
                {
                    if (!string.IsNullOrWhiteSpace(relative.Error))
                    {
                        errorsBuilder.AppendLine(relative.FullName + ": " + relative.Error);
                    }
                }
                return errorsBuilder.ToString();
            }
        }

        private ObservableCollection<ChildRelative> relativeList;

        public ObservableCollection<ChildRelative> RelativeList
        {
            get
            {
                if (relativeList == null)
                {
                    relativeList = new ObservableCollection<ChildRelative>(ChildRelatives);
                }
                return relativeList;
            }
            set => relativeList = value;
        }
    }
}
