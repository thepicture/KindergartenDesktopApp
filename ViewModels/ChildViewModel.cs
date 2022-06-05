using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
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
    }
}