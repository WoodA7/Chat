using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helper;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Thread(UpdateUsers).Start();
            new Thread(UpdateChat).Start();
        }

        private void UpdateUsers()
        {
            while (true)
            {
                var tcpClient = new TcpClient(@"192.168.2.94", 7777);
                var stream = tcpClient.GetStream();
                IFormatter formatter = new BinaryFormatter();
                var user = formatter.Deserialize(stream) as Dictionary<string, string>;
                lstUsers.Dispatcher.Invoke(() => lstUsers.ItemsSource = user?.Select((t) => t.Key));
                tcpClient.Close();
                Thread.Sleep(1000);
            }
        }

        private void UpdateChat()
        { }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (tbUserName.Text.Length == 0)
            {
                MessageBox.Show(@"Enter your user name!");
                return;
            }
                
            var richText = new TextRange(rtbMessage.Document.ContentStart, rtbMessage.Document.ContentEnd).Text;
            var from = tbUserName.Text;
            var msg = new Message(richText, from, "", DateTime.Now);

            try
            {
                var tcpClient = new TcpClient();
                tcpClient.Connect($"192.168.2.94", 4444);
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(tcpClient.GetStream(), msg);
                rtbMessage.Document.Blocks.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
