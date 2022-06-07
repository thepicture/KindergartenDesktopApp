using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using KindergartenDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class AddEditUserViewModel : KindergartenViewModelBase
    {
        private const int DaysInYear = 365;

        public void OnAppearing()
        {

        }

        public IOpenFileDialogService OpenFileDialog =>
            Ioc.Instance.GetService<IOpenFileDialogService>();
        public AddEditUserViewModel()
        {
            Title = "Добавить пользователя";
            QuestionText = "Добавить сотрудника";
            User = new User
            {
                RoleId = UserRoles.EmployeeId,
                Groups = new List<Group>(),
                WorkStartDate = DateTime.Now.AddDays(-DaysInYear),
                IsEmployeeValidating = true
            };
            User.PropertyChanged += (_, __) =>
            {
                User.Groups.Clear();
                User.Groups.Add(new Group
                {
                    Title = SelectedGroup.Title,
                    UpbringerId = User.Id
                });
                User.GenderId = SelectedGender?.Id ?? 0;
                RaisePropertyChanged(
                    nameof(IsCanSaveChanges));
            };
            LoadGroupsAsync()
                .ContinueWith(t =>
                {
                    LoadGendersAsync();
                });
        }

        public AddEditUserViewModel(User inputEmployee)
        {
            Title = "Редактировать пользователя";
            QuestionText = "Редактировать сотрудника";
            User = inputEmployee;
            User.IsEmployeeValidating = true;
            User.PropertyChanged += (_, __) =>
            {
                User.Groups.Clear();
                User.Groups.Add(new Group
                {
                    Title = SelectedGroup.Title,
                    UpbringerId = User.Id
                });
                User.GenderId = SelectedGender?.Id ?? 0;
                User.Groups.Clear();
                RaisePropertyChanged(
                    nameof(IsCanSaveChanges));
            };
            LoadGroupsAsync()
                .ContinueWith(t =>
                {
                    LoadGendersAsync();
                }); ;
        }

        private async void LoadGendersAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Gender> currentGenders = await context.Genders.ToListAsync();
                Genders = new ObservableCollection<Gender>(currentGenders);
                if (User.IsNew())
                {
                    SelectedGender = Genders.First();
                }
                else
                {
                    SelectedGender = Genders.First(g =>
                    {
                        return g.Id == User.GenderId;
                    });
                }
            }
        }

        private async Task LoadGroupsAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Group> currentGroups = await context.Groups.ToListAsync();
                Groups = new ObservableCollection<Group>(currentGroups);
                if (User.IsNew())
                {
                    SelectedGroup = Groups.FirstOrDefault();
                }
                else
                {
                    SelectedGroup = Groups.First(g =>
                    {
                        return g.Id == User.Groups.ElementAt(0).Id;
                    });
                }
            }
        }

        public User User { get; set; }

        private RelayCommand saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new RelayCommand(SaveChanges);
                }

                return saveChangesCommand;
            }
        }

        public bool IsCanSaveChanges => string.IsNullOrWhiteSpace(User.Error);

        private void SaveChanges()
        {
            IsAskControlOpened = true;
        }

        private RelayCommand addImageCommand;

        public ICommand AddImageCommand
        {
            get
            {
                if (addImageCommand == null)
                {
                    addImageCommand = new RelayCommand(AddImage);
                }

                return addImageCommand;
            }
        }

        public ObservableCollection<Group> Groups { get; set; }
        public Group SelectedGroup { get; set; }

        private void AddImage()
        {
            if (OpenFileDialog.TryOpen(out byte[] file))
            {
                User.Image = file;
            }
        }

        private RelayCommand confirmAddEmployeeCommand;

        public ICommand ConfirmAddEmployeeCommand
        {
            get
            {
                if (confirmAddEmployeeCommand == null)
                {
                    confirmAddEmployeeCommand = new RelayCommand(ConfirmAddEmployeeAsync);
                }

                return confirmAddEmployeeCommand;
            }
        }

        private async void ConfirmAddEmployeeAsync()
        {
            try
            {
                using (var context = ContextFactory.GetInstance())
                {

                    User.GenderId = SelectedGender.Id;
                    if (User.IsNew())
                    {
                        context.Users.Add(User);
                    }
                    else
                    {
                        User.Groups.Clear();

                        User.Groups.Add(new Group
                        {
                            Title = SelectedGroup.Title,
                            UpbringerId = User.Id
                        });

                        User existingEmployee = context.Users.Find(User.Id);
                        context
                            .Entry(existingEmployee).CurrentValues
                            .SetValues(User);
                    }
                    IsAskControlOpened = false;
                    await context.SaveChangesAsync();
                    Navigator.Back();
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }
        public bool IsAskControlOpened { get; set; }
        public string QuestionText { get; set; }
        public ObservableCollection<Gender> Genders { get; set; }
        public Gender SelectedGender { get; set; }
    }
}