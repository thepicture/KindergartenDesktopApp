using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for PersonTemplate.xaml
    /// </summary>
    public partial class PersonTemplate : UserControl
    {
        public ICommand ReviewChildCommand
        {
            get { return (ICommand)GetValue(ReviewChildCommandProperty); }
            set { SetValue(ReviewChildCommandProperty, value); }
        }

        public static readonly DependencyProperty ReviewChildCommandProperty =
            DependencyProperty.Register("ReviewChildCommand", typeof(ICommand), typeof(PersonTemplate), new PropertyMetadata(default));


        public ICommand EditPersonCommand
        {
            get { return (ICommand)GetValue(EditPersonCommandProperty); }
            set { SetValue(EditPersonCommandProperty, value); }
        }

        public static readonly DependencyProperty EditPersonCommandProperty =
            DependencyProperty.Register("EditPersonCommand", typeof(ICommand), typeof(PersonTemplate), new PropertyMetadata(default));

        public IList<byte> Image
        {
            get { return (IList<byte>)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(IList<byte>), typeof(PersonTemplate), new PropertyMetadata(default));


        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(PersonTemplate), new PropertyMetadata(default));



        public string FullName
        {
            get { return (string)GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }

        public static readonly DependencyProperty FullNameProperty =
            DependencyProperty.Register("FullName", typeof(string), typeof(PersonTemplate), new PropertyMetadata(default));


        public PersonTemplate()
        {
            InitializeComponent();
        }
    }
}
