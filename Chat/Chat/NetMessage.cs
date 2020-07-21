using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class NetMessage
    {
        public string Author, Date, Message;

        public NetMessage(string message)
        {
            AttributeNewMessage(message);
        }

        // Parse into -> Author, Date, Message
        // Message exemple : 28.65.322.21#20/07/2020 23:14:07#bonjour je suis un coucou
        // Message exemple : s#20/07/2020 23:14:07##!stop!#
        public void AttributeNewMessage(string newMessage)
        {
            int firstSeparation = newMessage.IndexOf('#');
            int secondSeparation = newMessage.IndexOf('#', firstSeparation + 1);

            Author = newMessage.Substring(0, firstSeparation);
            Date = newMessage.Substring(firstSeparation + 1, secondSeparation - firstSeparation-1);
            Message = newMessage.Substring(secondSeparation + 1, newMessage.Length - secondSeparation - 1);
        }

        public override string ToString()
        {
            return Author + "#" + Date + "#" + Message;
        }

        public byte[] ToByteArrayUTF8()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public byte[] ToByteArrayASCII()
        {
            return Encoding.ASCII.GetBytes(ToString());
        }

        public bool MessageEquals(string message)
        {
            return Message.Equals(message);
        }
    }
}
