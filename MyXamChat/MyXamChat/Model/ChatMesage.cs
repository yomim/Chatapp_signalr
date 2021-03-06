using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyXamChat.Model
{
  public  class ChatMesage:ObservableObject
    {
        string _user;
        public string User { 
            get=>_user; 
            set=>SetProperty(ref _user,value); }
        string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}
