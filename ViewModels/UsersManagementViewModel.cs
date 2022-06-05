using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class UsersManagementViewModel : KindergartenViewModelBase
    {
        public ObservableCollection<Gender> Genders { get; set; }
        public Gender SelectedGender { get; set; }
        public ObservableCollection<Group> Groups { get; set; }
        public Group SelectedGroup { get; set; }

        public UsersManagementViewModel()
        {
            Title = "Управление пользователями";
            LoadEmployeesAsync()
                .ContinueWith(t => LoadGroupsAsync())
                .ContinueWith(t => LoadGendersAsync());
        }

        public void OnAppearing()
        {
            _ = LoadEmployeesAsync();
            SelectedGender = Genders?.First();
            SelectedGroup = Groups?.First();
        }

        private async Task LoadGendersAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Gender> currentGenders = await context.Genders.ToListAsync();
                currentGenders.Insert(0, new Gender { Title = "Любой" });
                Genders = new ObservableCollection<Gender>(currentGenders);
                SelectedGender = Genders.First();
            }
        }

        private async Task LoadGroupsAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Group> currentGroups = await context.Groups.ToListAsync();
                currentGroups.Insert(0, new Group { Title = "Любая" });
                Groups = new ObservableCollection<Group>(currentGroups);
                SelectedGroup = Groups.First();
            }
        }

        private const int MaxAgeDifference = 5;

        private async Task LoadEmployeesAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                IEnumerable<User> currentEmployees = await context.Users
                    .Include(u => u.Group)
                    .Where(u => u.RoleId == UserRoles.EmployeeId)
                    .ToListAsync();
                if (!string.IsNullOrWhiteSpace(EmployeeSearchText))
                {
                    currentEmployees = currentEmployees.Where(e =>
                    {
                        return e.FullName.IndexOf(EmployeeSearchText,
                                                  StringComparison.OrdinalIgnoreCase) != -1;
                    });
                }
                if (SelectedGender is Gender gender && gender.Id > 0)
                {
                    currentEmployees = currentEmployees.Where(e => e.GenderId == gender.Id);
                }
                if (SelectedGroup is Group group && group.Id > 0)
                {
                    currentEmployees = currentEmployees.Where(e => e.GroupId == group.Id);
                }
                if (int.TryParse(Age, out int parsedAge))
                {
                    currentEmployees = currentEmployees.Where(e =>
                    {
                        if (e.Age.HasValue)
                        {
                            return Math.Abs(e.Age.Value - parsedAge) < MaxAgeDifference;
                        }
                        else
                        {
                            return false;
                        }
                    });
                }
                Employees = new ObservableCollection<User>(currentEmployees);
            }
        }

        public ObservableCollection<User> Employees { get; set; }

        private RelayCommand deleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(Delete);
                }

                return deleteCommand;
            }
        }

        private void Delete()
        {
        }

        private RelayCommand addCommand;

        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(Add);
                }

                return addCommand;
            }
        }

        private void Add()
        {
            IsFilterOpened = false;
            Navigator.Go<AddEditUserViewModel>();
        }

        private RelayCommand openFiltersCommand;

        public ICommand OpenFiltersCommand
        {
            get
            {
                if (openFiltersCommand == null)
                {
                    openFiltersCommand = new RelayCommand(OpenFilters);
                }

                return openFiltersCommand;
            }
        }

        private void OpenFilters()
        {
            IsFilterOpened = !IsFilterOpened;
        }

        private string employeeSearchText;

        public string EmployeeSearchText
        {
            get => employeeSearchText; set
            {
                if (Set(ref employeeSearchText, value))
                {
                    _ = LoadEmployeesAsync();
                }
            }
        }

        public bool IsFilterOpened { get; set; }

        private RelayCommand filterCollectionCommand;

        public ICommand FilterCollectionCommand
        {
            get
            {
                if (filterCollectionCommand == null)
                {
                    filterCollectionCommand = new RelayCommand(FilterCollection);
                }

                return filterCollectionCommand;
            }
        }

        private void FilterCollection()
        {
            IsFilterOpened = false;
            _ = LoadEmployeesAsync();
        }

        public string Age { get; set; }

        private RelayCommand<User> editEmployeeCommand;

        public RelayCommand<User> EditEmployeeCommand
        {
            get
            {
                if (editEmployeeCommand == null)
                {
                    editEmployeeCommand = new RelayCommand<User>(EditEmployee);
                }

                return editEmployeeCommand;
            }
        }

        private void EditEmployee(User employee)
        {
            IsFilterOpened = false;
            Navigator.Go<AddEditUserViewModel, User>(employee);
        }
    }
}