using KindergartenDesktopApp.Services;

namespace KindergartenDesktopApp.Models.Entities
{
    public partial class Message
    {
        public bool IsMeSent => SenderId == Ioc.Instance.GetService<ISessionService>().UserSession.Id;
    }
}
