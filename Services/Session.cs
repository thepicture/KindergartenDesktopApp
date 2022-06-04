using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.Services
{
    public class Session : ISession
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
