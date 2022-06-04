using System.Windows;
using System.Windows.Controls;


namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(Header),
                                        new PropertyMetadata(default(string)));

        public Header()
        {
            InitializeComponent();
        }
    }
}
