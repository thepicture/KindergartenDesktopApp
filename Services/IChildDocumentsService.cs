using KindergartenDesktopApp.Models.Entities;
using System.Collections.Generic;

namespace KindergartenDesktopApp.Services
{
    /// <summary>
    /// Defines a method for viewing documents about the current child.
    /// </summary>
    public interface IChildDocumentsService
    {
        string GetFolderPath();
        void CreateFolder(IEnumerable<ChildDocument> documents);
        void Open(IEnumerable<ChildDocument> documents);
        void Close();
        bool IsShouldSynchronize();
        IEnumerable<ChildDocument> GetSynchronizedDocuments();
    }
}
