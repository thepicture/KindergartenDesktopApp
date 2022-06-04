namespace KindergartenDesktopApp.Models.Entities
{
    public partial class User
    {
        public bool IsAdmin => RoleId == 1;
        public bool IsEmployee => RoleId == 2;
    }
}
