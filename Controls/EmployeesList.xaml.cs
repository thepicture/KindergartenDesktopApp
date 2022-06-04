using KindergartenDesktopApp.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for EmployeesList.xaml
    /// </summary>
    public partial class EmployeesList : UserControl
    {
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



        public EmployeesList()
        {
            InitializeComponent();
        }
    }
}
