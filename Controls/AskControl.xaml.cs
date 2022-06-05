using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for AskControl.xaml
    /// </summary>
    public partial class AskControl : UserControl
    {


        public string QuestionText
        {
            get { return (string)GetValue(QuestionTextProperty); }
            set { SetValue(QuestionTextProperty, value); }
        }

        public static readonly DependencyProperty QuestionTextProperty =
            DependencyProperty.Register("QuestionText", typeof(string), typeof(AskControl), new PropertyMetadata(default));


        public ICommand YesCommand
        {
            get { return (ICommand)GetValue(YesCommandProperty); }
            set { SetValue(YesCommandProperty, value); }
        }

        public static readonly DependencyProperty YesCommandProperty =
            DependencyProperty.Register("YesCommand", typeof(ICommand), typeof(AskControl), new PropertyMetadata(default));
        public AskControl()
        {
            InitializeComponent();
        }

        private void OnNoClicked(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
