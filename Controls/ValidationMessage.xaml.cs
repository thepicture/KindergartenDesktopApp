using System.Windows;
using System.Windows.Controls;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for ValidationMessage.xaml
    /// </summary>
    public partial class ValidationMessage : UserControl
    {
        public bool IsHasError
        {
            get { return (bool)GetValue(IsHasErrorProperty); }
            set { SetValue(IsHasErrorProperty, value); }
        }

        public static readonly DependencyProperty IsHasErrorProperty =
            DependencyProperty.Register("IsHasError",
                                        typeof(bool),
                                        typeof(ValidationMessage),
                                        new PropertyMetadata(default));

        public string ValidationText
        {
            get { return (string)GetValue(ValidationTextProperty); }
            set { SetValue(ValidationTextProperty, value); }
        }

        public static readonly DependencyProperty ValidationTextProperty =
            DependencyProperty.Register("ValidationText",
                                        typeof(string),
                                        typeof(ValidationMessage),
                                        new PropertyMetadata(default));

        public ValidationMessage()
        {
            InitializeComponent();
        }
    }
}
