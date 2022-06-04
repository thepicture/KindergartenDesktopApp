using KindergartenDesktopApp.Models.Entities;

namespace KindergartenDesktopApp.Services
{
    public interface IContextFactoryService
    {
        KindergartenBaseEntities GetInstance();
    }
}
