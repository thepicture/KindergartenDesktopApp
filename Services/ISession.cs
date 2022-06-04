using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.Services
{
    public interface ISessionService
    {
        User UserSession { get; }
        void Login(User user);
        void Logout();
    }
}
