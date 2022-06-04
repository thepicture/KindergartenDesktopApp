using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace KindergartenDesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for PersonTemplate.xaml
    /// </summary>
    public partial class PersonTemplate : UserControl
    {
        public bool IsImageExists => Image != null;
        public bool IsImageDoesNotExist => !IsImageExists;
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
