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
    public class AddEditChildViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public IOpenFileDialogService OpenFileDialog =>
           Ioc.Instance.GetService<IOpenFileDialogService>();
        public AddEditChildViewModel()
        {
            Title = "Добавить ребенка";

            var childRelatives = new List<ChildRelative>
            {
                new ChildRelative
                {
                    RelativeRoleId = RelativeRoles.Mother,
                    IsNotSealed = false
                },
                new ChildRelative
                {
                    RelativeRoleId = RelativeRoles.Father,
                    IsNotSealed = false
                },
            };
            foreach (var relative in childRelatives)
            {
                relative.PropertyChanged += (_, __) =>
                {
                    RaisePropertyChanged(nameof(IsCanSaveChanges));
                };
            }

            Child = new Child
            {
                ChildRelatives = childRelatives
            };
            Child.PropertyChanged += (_, __) =>
                {
                    RaisePropertyChanged(
                        nameof(IsCanSaveChanges));
                };
            LoadGendersAsync()
                      .ContinueWith(t =>
                      {
                          LoadGroupsAsync();
                      })
                      .ContinueWith(t =>
                      {
                          LoadRolesAsync();
                      });
        }

        private async void LoadRolesAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<RelativeRole> currentRoles = await context.RelativeRoles.ToListAsync();
                Roles = new ObservableCollection<RelativeRole>(currentRoles);
            }
        }

        public AddEditChildViewModel(Child inputChild)
        {
            Title = "Редактировать ребенка";
            Child = inputChild;
            Child.PropertyChanged += (_, __) =>
            {
                RaisePropertyChanged(
                    nameof(IsCanSaveChanges));
            };
            foreach (var relative in inputChild.ChildRelatives)
            {
                relative.PropertyChanged += (_, __) =>
                {
                    relative.IsNotSealed =
                        relative.RelativeRoleId != RelativeRoles.Mother
                        && relative.RelativeRoleId != RelativeRoles.Father;
                    RaisePropertyChanged(nameof(IsCanSaveChanges));
                };
            }

            LoadGendersAsync()
                .ContinueWith(t =>
                {
                    LoadGroupsAsync();
                });
        }

        private async Task LoadGendersAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Gender> currentGenders = await context.Genders.ToListAsync();
                Genders = new ObservableCollection<Gender>(currentGenders);
                if (!Child.IsNew())
                {
                    Child.Gender = Genders.First(g => g.Id == Child.GenderId);
                }
            }
        }

        private async void LoadGroupsAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Group> currentGroups = await context.Groups.ToListAsync();
                Groups = new ObservableCollection<Group>(currentGroups);
                if (Child.IsNew())
                {
                    SelectedGroup = Groups.FirstOrDefault();
                }
                else
                {
                    Child.Group = Groups.First(g => g.Id == Child.GroupId);
                }
            }
        }

        public Child Child { get; set; }

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

        public bool IsCanSaveChanges => string.IsNullOrWhiteSpace(Child.Error);

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
        public ObservableCollection<Gender> Genders { get; set; }
        public Gender SelectedGender { get; set; }
        public Group SelectedGroup { get; set; }

        private void AddImage()
        {
            if (OpenFileDialog.TryOpen(out byte[] file))
            {
                Child.Image = file;
            }
        }

        private RelayCommand openDocumentsCommand;

        public ICommand OpenDocumentsCommand
        {
            get
            {
                if (openDocumentsCommand == null)
                {
                    openDocumentsCommand = new RelayCommand(OpenDocuments);
                }

                return openDocumentsCommand;
            }
        }

        private void OpenDocuments()
        {
            if (Child.ChildDocuments == null)
            {
                Child.ChildDocuments = new List<ChildDocument>();
            }
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    DocumentsService.Open(Child.ChildDocuments);
                    if (DocumentsService.IsShouldSynchronize())
                    {
                        var synchronizedDocuments = DocumentsService.GetSynchronizedDocuments();
                        Child.ChildDocuments.Clear();
                        foreach (var document in synchronizedDocuments)
                        {
                            document.ChildId = Child.Id;
                            Child.ChildDocuments.Add(document);
                        }
                    }
                    DocumentsService.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        private RelayCommand confirmAddChildCommand;

        public ICommand ConfirmAddChildCommand
        {
            get
            {
                if (confirmAddChildCommand == null)
                {
                    confirmAddChildCommand = new RelayCommand(ConfirmAddChildAsync);
                }

                return confirmAddChildCommand;
            }
        }

        private async void ConfirmAddChildAsync()
        {
            try
            {
                foreach (var relative in Child.ChildRelatives)
                {
                    if (relative.RelativeRole != null)
                    {
                        relative.RelativeRoleId = relative.RelativeRole.Id;
                        relative.RelativeRole = null;
                    }
                }

                if (!Child.IsNew())
                {
                    using (var context = ContextFactory.GetInstance())
                    {
                        var relatives = context.ChildRelatives.Where(r => r.ChildId == Child.Id);
                        context.ChildRelatives.RemoveRange(relatives);
                        await context.SaveChangesAsync();

                        var childFromDb = await context.Children.FirstAsync(c => c.Id == Child.Id);
                        foreach (var relative in Child.ChildRelatives)
                        {
                            childFromDb.ChildRelatives.Add(new ChildRelative
                            {
                                ChildId = childFromDb.Id,
                                RelativeRoleId = relative.RelativeRoleId,
                                FullName = relative.FullName,
                                PhoneNumber = relative.PhoneNumber,
                                Year = relative.Year
                            });
                        }
                        await context.SaveChangesAsync();
                    }

                    using (var context = ContextFactory.GetInstance())
                    {
                        var documents = context.ChildDocuments.Where(d => d.ChildId == Child.Id);
                        context.ChildDocuments.RemoveRange(documents);
                        await context.SaveChangesAsync();

                        var childFromDb = await context.Children.FirstAsync(c => c.Id == Child.Id);
                        foreach (var document in Child.ChildDocuments)
                        {
                            childFromDb.ChildDocuments.Add(new ChildDocument
                            {
                                ChildId = childFromDb.Id,
                                FileName = document.FileName,
                                FileBytes = document.FileBytes,
                            });
                        }
                        await context.SaveChangesAsync();
                    }
                }

                using (var context = ContextFactory.GetInstance())
                {
                    if (Child.Group != null)
                    {
                        Child.GroupId = Child.Group.Id;
                        Child.Group = null;
                    }
                    if (Child.Gender != null)
                    {
                        Child.GenderId = Child.Gender.Id;
                        Child.Gender = null;
                    }
                    if (Child.IsNew())
                    {
                        context.Children.Add(Child);
                    }
                    else
                    {
                        Child.ChildDocuments.Clear();
                        context.Entry(Child).State = EntityState.Modified;
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
        public string QuestionText => Title;

        private RelayCommand addRelativeCommand;

        public ICommand AddRelativeCommand
        {
            get
            {
                if (addRelativeCommand == null)
                    addRelativeCommand = new RelayCommand(AddRelative);

                return addRelativeCommand;
            }
        }

        public ObservableCollection<RelativeRole> Roles { get; set; }

        private void AddRelative()
        {
            ChildRelative relative = new ChildRelative
            {
                RelativeRoleId = RelativeRoles.Brother,
                ChildId = Child.Id,
                IsNotSealed = true
            };
            relative.PropertyChanged += (_, __) =>
            {
                RaisePropertyChanged(nameof(IsCanSaveChanges));
            };
            Child.ChildRelatives.Add(relative);
            Child.RelativeList = new ObservableCollection<ChildRelative>(Child.ChildRelatives);
        }
    }
}