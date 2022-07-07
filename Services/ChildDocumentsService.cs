using KindergartenDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;

namespace KindergartenDesktopApp.Services
{
    public class ChildDocumentsService : IChildDocumentsService
    {
        private IEnumerable<ChildDocument> _documents;
        private List<ChildDocument> _synchronizedDocuments;
        private string _folder;

        public void Close()
        {
            if (Directory.Exists(_folder))
            {
                Directory.Delete(_folder, recursive: true);
            }
            _documents = null;
            _synchronizedDocuments = null;
            _folder = null;
        }

        public void CreateFolder(IEnumerable<ChildDocument> documents)
        {
            _documents = documents;
            _synchronizedDocuments = new List<ChildDocument>();
            _folder = Path.Combine(Environment.CurrentDirectory, "Документы");
            if (Directory.Exists(_folder))
            {
                Directory.Delete(_folder, recursive: true);
            }
            Directory.CreateDirectory(_folder);
            foreach (var document in documents)
            {
                using (var documentStream = File.Create(
                    Path.Combine(_folder, document.FileName)))
                {
                    documentStream.Write(array: document.FileBytes,
                                         offset: 0,
                                         count: document.FileBytes.Length);
                }
            }
        }

        public string GetFolderPath()
        {
            return _folder;
        }

        public IEnumerable<ChildDocument> GetSynchronizedDocuments()
        {
            return _synchronizedDocuments;
        }

        public bool IsShouldSynchronize()
        {
            if (_documents.Count() != _synchronizedDocuments.Count)
            {
                return true;
            }
            else
            {
                foreach (var oldDocument in _documents)
                {
                    var searchedDocument = _synchronizedDocuments.FirstOrDefault(d =>
                    {
                        return Enumerable.SequenceEqual(oldDocument.FileBytes, d.FileBytes) && d.FileName == oldDocument.FileName;
                    });
                    if (searchedDocument == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Open(IEnumerable<ChildDocument> documents)
        {
            CreateFolder(documents);
            OpenFileDialog openDocumentDialog = new OpenFileDialog
            {
                Title = "Просмотр документов",
                InitialDirectory = _folder
            };
            openDocumentDialog.ShowDialog();

            string[] directories = Directory.GetDirectories(_folder);
            if (directories.Length > 0)
            {
                Ioc.Instance
                    .GetService<IMessageBoxService>()
                    .Warn("Вы попытались создать папку. "
                          + "Хранение документов в созданных Вами папках не поддерживаются. "
                          + "Файлы, находящиеся в папках, перенесены в корневую папку. "
                          + "Используйте открытую папку для загрузки файлов, "
                          + "не создавая папки самостоятельно");
                foreach (string directoryPath in Directory.GetDirectories(_folder))
                {
                    MoveFilesToRootRecursive(directoryPath, _folder);
                }
            }

            foreach (var documentPath in Directory.GetFiles(_folder))
            {
                _synchronizedDocuments.Add(new ChildDocument
                {
                    FileBytes = File.ReadAllBytes(documentPath),
                    FileName = new FileInfo(documentPath).Name
                });
            }
        }

        [SecurityCritical]
        private void MoveFilesToRootRecursive(string directoryPath, string folder)
        {
            foreach (var path in Directory.GetFiles(directoryPath).Concat(Directory.GetDirectories(directoryPath)))
            {
                FileAttributes attributes = File.GetAttributes(path);
                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    MoveFilesToRootRecursive(path, folder);
                }
                else
                {
                    File.Move(path, Path.Combine(folder, new FileInfo(path).Name));
                }
            }
        }
    }
}
