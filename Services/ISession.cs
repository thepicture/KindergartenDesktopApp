using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.Services
{
    public interface ISession
    {
        User UserSession { get; }
        void Login(User user);
        void Logout();
    }
}
