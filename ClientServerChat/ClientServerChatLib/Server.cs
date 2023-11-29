using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerChatLib
{
    public class Server
    {
        private TcpListener server;
        private List<ClientInfo> clients = new List<ClientInfo>();
        public event EventHandler<string> ClientConnected;
        public event EventHandler<string> GotMessage;

        private Thread listenThread;
        private const int port = 8888; // Оберіть потрібний порт

        public Server()
        {
            server = new TcpListener(IPAddress.Any, port);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

        private void ListenForClients()
        {
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }


        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                string dataReceived = Encoding.Unicode.GetString(message, 0, bytesRead);
                // Розбиваємо отримане повідомлення на тип та вміст
                string[] messageParts = dataReceived.Split('|');
                string messageType = messageParts[0]; // Перший елемент - тип повідомлення
                string messageContent = messageParts[1]; // Другий - вміст повідомлення

                if (messageType == "Login")
                {
                    // Якщо це повідомлення про логін, реєструємо клієнта на сервері
                    ClientInfo newClient = new ClientInfo
                    {
                        Name = messageContent,
                        Client = tcpClient
                    };
                    clients.Add(newClient);
                    ClientConnected.Invoke(client, newClient.Name);

                    //Надо отправителю сообщить список всех клиентов
                    SendLoggedListMessage(tcpClient);
                    //Надо всем сообщить кто добавился
                    SendNewLogInInfoToAll(tcpClient);
                    GotMessage.Invoke(tcpClient, $"Client {messageContent} logged in.");

                }

                else if (messageType == "Public")
                {
                    // Якщо це звичайне повідомлення, розсилаємо його іншим клієнтам
                    string senderName = clients.FirstOrDefault(c => c.Client == tcpClient)?.Name;
                    if (senderName != null)
                    {
                        GotMessage.Invoke(tcpClient, senderName + ":" + dataReceived);

                        SendMessageToAllClients($"{senderName}:{messageContent}", tcpClient);
                    }
                }
                else if (messageType == "Private")
                {
                    // Якщо це приватне повідомлення, визначаємо адресата та відправляємо йому повідомлення
                    messageParts = messageContent.Split(':');
                    string recipientName = messageParts[0];
                    string privateMessage = messageParts[1];

                    string senderName = clients.FirstOrDefault(c => c.Client == tcpClient)?.Name;
                    // Пошук клієнта за ім'ям
                    var recipientClient = clients.FirstOrDefault(c => c.Name == recipientName);
                    GotMessage.Invoke(tcpClient, senderName + ":" + dataReceived);

                    if (recipientClient != null)
                    {
                        byte[] privateData = Encoding.Unicode.GetBytes($"{senderName}:{privateMessage}");
                        NetworkStream recipientStream = recipientClient.Client.GetStream();
                        recipientStream.Write(privateData, 0, privateData.Length);
                        recipientStream.Flush();
                    }
                    else
                    {
                        Console.WriteLine($"Recipient {recipientName} not found.");
                    }
                }
            }

            tcpClient.Close();
        }

        private void SendMessageToAllClients(string message, TcpClient senderClient)
        {
            foreach (var client in clients)
            {
                if (client.Client != senderClient)
                {
                    SendMessageToClient(client.Client,message);
                }
            }
        }
        private void SendNewLogInInfoToAll(TcpClient sender)
        {
            var senderName = GetClientInfo(sender).Name;
            SendMessageToAllClients($"Logged:{senderName}", sender);
        }

        private void SendLoggedListMessage(TcpClient receiver)
        {
            var nameList = clients.Where(c => c.Client != receiver).Select(x => x.Name).ToList();
            SendMessageToClient(receiver, "LoggedList:"+string.Join(",",nameList));
        }


        private void SendMessageToClient(TcpClient receiverClient, string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            NetworkStream clientStream = receiverClient.GetStream();
            clientStream.Write(data, 0, data.Length);
            clientStream.Flush();
        }

        public ClientInfo GetClientInfo(TcpClient client)
        {
            return clients.FirstOrDefault(c => c.Client == client);
        }
    }
}