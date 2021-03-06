using System;
using System.Windows;

namespace KindergartenDesktopApp.Services
{
    public class NavigationService : INavigationService
    {
        public bool IsCanGoBack => throw new NotImplementedException();

        public void Go<T>()
        {
            ((NavigationView)Application.Current.MainWindow).NavigationFrame
                .Navigate(
                    Activator.CreateInstance(
                        typeof(T)));
        }

        public void Back()
        {
            ((NavigationView)Application.Current.MainWindow).NavigationFrame.GoBack();
        }

        public void ToRoot()
        {
            while (((NavigationView)Application.Current.MainWindow).NavigationFrame.CanGoBack)
            {
                ((NavigationView)Application.Current.MainWindow).NavigationFrame.GoBack();
            }
        }

        public void Go<T, TParam>(TParam param)
        {
            ((NavigationView)Application.Current.MainWindow).NavigationFrame
                .Navigate(
                    Activator.CreateInstance(typeof(T),
                                             new object[] { param }));
        }
    }
}
