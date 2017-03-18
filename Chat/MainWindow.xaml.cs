using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var richText = new TextRange(rtbMessage.Document.ContentStart, rtbMessage.Document.ContentEnd).Text;
            var from = "Dmitri";
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
