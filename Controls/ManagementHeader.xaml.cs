using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for ManagementHeader.xaml
    /// </summary>
    public partial class ManagementHeader : UserControl
    {


        public bool IsCanDelete
        {
            get { return (bool)GetValue(IsCanDeleteProperty); }
            set { SetValue(IsCanDeleteProperty, value); }
        }

        public static readonly DependencyProperty IsCanDeleteProperty =
            DependencyProperty.Register("IsCanDelete", typeof(bool), typeof(ManagementHeader), new PropertyMetadata(default));



        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText",
                                        typeof(string),
                                        typeof(ManagementHeader),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ManagementHeader), new PropertyMetadata(default));



        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(ManagementHeader), new PropertyMetadata(default));



        public ICommand OpenFiltersCommand
        {
            get { return (ICommand)GetValue(OpenFiltersCommandProperty); }
            set { SetValue(OpenFiltersCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenFiltersCommandProperty =
            DependencyProperty.Register("OpenFiltersCommand", typeof(ICommand), typeof(ManagementHeader), new PropertyMetadata(default));

        public ManagementHeader()
        {
            InitializeComponent();
        }
    }
}
