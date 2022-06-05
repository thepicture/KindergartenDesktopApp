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
            Child = new Child();
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
                    saveChangesCommand = new RelayCommand(SaveChangesAsync);
                }

                return saveChangesCommand;
            }
        }

        public bool IsCanSaveChanges => !string.IsNullOrWhiteSpace(Child.FullName)
                                        && Child.Group != null
                                        && Child.Gender != null;

        private async void SaveChangesAsync()
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
                        ICollection<ChildDocument> newDocuments = new Collection<ChildDocument>();
                        foreach (var document in DocumentsService.GetSynchronizedDocuments())
                        {
                            newDocuments.Add(document);
                        }
                        Child.ChildDocuments = newDocuments;
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
                        Child existingChild = context.Children
                            .Include(c => c.ChildDocuments)
                            .First(c => c.Id == Child.Id);
                        existingChild.ChildDocuments
                            .ToList()
                            .ForEach(d => context.Entry(d).State = EntityState.Deleted);
                        await context.SaveChangesAsync();
                        existingChild.ChildDocuments.Clear();
                        foreach (var document in Child.ChildDocuments)
                        {
                            existingChild.ChildDocuments.Add(new ChildDocument
                            {
                                FileName = document.FileName,
                                FileBytes = document.FileBytes,
                            });
                        }
                        context
                            .Entry(existingChild).CurrentValues
                            .SetValues(Child);
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