using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using MyXamChat.Model;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace MyXamChat.ViewModel
{
   public class ChatViewModel:BaseViewModel
    {
        protected HubConnection hubConnection;
        public ChatMesage chatmessage { get; }
        public ObservableRangeCollection<ChatMesage> Mesages { get; }
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
         Random random;
        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetProperty(ref isConnected, value);
                });
            }
        }

    
        public ChatViewModel()
        {
            chatmessage = new ChatMesage();
            Mesages = new ObservableRangeCollection<ChatMesage>();
            SendMessageCommand = new Command(async()=>await SendMessage());
            ConnectCommand = new Command(async()=>await Connect());
            DisconnectCommand = new Command(async()=>await Disconnect());
            var ip = "localhost";
            if (Device.RuntimePlatform == Device.Android)
                ip = "10.0.2.2";
            hubConnection = new HubConnectionBuilder()
                                .WithUrl($"http://192.168.1.22:5000/chatHub")
                                .Build();
             random = new Random();
            hubConnection.Closed += async (error) =>
            {
            SendLocalMessage("connected Cloose...");

                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
              {
                  var finalmessage = $"{user} says {message}";
                  SendLocalMessage(finalmessage);
              });
        }

        private void SendLocalMessage(string message)
        {
            Mesages.Add(new ChatMesage
            {
                Message = message
            });
        }
       async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendMessage",
                                               chatmessage.User,
                                               chatmessage.Message);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"connection faild :{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task Connect()
        {
            if (IsConnected)
                return;

            try
            {
                await hubConnection.StartAsync();
                isConnected = true;
                SendLocalMessage("Connecting....");
            }
            catch (Exception ex)
            {

               SendLocalMessage($"connection error :{ex.Message}");
            }
        }
        async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await hubConnection.StopAsync();
            isConnected = false;
            SendLocalMessage("Disconnected");
        }
    }
}
