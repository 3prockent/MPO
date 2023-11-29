using ClientServerChatLib;
class Program
{
    static void Main(string[] args)
    {
        Client chatClient = new Client();

        Console.WriteLine("Enter your username:");
        string username = Console.ReadLine();
        chatClient.SendLoginMessage(username);

        while (true)
        {
            string message = Console.ReadLine();
            chatClient.SendMessage(message);
        }
    }
}