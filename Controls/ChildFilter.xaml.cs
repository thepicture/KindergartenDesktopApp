using KindergartenDesktopApp.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for ChildFilter.xaml
    /// </summary>
    public partial class ChildFilter
    {
        public ICommand FilterCollectionCommand
        {
            get { return (ICommand)GetValue(FilterCollectionCommandProperty); }
            set { SetValue(FilterCollectionCommandProperty, value); }
        }

        public static readonly DependencyProperty FilterCollectionCommandProperty =
            DependencyProperty.Register("FilterCollectionCommand", typeof(ICommand), typeof(ChildFilter), new PropertyMetadata(default));



        public bool IsOnlyArchived
        {
            get { return (bool)GetValue(IsOnlyArchivedProperty); }
            set { SetValue(IsOnlyArchivedProperty, value); }
        }

        public static readonly DependencyProperty IsOnlyArchivedProperty =
            DependencyProperty.Register("IsOnlyArchived",
                                        typeof(bool),
                                        typeof(ChildFilter),
                                        new FrameworkPropertyMetadata(default(bool),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public string Year
        {
            get { return (string)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year",
                                        typeof(string),
                                        typeof(ChildFilter),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool IsHasDisability
        {
            get { return (bool)GetValue(IsHasDisabilityProperty); }
            set { SetValue(IsHasDisabilityProperty, value); }
        }

        public static readonly DependencyProperty IsHasDisabilityProperty =
            DependencyProperty.Register("IsHasDisability",
                                        typeof(bool),
                                        typeof(ChildFilter),
                                        new FrameworkPropertyMetadata(default(bool),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ObservableCollection<User> Upbringers
        {
            get { return (ObservableCollection<User>)GetValue(UpbringersProperty); }
            set { SetValue(UpbringersProperty, value); }
        }

        public static readonly DependencyProperty UpbringersProperty =
            DependencyProperty.Register("Upbringers", typeof(ObservableCollection<User>), typeof(ChildFilter), new PropertyMetadata(default));



        public User SelectedUpbringer
        {
            get { return (User)GetValue(SelectedUpbringerProperty); }
            set { SetValue(SelectedUpbringerProperty, value); }
        }

        public static readonly DependencyProperty SelectedUpbringerProperty =
            DependencyProperty.Register("SelectedUpbringer", typeof(User), typeof(ChildFilter), new PropertyMetadata(default));




        public ObservableCollection<Group> Groups
        {
            get { return (ObservableCollection<Group>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }

        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(ObservableCollection<Group>), typeof(ChildFilter), new PropertyMetadata(default));



        public ObservableCollection<Gender> Genders
        {
            get { return (ObservableCollection<Gender>)GetValue(GendersProperty); }
            set { SetValue(GendersProperty, value); }
        }

        public static readonly DependencyProperty GendersProperty =
            DependencyProperty.Register("Genders", typeof(ObservableCollection<Gender>), typeof(ChildFilter), new PropertyMetadata(default));



        public Group SelectedGroup
        {
            get { return (Group)GetValue(SelectedGroupProperty); }
            set { SetValue(SelectedGroupProperty, value); }
        }

        public static readonly DependencyProperty SelectedGroupProperty =
            DependencyProperty.Register("SelectedGroup",
                                        typeof(Group),
                                        typeof(ChildFilter),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Gender SelectedGender
        {
            get { return (Gender)GetValue(SelectedGenderProperty); }
            set { SetValue(SelectedGenderProperty, value); }
        }

        public static readonly DependencyProperty SelectedGenderProperty =
            DependencyProperty.Register("SelectedGender",
                                        typeof(Gender),
                                        typeof(ChildFilter),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ChildFilter()
        {
            InitializeComponent();
        }
    }
}
