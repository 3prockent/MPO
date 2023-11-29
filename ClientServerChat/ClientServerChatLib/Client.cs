using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerChatLib
{
    public class Client
    {
        private TcpClient client;
        private Thread receiveThread;
        private const int port = 8888; // Порт, на який підключаємося
        private const string serverIp = "127.0.0.1"; // IP-адреса сервера
        public event EventHandler<string> UserLoggedIn;
        public event EventHandler<List<string>> LoggedList;
        public event EventHandler<string> GotMessage;
        
        public Client()
        {
            client = new TcpClient();
            client.Connect(serverIp, port);

            receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();
        }
        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] message = new byte[4096];
                    int bytesRead;

                    bytesRead = client.GetStream().Read(message, 0, 4096);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    string dataReceived = Encoding.Unicode.GetString(message, 0, bytesRead);
                    Console.WriteLine(dataReceived);

                    // Розбиваємо отримане повідомлення на Sender та вміст повідомлення
                    string[] messageParts = dataReceived.Split(':');
                    string senderName = messageParts[0]; // Перший елемент - Sender
                    string messageContent = messageParts[1]; // Другий - вміст повідомлення

                    if (senderName == "Logged")
                    {
                        UserLoggedIn?.Invoke(this, messageContent); // Відправляємо подію з ім'ям залогованого учасника
                    }
                    else if(senderName == "LoggedList")
                    {
                        List<string> loggedNames = messageContent.Split(",").ToList();
                        LoggedList?.Invoke(this, loggedNames);
                    }
                    else
                    {
                        GotMessage?.Invoke(this, dataReceived);
                    }
                }
                catch
                {
                    break;
                }
            }

            client.Close();
        }
        public void SendLoginMessage(string username)
        {
            string messageToSend = $"Login|{username}";
            SendMessage(messageToSend);
        }

        public void SendPrivateMessage(string receiver, string message)
        {
            string messageToSend = $"Private|{receiver}:{message}";
            SendMessage(messageToSend);
        }

        public void SendPublicMessage(string message)
        {
            string messageToSend = $"Public|{message}";
            SendMessage(messageToSend);
        }

        private void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);
            client.GetStream().Flush();
        }
    }
}
