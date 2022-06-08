namespace KindergartenDesktopApp.Services
{
    public interface IMessageBoxService
    {
        void Warn(object warning);
        bool Ask(object question);
    }
}
