using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientServerChatLib.Server server;

        public MainWindow()
        {
            InitializeComponent();
            server = new ClientServerChatLib.Server();

            // Підписка на подію для оновлення списку підключених клієнтів
            server.ClientConnected += Server_ClientConnected;
            server.GotMessage += Server_Log;
        }

        // Метод для оновлення списку підключених клієнтів
        private void Server_ClientConnected(object sender, string clientName)
        {
            ConnectedClientsListBox.Dispatcher.Invoke(() =>
            {
                ConnectedClientsListBox.Items.Add(clientName);
            });
        }

        // Метод для видалення підключеного клієнта зі списку
        private void Server_Log(object sender, string message)
        {
            TextBoxServerBoxLog.Dispatcher.Invoke(() =>
            {
                TextBoxServerBoxLog.Text += message + "\n";
            });
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}