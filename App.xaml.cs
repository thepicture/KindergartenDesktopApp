using KindergartenDesktopApp.Properties;
using KindergartenDesktopApp.Services;
using KindergartenDesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KindergartenDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureDependencies();
            ConfigureTemplates();

            Settings.Default.SettingsSaving += async (_, __) =>
            {
                Ioc.Instance.GetService<INavigationService>()
                    .Back();
                await Task.Delay(100);
                Ioc.Instance.GetService<INavigationService>()
                    .Go<SettingsViewModel>();
            };

            OpenNavigationView<LoginViewModel>();
        }

        private void ConfigureDependencies()
        {
            var interfaces = ResourceAssembly
                .GetTypes()
                .Where(t =>
                {
                    return t.Name.StartsWith("I")
                           && t.Name.EndsWith("Service");
                });

            var serviceCollection = new ServiceCollection();

            foreach (var @interface in interfaces)
            {
                var implementor = ResourceAssembly
                    .GetTypes()
                    .First(t =>
                    {
                        return t.Name == @interface.Name.Substring(1);
                    });
                serviceCollection.AddSingleton(@interface, implementor);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Ioc.Instance.ConfigureServices(serviceProvider);
        }

        private static void OpenNavigationView<TWhere>()
        {
            Window window = new NavigationView();
            window.Show();
            Ioc.Instance
                .GetService<INavigationService>()
                .Go<TWhere>();
        }

        private void ConfigureTemplates()
        {
            var views = ResourceAssembly
                            .GetTypes()
                            .Where(t =>
                            {
                                return t.Name.EndsWith("View")
                                       && t.AssemblyQualifiedName.Contains("Pages");
                            });
            foreach (var view in views)
            {
                var viewModel = ResourceAssembly
                    .GetTypes()
                    .First(t => t.Name == view.Name + "Model");
                FrameworkElementFactory viewFactory = new FrameworkElementFactory(view);
                viewFactory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));
                DataTemplate viewModelTemplate = new DataTemplate(viewModel)
                {
                    VisualTree = viewFactory,
                    DataType = viewModel
                };
                Resources.Add(new DataTemplateKey(viewModel), viewModelTemplate);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext != null)
            {
                ((UserControl)sender).FontFamily = new FontFamily("Microsoft JhengHei");
                if (Settings.Default.IsAccessibleMode)
                {
                    ((UserControl)sender).FontSize = 25;
                    ((UserControl)sender).FontWeight = FontWeights.Bold;
                }
                ((dynamic)sender).DataContext.OnAppearing();
            }
        }
    }
}
