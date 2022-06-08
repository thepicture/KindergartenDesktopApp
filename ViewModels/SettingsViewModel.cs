namespace KindergartenDesktopApp.ViewModels
{
    public class SettingsViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public SettingsViewModel()
        {
            Title = "Настройки";
        }

        public bool IsAccessibleMode
        {
            get => Properties.Settings.Default.IsAccessibleMode;
            set
            {
                Properties.Settings.Default.IsAccessibleMode = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}