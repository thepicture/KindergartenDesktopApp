using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Input;

namespace KindergartenDesktopApp.ViewModels
{
    public class ChildViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public ChildViewModel()
        {
            Title = "Информация о ребёнке";
            Child = new Child();
        }

        public ChildViewModel(Child child)
        {
            Title = "Информация о ребёнке";
            Child = child;
        }

        public Child Child { get; set; }

        private RelayCommand openDocumentsCommand;

        public ICommand OpenDocumentsCommand
        {
            get
            {
                if (openDocumentsCommand == null)
                {
                    openDocumentsCommand = new RelayCommand(OpenDocuments);
                }

                return openDocumentsCommand;
            }
        }

        private void OpenDocuments()
        {
            if (Child.ChildDocuments == null)
            {
                Child.ChildDocuments = new List<ChildDocument>();
            }
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    DocumentsService.Open(Child.ChildDocuments);
                    if (DocumentsService.IsShouldSynchronize())
                    {
                        MessageBox.Warn("Вы попытались изменить внутреннюю " +
                            "структуру документов ребенка. Она не была сохранена. " +
                            @"Если вы хотите добавить документы, нажмите ""Редактировать"".");
                    }
                    DocumentsService.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        private RelayCommand loadSamplesCommand;

        public ICommand LoadSamplesCommand
        {
            get
            {
                if (loadSamplesCommand == null)
                {
                    loadSamplesCommand = new RelayCommand(LoadSamples);
                }

                return loadSamplesCommand;
            }
        }

        private void LoadSamples()
        {
            DocumentsService.CreateFolder(Child.ChildDocuments);
            string samplesZipFilePath = Path.Combine(Environment.CurrentDirectory, "ОбразцыДокументов.zip");
            if (File.Exists(samplesZipFilePath))
            {
                File.Delete(samplesZipFilePath);
            }
            ZipFile.CreateFromDirectory(DocumentsService.GetFolderPath(),
                                        samplesZipFilePath);
            DocumentsService.Close();
            Process.Start(samplesZipFilePath);
        }

        private RelayCommand moveChildToArchiveCommand;

        public ICommand MoveChildToArchiveCommand
        {
            get
            {
                if (moveChildToArchiveCommand == null)
                    moveChildToArchiveCommand = new RelayCommand(MoveChildToArchive);

                return moveChildToArchiveCommand;
            }
        }

        public string DeleteReason { get; set; }

        public bool IsChildNotArchived => !Child.IsArchived;
        public bool IsChildNotArchivedAndNotDeleted => !IsChildNotArchived && !Child.IsDeleted;

        private void MoveChildToArchive()
        {
            if (!MessageBox.Ask("Вы действительно хотите поместить профиль ребёнка в архив?"))
            {
                return;
            }
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    var childFromDb = context.Children.First(c => c.Id == Child.Id);
                    childFromDb.IsArchived = true;
                    context.SaveChanges();
                }
                Navigator.Back();
                MessageBox.Warn("Профиль ребёнка перемещён в архив. "
                    + "Для дальнейшего удаления "
                    + "зайдите в архив");
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        private RelayCommand deleteChildCommand;

        public ICommand DeleteChildCommand
        {
            get
            {
                if (deleteChildCommand == null)
                    deleteChildCommand = new RelayCommand(DeleteChild);

                return deleteChildCommand;
            }
        }

        private void DeleteChild()
        {
            if (string.IsNullOrWhiteSpace(DeleteReason))
            {
                MessageBox.Warn("Укажите причину удаления профиля ребёнка");
            }
            else
            {
                try
                {
                    using (var context = ContextFactory.GetInstance())
                    {
                        var childFromDb = context.Children.First(c => c.Id == Child.Id);
                        childFromDb.IsDeleted = true;
                        childFromDb.ArchiveReason = DeleteReason;
                        context.SaveChanges();
                    }
                    Navigator.Back();
                    MessageBox.Warn("Профиль ребёнка удалён. "
                                    + "Для восстановления обратитесь к "
                                    + "администратору базы данных");
                }
                catch (Exception ex)
                {
                    ExceptionInformerService.Inform(ex);
                }
            }
        }
    }
}