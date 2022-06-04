using KindergartenDesktopApp.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl
    {


        public ICommand FilterCollectionCommand
        {
            get { return (ICommand)GetValue(FilterCollectionCommandProperty); }
            set { SetValue(FilterCollectionCommandProperty, value); }
        }

        public static readonly DependencyProperty FilterCollectionCommandProperty =
            DependencyProperty.Register("FilterCollectionCommand", typeof(ICommand), typeof(FilterControl), new PropertyMetadata(default));



        public string Age
        {
            get { return (string)GetValue(AgeProperty); }
            set { SetValue(AgeProperty, value); }
        }

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register("Age",
                                        typeof(string),
                                        typeof(FilterControl),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public ObservableCollection<Group> Groups
        {
            get { return (ObservableCollection<Group>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }

        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(ObservableCollection<Group>), typeof(FilterControl), new PropertyMetadata(default));



        public ObservableCollection<Gender> Genders
        {
            get { return (ObservableCollection<Gender>)GetValue(GendersProperty); }
            set { SetValue(GendersProperty, value); }
        }

        public static readonly DependencyProperty GendersProperty =
            DependencyProperty.Register("Genders", typeof(ObservableCollection<Gender>), typeof(FilterControl), new PropertyMetadata(default));



        public Group SelectedGroup
        {
            get { return (Group)GetValue(SelectedGroupProperty); }
            set { SetValue(SelectedGroupProperty, value); }
        }

        public static readonly DependencyProperty SelectedGroupProperty =
            DependencyProperty.Register("SelectedGroup",
                                        typeof(Group),
                                        typeof(FilterControl),
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
                                        typeof(FilterControl),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public FilterControl()
        {
            InitializeComponent();
        }
    }
}
