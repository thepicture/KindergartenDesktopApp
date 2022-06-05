using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for EmployeesList.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class EmployeesList : UserControl
    {


        public ICommand ReviewEmployeeCommand
        {
            get { return (ICommand)GetValue(ReviewEmployeeCommandProperty); }
            set { SetValue(ReviewEmployeeCommandProperty, value); }
        }

        public static readonly DependencyProperty ReviewEmployeeCommandProperty =
            DependencyProperty.Register("ReviewEmployeeCommand", typeof(ICommand), typeof(EmployeesList), new PropertyMetadata(default));


        public bool IsHasItem
        {
            get { return (bool)GetValue(IsHasItemProperty); }
            set { SetValue(IsHasItemProperty, value); }
        }

        public static readonly DependencyProperty IsHasItemProperty =
            DependencyProperty.Register("IsHasItem", typeof(bool), typeof(EmployeesList), new PropertyMetadata(default));

        public RelayCommand<User> EditEmployeeCommand
        {
            get { return (RelayCommand<User>)GetValue(EditEmployeeCommandProperty); }
            set { SetValue(EditEmployeeCommandProperty, value); }
        }

        public static readonly DependencyProperty EditEmployeeCommandProperty =
            DependencyProperty.Register("EditEmployeeCommand", typeof(RelayCommand<User>), typeof(EmployeesList), new PropertyMetadata(default));

        public ObservableCollection<User> Employees
        {
            get { return (ObservableCollection<User>)GetValue(EmployeesProperty); }
            set { SetValue(EmployeesProperty, value); }
        }

        public static readonly DependencyProperty EmployeesProperty =
            DependencyProperty.Register("Employees",
                                        typeof(ObservableCollection<User>),
                                        typeof(EmployeesList),
                                        new PropertyMetadata(default));

        public User SelectedEmployee
        {
            get { return (User)GetValue(SelectedEmployeeProperty); }
            set { SetValue(SelectedEmployeeProperty, value); }
        }

        public static readonly DependencyProperty SelectedEmployeeProperty =
            DependencyProperty.Register("SelectedEmployee",
                                        typeof(User),
                                        typeof(EmployeesList),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnSelectedEmployee));

        private static void OnSelectedEmployee(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EmployeesList employeesList = (EmployeesList)d;
            employeesList.IsHasItem = e.NewValue != null;
        }

        public EmployeesList()
        {
            InitializeComponent();
        }
    }
}
