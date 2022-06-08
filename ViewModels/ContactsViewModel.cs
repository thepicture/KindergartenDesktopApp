using CommunityToolkit.Mvvm.Input;
using KindergartenDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace KindergartenDesktopApp.ViewModels
{
    public class ContactsViewModel : KindergartenViewModelBase
    {
        public void OnAppearing()
        {

        }

        public ContactsViewModel()
        {
            Title = "Контакты";
            _ = LoadContactsAsync();
        }

        private async Task LoadContactsAsync()
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<User> currentContacts = await context.Users
                    .Include(u => u.UserRole)
                    .Where(u => u.Id != Session.UserSession.Id)
                    .ToListAsync();
                Contacts = new ObservableCollection<User>(currentContacts);
                if (Contacts.Count > 0)
                {
                    CurrentReceiver = Contacts.First();
                }
            }
        }

        private async Task LoadChatForAsync(User user)
        {
            using (var context = ContextFactory.GetInstance())
            {
                List<Message> currentMessages = await context.Messages
                    .Where(m => m.ReceiverId == user.Id && m.SenderId == Session.UserSession.Id || m.ReceiverId == Session.UserSession.Id && m.SenderId == user.Id)
                    .ToListAsync();
                CurrentChat = new ObservableCollection<Message>(currentMessages);
            }
        }

        public ObservableCollection<User> Contacts { get; set; }
        public ObservableCollection<Message> CurrentChat { get; set; }

        private string currentMessage;

        public string CurrentMessage
        {
            get => currentMessage;
            set
            {
                if (Set(ref currentMessage, value))
                {
                    SendMessageCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private RelayCommand sendMessageCommand;

        public RelayCommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null)
                    sendMessageCommand = new RelayCommand(SendMessageAsync, () =>
                    {
                        return !string.IsNullOrWhiteSpace(CurrentMessage);
                    });

                return sendMessageCommand;
            }
        }

        private async void SendMessageAsync()
        {
            try
            {
                using (var context = ContextFactory.GetInstance())
                {
                    var message = new Message
                    {
                        ReceiverId = CurrentReceiver.Id,
                        SenderId = Session.UserSession.Id,
                        Text = CurrentMessage,
                        PublicationDateTime = DateTime.Now
                    };
                    context.Messages.Add(message);
                    await context.SaveChangesAsync();
                    CurrentMessage = String.Empty;
                    await LoadChatForAsync(CurrentReceiver);
                }
            }
            catch (Exception ex)
            {
                ExceptionInformerService.Inform(ex);
            }
        }

        private User currentReceiver;

        public User CurrentReceiver
        {
            get => currentReceiver;
            set
            {
                if (Set(ref currentReceiver, value))
                {
                    _ = LoadChatForAsync(value);
                }
            }
        }

        private RelayCommand<User> changeChatCommand;

        public RelayCommand<User> ChangeChatCommand
        {
            get
            {
                if (changeChatCommand == null)
                    changeChatCommand = new RelayCommand<User>(ChangeChat);

                return changeChatCommand;
            }
        }

        private void ChangeChat(User receiver)
        {
            CurrentReceiver = receiver;
        }
    }
}