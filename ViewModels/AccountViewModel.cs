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
    public class AccountViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {
            Me.Password = _lastPassword;
        }

        public AccountViewModel()
        {
            Title = "Личный кабинет";
            Me = Session.UserSession;
            if (!string.IsNullOrWhiteSpace(Me.Password))
            {
                _lastPassword = Me.Password;
            }
            Me.PropertyChanged += (_, __) =>
            {
                RaisePropertyChanged(
                    nameof(IsCanUpdateAccount));
            };
            LoadGroupsAsync()
             .ContinueWith(t =>
             {
                 return LoadGendersAsync();
             });
        }

        private async Task LoadGendersAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Gender> currentGenders = await context.Genders.ToListAsync();
                Genders = new ObservableCollection<Gender>(currentGenders);
                Me.Gender = Genders.FirstOrDefault(g => g.Id == Me.GenderId);
            }
        }

        private async Task LoadGroupsAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Group> currentGroups = await context.Groups.ToListAsync();
                Groups = new ObservableCollection<Group>(currentGroups);
                SelectedGroup = Me.Groups.First();
            }
        }

        public User Me { get; set; }

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

        public Group SelectedGroup { get; set; }

        private void SaveChanges()
        {
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    _lastPassword = Me.Password;
                    Me.GenderId = Me.Gender.Id;
                    Me.Groups.Clear();
                    Me.Groups.Add(SelectedGroup);
                    context.Entry(Me).State = EntityState.Modified;
                    context.SaveChangesAsync();
                    Session.Login(Me);
                    MessageBox.Warn("Профиль обновлён");
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        public bool IsCanUpdateAccount => string.IsNullOrWhiteSpace(Me.Error);
        public ObservableCollection<Gender> Genders { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        private RelayCommand changeImageCommand;
        private static string _lastPassword;

        public ICommand ChangeImageCommand
        {
            get
            {
                if (changeImageCommand == null)
                {
                    changeImageCommand = new RelayCommand(ChangeImage);
                }

                return changeImageCommand;
            }
        }

        public IOpenFileDialogService OpenFileDialog => Ioc.Instance.GetService<IOpenFileDialogService>();

        private void ChangeImage()
        {
            if (OpenFileDialog.TryOpen(out byte[] file))
            {
                Me.Image = file;
            }
        }
    }
}