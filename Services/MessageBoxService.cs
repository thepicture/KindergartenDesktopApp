namespace KindergartenDesktopApp.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void Warn(object warning)
        {
            System.Windows.MessageBox.Show(warning.ToString(),
                                           "Предупреждение",
                                           System.Windows.MessageBoxButton.OK,
                                           System.Windows.MessageBoxImage.Warning);
        }
    }
}
