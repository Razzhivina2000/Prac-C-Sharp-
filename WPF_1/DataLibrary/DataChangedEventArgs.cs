using System;
namespace DataLibrary
{
    public enum ChangeInfo { ItemChanged, Add, Remove, Replace };

    public class DataChangedEventArgs
    {
        public ChangeInfo changeInfo { get; set; }
        public string message { get; set; }
        public DataChangedEventArgs(ChangeInfo changeInfo_, string message_)
        {
            changeInfo = changeInfo_;
            message = message_;
        }
        public override string ToString() => changeInfo + " " + message;
    }
}
