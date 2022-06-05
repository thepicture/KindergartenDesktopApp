using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {


        public bool IsCanGoBack
        {
            get { return (bool)GetValue(IsCanGoBackProperty); }
            set { SetValue(IsCanGoBackProperty, value); }
        }

        public static readonly DependencyProperty IsCanGoBackProperty =
            DependencyProperty.Register("IsCanGoBack", typeof(bool), typeof(Header), new PropertyMetadata(default));



        public ICommand GoBackCommand
        {
            get { return (ICommand)GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }

        public static readonly DependencyProperty GoBackCommandProperty =
            DependencyProperty.Register("GoBackCommand", typeof(ICommand), typeof(Header), new PropertyMetadata(default));


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
