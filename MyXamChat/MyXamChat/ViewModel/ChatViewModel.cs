using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using MyXamChat.Model;
using Xamarin.Forms;
//using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace MyXamChat.ViewModel
{
   public class ChatViewModel:BaseViewModel
    {
      //  protected HubConnection hubConnection;
      protected IHubProxy hubProxy;
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
            HubConnection con = new HubConnection("http://infosasics-001-site9.htempurl.com/signalr/hubs");
            hubProxy = con.CreateHubProxy("chat");
            con.Start();
            chatmessage = new ChatMesage();
            Mesages = new ObservableRangeCollection<ChatMesage>();
            SendMessageCommand = new Command(async()=>await SendMessage());
           // ConnectCommand = new Command(async()=>await Connect());
           // DisconnectCommand = new Command(async()=>await Disconnect());
            //var ip = "localhost";
            //if (Device.RuntimePlatform == Device.Android)
            //    ip = "10.0.2.2";
            //hubConnection = new HubConnectionBuilder()
            //                    .WithUrl($"http://infosasics-001-site9.htempurl.com/signalr/hubs")
            //                    .Build();
             random = new Random();
            //hubConnection.Closed += async (error) =>
            //{
            //SendLocalMessage("connected Cloose...");

            //    await Task.Delay(random.Next(0, 5) * 1000);
            //    await Connect();
            //};

            hubProxy.On<string, string>("newMessage", (user, message) =>
              {
                  var finalmessage = $"{user} says :{message}";
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
               // IsBusy = true;
                await hubProxy.Invoke("sendMessage", chatmessage.User,
                                                     chatmessage.Message);
                //await hubConnection.InvokeAsync("SendMessage",
                //                               chatmessage.User,
                //                               chatmessage.Message);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"connection faild :{ex.Message}");
            }
            finally
            {
              //  IsBusy = false;
            }
        }
        //async Task Connect()
        //{
        //    if (IsConnected)
        //        return;

        //    try
        //    {
        //        await hubConnection.StartAsync();
        //        isConnected = true;
        //        SendLocalMessage("Connecting....");
        //    }
        //    catch (Exception ex)
        //    {

        //       SendLocalMessage($"connection error :{ex.Message}");
        //    }
        //}
        //async Task Disconnect()
        //{
        //    if (!IsConnected)
        //        return;
        //    await hubConnection.StopAsync();
        //    isConnected = false;
        //    SendLocalMessage("Disconnected");
        //}
    }
}
