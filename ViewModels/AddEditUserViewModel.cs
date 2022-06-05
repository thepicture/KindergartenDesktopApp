using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using KindergartenDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class AddEditUserViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public IOpenFileDialogService OpenFileDialog =>
            Ioc.Instance.GetService<IOpenFileDialogService>();
        public AddEditUserViewModel()
        {
            Title = "Добавить пользователя";
            User = new User
            {
                RoleId = UserRoles.EmployeeId
            };
            User.PropertyChanged += (_, __) =>
            {
                RaisePropertyChanged(
                    nameof(IsCanSaveChanges));
            };
            LoadGroupsAsync();
        }

        public AddEditUserViewModel(User inputEmployee)
        {
            Title = "Редактировать пользователя";
            User = inputEmployee;
            User.PropertyChanged += (_, __) =>
            {
                RaisePropertyChanged(
                    nameof(IsCanSaveChanges));
            };
            LoadGroupsAsync();
        }

        private async void LoadGroupsAsync()
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
                    User.Group = Groups.First(g => g.Id == User.GroupId);
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
                    saveChangesCommand = new RelayCommand(SaveChangesAsync);
                }

                return saveChangesCommand;
            }
        }

        public bool IsCanSaveChanges => !string.IsNullOrWhiteSpace(User.FullName)
                                        && !string.IsNullOrWhiteSpace(User.Login)
                                        && !string.IsNullOrWhiteSpace(User.Password)
                                        && User.Group != null;

        private async void SaveChangesAsync()
        {
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    if (User.Group != null)
                    {
                        User.GroupId = User.Group.Id;
                        User.Group = null;
                    }
                    if (User.IsNew())
                    {
                        context.Users.Add(User);
                    }
                    else
                    {
                        User existingEmployee = context.Users.Find(User.Id);
                        context
                            .Entry(existingEmployee).CurrentValues
                            .SetValues(User);
                    }
                    await context.SaveChangesAsync();
                    Navigator.Back();
                }
            }
            catch (Exception ex)
            {
                ExceptionFeedbacker.Inform(ex);
            }
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
    }
}