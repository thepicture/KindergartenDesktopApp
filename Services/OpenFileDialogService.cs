using Microsoft.Win32;
using System.IO;

namespace KindergartenDesktopApp.Services
{
    internal class OpenFileDialogService : IOpenFileDialogService
    {
        public bool TryOpen(out byte[] file)
        {
            file = null;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл",
            };
            bool? isFileOpened = openFileDialog.ShowDialog();
            if (isFileOpened.HasValue && isFileOpened.Value)
            {
                file = File.ReadAllBytes(openFileDialog.FileName);
            }

            return isFileOpened.HasValue && isFileOpened.Value;
        }
    }
}
