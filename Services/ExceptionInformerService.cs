namespace KindergartenDesktopApp.Services
{
    public class ExceptionInformerService : IExceptionInformerService
    {
        public void Inform(object information)
        {
            System.Windows.MessageBox.Show("Произошла ошибка выполнения программы: " + information.ToString(),
                                           "Ошибка",
                                           System.Windows.MessageBoxButton.OK,
                                           System.Windows.MessageBoxImage.Error);
        }
    }
}
