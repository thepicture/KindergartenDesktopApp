namespace KindergartenDesktopApp.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public bool Ask(object question)
        {
            return System.Windows.MessageBox.Show(question.ToString(),
                                           "Вопрос",
                                           System.Windows.MessageBoxButton.YesNo,
                                           System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes;
        }

        public void Warn(object warning)
        {
            System.Windows.MessageBox.Show(warning.ToString(),
                                           "Предупреждение",
                                           System.Windows.MessageBoxButton.OK,
                                           System.Windows.MessageBoxImage.Warning);
        }
    }
}
