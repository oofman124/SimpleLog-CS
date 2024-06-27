using System;
using System.Collections.Generic;

/*
SimpleLog v0.01 indev
Made by @oofman124 on GitHub
https://github.com/oofman124/SimpleLog-CS
MIT License.
*/

namespace SimpleLog
{
    public enum MESSAGE_TYPES { NEUTRAL, WARNING, ERROR }

    public struct Message
    {
        public MESSAGE_TYPES Type { get; }
        public string Content { get; }

        public Message(MESSAGE_TYPES type, string content)
        {
            Type = type;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }

    public class MessageLog
    {
        private readonly List<Message> messages = new List<Message>();
        public int MaxMessages { get; set; } = 10;

        // Event declaration
        public event EventHandler<MessageEventArgs> MessageLogged;

        public bool AddMessage(Message message) // Inserts a message to the `messages` list. The message's `content` can not be whitespaces or `null`.
        {
            if (string.IsNullOrWhiteSpace(message.Content))
                return false;

            if (messages.Count >= MaxMessages)
                messages.RemoveAt(0);

            messages.Add(message);

            // Raise the event
            OnMessageLogged(new MessageEventArgs(message));

            return true;
        }

        protected virtual void OnMessageLogged(MessageEventArgs e)
        {
            MessageLogged?.Invoke(this, e);
        }

        public IReadOnlyList<Message> Messages => messages.AsReadOnly();
    }

    public class MessageEventArgs : EventArgs
    {
        public Message Message { get; }

        public MessageEventArgs(Message message)
        {
            Message = message;
        }
    }
}
