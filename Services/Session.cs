using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.Services
{
    public class SessionService : ISessionService
    {
        public User UserSession { get; private set; }

        public void Login(User user)
        {
            UserSession = user;
        }

        public void Logout()
        {
            UserSession = null;
        }
    }
}
