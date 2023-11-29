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

namespace ClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class ClientWindow : Window
    {
        private ClientServerChatLib.Client client;

        public ClientWindow()
        {
            InitializeComponent();
            client = new ClientServerChatLib.Client();
            client.UserLoggedIn += Client_UserLoggedIn;
            client.LoggedList += Client_LoggedList;
            client.GotMessage += Client_GotMessage;
            clientsListBox.Dispatcher.Invoke(() =>
            {
                clientsListBox.Items.Add("All");
            });
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            client.SendLoginMessage(username);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageContent = messageTextBox.Text;

            bool allSelected = clientsListBox.SelectedItems.Contains("All");
            bool nothingSelected = clientsListBox.SelectedItems.Count == 0 ||
                                   (clientsListBox.SelectedItems.Count == 1 && allSelected);

            if (nothingSelected)
            {
                client.SendPublicMessage(messageContent);
            }
            else
            {
                foreach (var selectedItem in clientsListBox.SelectedItems)
                {
                    string recipientName = selectedItem.ToString();
                    if (recipientName != "All")
                    {
                        // Відправити приватне повідомлення користувачу recipientName
                        client.SendPrivateMessage(recipientName, messageContent);
                    }
                }
            }
        }

        private void Client_GotMessage(object sender, string e)
        {
            logTextBox.Dispatcher.Invoke(() =>
            {
                logTextBox.Text += e + "\n";
            });
        }
        private void Client_UserLoggedIn(object sender, string e)
        {
            // Оновлення ListBox з іменами учасників при отриманні імені залогованого користувача
            clientsListBox.Dispatcher.Invoke(() =>
            {
                clientsListBox.Items.Add(e);
            });
        }
        private void Client_LoggedList(object sender, List<string> e)
        {
            foreach (var clientName in e)
            {
                if (!string.IsNullOrWhiteSpace(clientName))
                {
                    clientsListBox.Dispatcher.Invoke(() =>
                    {
                        clientsListBox.Items.Add(clientName);
                    });
                }
            }
        }

    }
}
