using KindergartenDesktopApp.Models.Entities;
using KindergartenDesktopApp.Properties;
using KindergartenDesktopApp.Services;
using KindergartenDesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
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
        public readonly string DataSourcePath = "./../../DataSource.txt";

        public static string Connection { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!IsConnectionStringWorks())
            {
                Crash();
            }

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

        private bool IsConnectionStringWorks()
        {
            if (!File.Exists(DataSourcePath))
            {
                return false;
            }
            else
            {
                string connection = default;
                try
                {
                    string source = File
                        .ReadAllLines(DataSourcePath)
                        .First();

                    connection = $@"metadata=res://*/Models.Entities.BaseModel.csdl|res://*/Models.Entities.BaseModel.ssdl|res://*/Models.Entities.BaseModel.msl;
                                    provider=System.Data.SqlClient;
                                    provider connection string="";
                                    data source={source};
                                    initial catalog=KindergartenBase;
                                    integrated security=True;
                                    MultipleActiveResultSets=True;
                                    App=EntityFramework""";
                    using (KindergartenBaseEntities entities = new KindergartenBaseEntities(connection))
                    {
                        entities.Database.Connection.Open();
                    }
                    Connection = connection;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Connection string "
                                            + connection
                                            + " is not working: "
                                            + ex.ToString());
                }

                return false;
            }
        }

        private void Crash()
        {
            MessageBox.Show("Работа программы невозможна. " +
                "Проверьте источник данных по пути " + DataSourcePath);
            Shutdown();
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
                if (Settings.Default.IsAccessibleMode)
                {
                    ((UserControl)sender).FontSize = 25;
                    ((UserControl)sender).FontWeight = FontWeights.Bold;
                    Current.MainWindow.WindowState = WindowState.Maximized;
                    Resources["PrimaryBackgroundColor"] = new SolidColorBrush(Colors.White);
                    TextBlock accessibleBlock = new TextBlock
                    {
                        Text = "Включён режим для слабовидящих",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        IsHitTestVisible = false,
                        Background = Brushes.White
                    };
                    accessibleBlock.SetValue(Panel.ZIndexProperty, 128);
                    accessibleBlock.SetValue(Grid.RowSpanProperty, 128);
                    accessibleBlock.SetValue(Grid.ColumnSpanProperty, 128);
                    ((dynamic)sender).Content.Children.Add(accessibleBlock);
                }
                else
                {
                    ((UserControl)sender).FontFamily = new FontFamily("Microsoft JhengHei");
                    Resources["PrimaryBackgroundColor"] = new SolidColorBrush(new Color() { R = 218, G = 224, B = 230, A = 255 });
                }
                ((dynamic)sender).DataContext.OnAppearing();
            }
        }
    }
}
