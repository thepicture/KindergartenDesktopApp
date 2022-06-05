namespace KindergartenDesktopApp.Services
{
    public interface IOpenFileDialogService
    {
        bool TryOpen(out byte[] file);
    }
}
