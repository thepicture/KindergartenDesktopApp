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
    public class ChildrenManagementViewModel : KindergartenViewModelBase
    {
        public ObservableCollection<Gender> Genders { get; set; }
        public Gender SelectedGender { get; set; }
        public ObservableCollection<Group> Groups { get; set; }
        public Group SelectedGroup { get; set; }

        public ChildrenManagementViewModel()
        {
            Title = "Управление детьми";
            LoadChildrenAsync()
                .ContinueWith(t =>
                {
                    return LoadGroupsAsync();
                })
                .ContinueWith(t =>
                {
                    return LoadUpbringersAsync();
                })
                .ContinueWith(t =>
                {
                    return LoadGendersAsync();
                });
        }

        private async Task LoadUpbringersAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<User> currentUpbringers = await context.Users.ToListAsync();
                currentUpbringers.Insert(0, new User { FullName = "Любой" });
                Upbringers = new ObservableCollection<User>(currentUpbringers);
                SelectedUpbringer = Upbringers.First();
            }
        }

        public void OnAppearing()
        {
            _ = LoadChildrenAsync();
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

        private const int MaxAgeDifference = 0;

        private async Task LoadChildrenAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                IEnumerable<Child> currentChildren = await context.Children
                    .Where(c => !c.IsDeleted)
                    .Include(u => u.Group)
                    .Include(u => u.Group.Users)
                    .Include(u => u.ChildRelatives)
                    .Include(u =>
                        u.ChildRelatives.Select(r => r.RelativeRole))
                    .Include(u => u.ChildDocuments)
                    .Include(u => u.Gender)
                    .ToListAsync();
                if (!string.IsNullOrWhiteSpace(ChildrenSearchText))
                {
                    currentChildren = currentChildren.Where(c =>
                    {
                        return c.FullName.IndexOf(ChildrenSearchText,
                                                  StringComparison.OrdinalIgnoreCase) != -1;
                    });
                }
                if (SelectedGender is Gender gender && gender.Id > 0)
                {
                    currentChildren = currentChildren.Where(c => c.GenderId == gender.Id);
                }
                if (SelectedGroup is Group group && group.Id > 0)
                {
                    currentChildren = currentChildren.Where(c => c.GroupId == group.Id);
                }
                if (SelectedUpbringer is User upbringer && upbringer.Id > 0)
                {
                    currentChildren = currentChildren.Where(c => c.Group.Users.First().Id == upbringer.Id);
                }
                if (IsHasDisability)
                {
                    currentChildren = currentChildren.Where(e => e.IsDisabled == IsHasDisability);
                }
                currentChildren = currentChildren.Where(e => e.IsArchived == IsOnlyArchived);
                if (int.TryParse(Year, out int parsedAge))
                {
                    currentChildren = currentChildren.Where(c =>
                    {
                        return Math.Abs(parsedAge - (DateTime.Now.Year - c.Year.Value)) < MaxAgeDifference;
                    });
                }
                Children = new ObservableCollection<Child>(currentChildren);
            }
        }

        public ObservableCollection<Child> Children { get; set; }
        public ObservableCollection<User> Upbringers { get; set; }
        public User SelectedUpbringer { get; set; }

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
            Navigator.Go<AddEditChildViewModel>();
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

        private string childrenSearchText;

        public string ChildrenSearchText
        {
            get => childrenSearchText; set
            {
                if (Set(ref childrenSearchText, value))
                {
                    _ = LoadChildrenAsync();
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
            _ = LoadChildrenAsync();
        }

        public string Year { get; set; }

        private RelayCommand<Child> editChildCommand;

        public RelayCommand<Child> EditChildCommand
        {
            get
            {
                if (editChildCommand == null)
                {
                    editChildCommand = new RelayCommand<Child>(EditChild);
                }

                return editChildCommand;
            }
        }

        private void EditChild(Child child)
        {
            IsAskControlOpened = false;
            IsFilterOpened = false;
            Navigator.Go<AddEditChildViewModel, Child>(child);
        }

        private ICommand deleteChildCommand;

        public ICommand DeleteChildCommand
        {
            get
            {
                if (deleteChildCommand == null)
                {
                    deleteChildCommand = new RelayCommand(ShowActionsToDeleteChild);
                }

                return deleteChildCommand;
            }
        }

        private void ShowActionsToDeleteChild()
        {
            MessageBox.Warn("Для удаления профиля " +
                "ребёнка вы должны поместить его в архив " +
                "в окне обзора");
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

        public Child SelectedChild { get; set; }

        private RelayCommand<Child> reviewChildCommand;

        public RelayCommand<Child> ReviewChildCommand
        {
            get
            {
                if (reviewChildCommand == null)
                {
                    reviewChildCommand = new RelayCommand<Child>(ReviewChild);
                }

                return reviewChildCommand;
            }
        }

        private void ReviewChild(Child child)
        {
            Navigator.Go<ChildViewModel, Child>(child);
        }

        public bool IsHasDisability { get; set; }

        public bool IsOnlyArchived { get; set; }
    }
}
