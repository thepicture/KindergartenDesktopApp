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
                .ContinueWith(t =>
                {
                    return LoadGroupsAsync();
                })
                .ContinueWith(t =>
                {
                    return LoadGendersAsync();
                });
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
            IsBusy = true;
            using (var context = ContextFactory.GetInstance())
            {
                IEnumerable<User> currentEmployees = await context.Users
                    .Include(u => u.Groups)
                    .Include(u => u.Gender)
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
                    currentEmployees = currentEmployees.Where(e =>
                    {
                        return e.Groups.ElementAt(0).Id == group.Id;
                    });
                }
                if (int.TryParse(Age, out int parsedAge))
                {
                    currentEmployees = currentEmployees.Where(e =>
                    {
                        return Math.Abs(e.Age - parsedAge) < MaxAgeDifference;
                    });
                }
                Employees = new ObservableCollection<User>(currentEmployees);
            }
            IsBusy = false;
        }

        public ObservableCollection<User> Employees { get; set; }

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
            IsAskControlOpened = false;
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
            IsAskControlOpened = false;
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
            IsAskControlOpened = false;
            IsFilterOpened = false;
            Navigator.Go<AddEditUserViewModel, User>(employee);
        }

        private ICommand deleteEmployeeCommand;

        public ICommand DeleteEmployeeCommand
        {
            get
            {
                if (deleteEmployeeCommand == null)
                {
                    deleteEmployeeCommand = new RelayCommand(DeleteEmployeeAsync);
                }

                return deleteEmployeeCommand;
            }
        }

        private async void DeleteEmployeeAsync()
        {
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    var userToDelete = await context.Users.FindAsync(SelectedEmployee.Id);
                    context.Users.Remove(userToDelete);
                    await context.SaveChangesAsync();
                    IsAskControlOpened = false;
                    await LoadEmployeesAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        public bool IsAskControlOpened { get; set; }

        private RelayCommand openDeletePromptCommand;

        public ICommand OpenDeletePromptCommand
        {
            get
            {
                if (openDeletePromptCommand == null)
                {
                    openDeletePromptCommand = new RelayCommand(OpenDeletePrompt);
                }

                return openDeletePromptCommand;
            }
        }

        private void OpenDeletePrompt()
        {
            IsFilterOpened = false;
            IsAskControlOpened = true;
        }

        public bool IsCanDelete { get; set; }

        public User SelectedEmployee { get; set; }

        private RelayCommand<User> reviewEmployeeCommand;

        public RelayCommand<User> ReviewEmployeeCommand
        {
            get
            {
                if (reviewEmployeeCommand == null)
                {
                    reviewEmployeeCommand = new RelayCommand<User>(ReviewEmployee);
                }

                return reviewEmployeeCommand;
            }
        }

        private void ReviewEmployee(User employee)
        {
            Navigator.Go<UserViewModel, User>(employee);
        }
    }
}