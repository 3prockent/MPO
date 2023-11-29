using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        ProcessStartInfo startServer = new ProcessStartInfo();
        startServer.FileName = "cmd.exe";
        startServer.Arguments = @"/C start E:\Univer\MPO\ClientServerChat\ServerApp\bin\Debug\net6.0\ServerApp.exe";

        Process.Start(startServer);

        Thread.Sleep(3000);

        int numberOfClients = 3;

        for (int i = 0; i < numberOfClients; i++)
        {
            ProcessStartInfo startClient = new ProcessStartInfo();
            startClient.FileName = "cmd.exe";
            startClient.Arguments = @"/C start E:\Univer\MPO\ClientServerChat\ClientApp\bin\Debug\net6.0\ClientApp.exe";

            Process.Start(startClient);
        }
    }
}