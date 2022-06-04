using KindergartenDesktopApp.Models.Entities;
using System;
using System.Windows;

namespace KindergartenDesktopApp.Services
{
    public class ContextFactoryService : IContextFactoryService
    {
        public KindergartenBaseEntities GetInstance()
        {
            try
            {
                KindergartenBaseEntities entities = new KindergartenBaseEntities();
                return entities;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удаётся подключиться к БД. " + ex);
                return default;
            }
        }
    }
}
