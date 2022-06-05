using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for ChildrenList.xaml
    /// </summary>
    public partial class ChildrenList : UserControl
    {
        public RelayCommand<Child> ReviewPersonCommand
        {
            get { return (RelayCommand<Child>)GetValue(ReviewPersonCommandProperty); }
            set { SetValue(ReviewPersonCommandProperty, value); }
        }

        public static readonly DependencyProperty ReviewPersonCommandProperty =
            DependencyProperty.Register("ReviewPersonCommand", typeof(RelayCommand<Child>), typeof(ChildrenList), new PropertyMetadata(default));

        public bool IsHasItem
        {
            get { return (bool)GetValue(IsHasItemProperty); }
            set { SetValue(IsHasItemProperty, value); }
        }

        public static readonly DependencyProperty IsHasItemProperty =
            DependencyProperty.Register("IsHasItem", typeof(bool), typeof(ChildrenList), new PropertyMetadata(default));

        public RelayCommand<Child> EditChildCommand
        {
            get { return (RelayCommand<Child>)GetValue(EditChildCommandProperty); }
            set { SetValue(EditChildCommandProperty, value); }
        }

        public static readonly DependencyProperty EditChildCommandProperty =
            DependencyProperty.Register("EditChildCommand", typeof(RelayCommand<Child>), typeof(ChildrenList), new PropertyMetadata(default));

        public ObservableCollection<Child> Children
        {
            get { return (ObservableCollection<Child>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register("Children",
                                        typeof(ObservableCollection<Child>),
                                        typeof(ChildrenList),
                                        new PropertyMetadata(default));

        public Child SelectedChild
        {
            get { return (Child)GetValue(SelectedChildProperty); }
            set { SetValue(SelectedChildProperty, value); }
        }

        public static readonly DependencyProperty SelectedChildProperty =
            DependencyProperty.Register("SelectedChild",
                                        typeof(Child),
                                        typeof(ChildrenList),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnSelectedChild));

        private static void OnSelectedChild(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChildrenList childrenList = (ChildrenList)d;
            childrenList.IsHasItem = e.NewValue != null;
        }

        public ChildrenList()
        {
            InitializeComponent();
        }
    }
}
