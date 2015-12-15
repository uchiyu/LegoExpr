using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LegoExprEV3
{
    public partial class MainWindow : Window
    {
        private void OnClickConnectButton(object sender, EventArgs e)
        {
            ev3.ConnectEv3((string)this.portComboBox.SelectedItem);
            isConnectedText.Text = "接続中";
            KeyDown += OnKeyDown;
            this.connectButton.IsEnabled = false;
            this.disconnectButton.IsEnabled = true;
        }

        private void OnClickDisConnectButton(object sender, EventArgs e)
        {
            ev3.DisconnectEv3();
            isConnectedText.Text = "";
            KeyDown -= OnKeyDown;
            this.connectButton.IsEnabled = true;
            this.disconnectButton.IsEnabled = false;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Console.Write(e.Key);
            switch (e.Key)
            {
                case Key.W:
                    break;
                default:
                    break;
            }
        }
    }
}
