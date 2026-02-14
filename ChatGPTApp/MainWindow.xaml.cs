using Azure.AI.OpenAI;
using Azure;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenAI_API;
using Azure.Identity;
using OpenAI_API.Chat;
using OpenAI.Managers;
using OpenAI;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using ChatMessage = OpenAI.ObjectModels.RequestModels.ChatMessage;
using System.Collections.ObjectModel;
using ChatGPTApp.Context;
using ChatGPTApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatGPTApp.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Net.Http;
using ChatGPTApp.Infrastructure;

namespace ChatGPTApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _currentChatTitle;
        public string CurrentChatTitle
        {
            get { return _currentChatTitle; }
            set
            {
                _currentChatTitle = value;
                InitChatMessages(_currentChatTitle);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _chats;
        public ObservableCollection<string> Chats
        {
            get { return _chats; }
            set
            {
                _chats = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Message> _chatMessages;
        public ObservableCollection<Message> ChatMessages
        {
            get { return _chatMessages; }
            set
            {
                _chatMessages = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private readonly ChatService _chatService;
        private readonly APIService _apiService;

        public MainWindow()
        {
            InitializeComponent();
            AppServiceProvider.Initialize();

            _apiService = (APIService)AppServiceProvider.ServiceProvider.GetService(typeof(APIService));
            _apiService.APIKey = "sk-1-bEcYDNcBPk91SpLFIIcDT3BlbkFJBKYOajzPxvcsvQI8LWpI";
            _apiService.ExceptionMessage = x => MessageBox.Show(x, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            _chatService = (ChatService)(AppServiceProvider.ServiceProvider.GetService(typeof(ChatService)));

            InitChatsAsync();

            DataContext = this;
        }

        private async void InitChatsAsync()
        {
            try
            {
                Chats = new ObservableCollection<string>(await _chatService.GetAllChatTitlesAsync());
                if (Chats.Any())
                {
                    var currentChat = await _chatService.GetChatAsync(Chats[Chats.Count - 1]);
                    CurrentChatTitle = currentChat?.Title;
                    var messages = currentChat.Messages;
                    ChatMessages = new ObservableCollection<Message>(messages);
                }
                else
                {
                    AddNewChatAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void InitChatMessages(string chatTitle)
        {
            var chat = await _chatService.GetChatAsync(chatTitle);
            if (chat != null)
            {
                ChatMessages = new ObservableCollection<Message>(chat.Messages);
            }
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentChat = await _chatService.GetChatAsync(CurrentChatTitle);
                await _chatService.RemoveChatAsync(currentChat.Title);
                await _chatService.AddChatAsync(new Chat()
                {
                    Title = currentChat.Title,
                    Messages = new List<Message>()
                });
                ChatMessages.Clear();
            }
            catch (Exception ex)
            {
                ChatMessages.Clear();
                MessageBox.Show(ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var newUserMessage = new Message()
            {
                Author = MessageAuthor.User,
                Text = MessageTextBox.Text
            };

            MessageTextBox.Text = null;

            ChatMessages.Add(newUserMessage);

            var response = await _apiService.GetResponseFromChatGPTAsync(ChatMessages.TakeLast(10).ToList());

            var newGPTMessage = new Message()
            {
                Author = MessageAuthor.GPT,
                Text = response
            };

            ChatMessages.Add(newGPTMessage);

            var currentChat = await _chatService.GetChatAsync(CurrentChatTitle);
            var currentChatMessage = currentChat.Messages.ToList();
            currentChatMessage.Add(newUserMessage);
            currentChatMessage.Add(newGPTMessage);
            currentChat.Messages = currentChatMessage;
            await _chatService.UpdateChatAsync(currentChat);

            if (ChatMessages.Count <= 2)
            {
                var titleQuery = ChatMessages.ToList();
                titleQuery.Add(new Message() { Author = MessageAuthor.Sys, Text = "Please, come up with a title for this dialog." });
                var newChatTitle = $"{await _apiService.GetResponseFromChatGPTAsync(titleQuery)} ({DateTime.Now})";

                if (newChatTitle != null)
                {
                    var chat = await _chatService.GetChatAsync(Chats[0]);
                    chat.Title = newChatTitle;
                    await _chatService.UpdateChatAsync(chat);

                    Chats = new ObservableCollection<string>(await _chatService.GetAllChatTitlesAsync());
                    CurrentChatTitle = newChatTitle;
                }
            }
        }

        private async void AddChatButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewChatAsync();
        }

        private async void DeleteChatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _chatService.RemoveChatAsync((sender as Button)?.Tag?.ToString());
                Chats = new ObservableCollection<string>(await _chatService.GetAllChatTitlesAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void AddNewChatAsync()
        {
            var newChat = new Chat()
            {
                Title = $"New chat {DateTime.Now}",
                Messages = new List<Message>()
            };

            await _chatService.AddChatAsync(newChat);
            Chats = new ObservableCollection<string>(await _chatService.GetAllChatTitlesAsync());
            ChatMessages = new ObservableCollection<Message>();
            CurrentChatTitle = newChat.Title;
        }
    }
}