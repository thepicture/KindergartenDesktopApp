using KindergartenDesktopApp.Services;
using KindergartenDesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

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
                DataTemplate viewModelTemplate = new DataTemplate(viewModel)
                {
                    VisualTree = new FrameworkElementFactory(view),
                    DataType = viewModel
                };
                Resources.Add(new DataTemplateKey(viewModel), viewModelTemplate);
            }
        }
    }
}
