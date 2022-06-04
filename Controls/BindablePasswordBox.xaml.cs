using System.Windows;
using System.Windows.Controls;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {


        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText",
                                        typeof(string),
                                        typeof(BindablePasswordBox),
                                        new FrameworkPropertyMetadata(default(string),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            BindableText = (sender as PasswordBox).Password;
        }
    }
}
