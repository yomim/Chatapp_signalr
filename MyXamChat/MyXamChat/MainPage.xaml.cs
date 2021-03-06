using MyXamChat.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyXamChat
{
    public partial class MainPage : ContentPage
    {
        ChatViewModel _vm;
        ChatViewModel VM {
            get => _vm ?? (_vm = (ChatViewModel)BindingContext);
        }
        
        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.ConnectCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.DisconnectCommand.Execute(null);
        }
    }
}
