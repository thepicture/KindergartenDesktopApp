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
                    RelativeRoleId = RelativeRoles.Mother
                },
                new ChildRelative
                {
                    RelativeRoleId = RelativeRoles.Father
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
                      });
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
                        foreach (var document in Child.ChildDocuments)
                        {
                            if (!synchronizedDocuments.Any(d =>
                            {
                                return d.FileName == document.FileName && Enumerable.SequenceEqual(d.FileBytes, document.FileBytes);
                            }))
                            {
                                Child.ChildDocuments.First(d =>
                                {
                                    return d.FileName == document.FileName && Enumerable.SequenceEqual(d.FileBytes, document.FileBytes);
                                }).IsDeleted = true;
                            }
                            else
                            {
                                if (synchronizedDocuments.FirstOrDefault(d => d.FileName == document.FileName) is ChildDocument foundDocument)
                                {
                                    if (!Enumerable.SequenceEqual(document.FileBytes, foundDocument.FileBytes))
                                    {
                                        document.FileBytes = foundDocument.FileBytes;
                                        document.IsModified = true;
                                    }
                                }
                            }
                        }
                        foreach (var document in synchronizedDocuments)
                        {
                            if (!Child.ChildDocuments.Any(d =>
                            {
                                return d.FileName == document.FileName && Enumerable.SequenceEqual(d.FileBytes, document.FileBytes);
                            }))
                            {
                                document.IsAdded = true;
                                Child.ChildDocuments.Add(document);
                            }
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
                        foreach (var relative in Child.ChildRelatives)
                        {
                            if (relative.Id != 0)
                            {
                                context.Entry(relative).State = EntityState.Modified;
                            }
                        }
                        foreach (var document in Child.ChildDocuments)
                        {
                            if (document.IsDeleted)
                            {
                                context.Entry(document).State = EntityState.Deleted;
                            }
                            else if (document.IsModified)
                            {
                                context.Entry(document).State = EntityState.Modified;
                            }
                            else if (document.IsAdded)
                            {
                                context.Entry(document).State = EntityState.Added;
                            }
                        }
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
    }
}