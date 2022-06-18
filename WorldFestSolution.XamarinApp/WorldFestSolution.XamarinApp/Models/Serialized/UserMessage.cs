using System;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UserMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime PostDateTime { get; set; }

        public User Receiver { get; set; }
        public User Sender { get; set; }
    }
}