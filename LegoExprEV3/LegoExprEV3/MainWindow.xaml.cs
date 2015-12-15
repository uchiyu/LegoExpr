using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using LegoExprEV3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
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

namespace LegoExprEV3
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private EV3 ev3 = new EV3();

        public MainWindow()
        {
            InitializeComponent();
            foreach (string portName in ev3.getPorts()) { portComboBox.Items.Add(portName); }
            portComboBox.IsEnabled = true;
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = true;
            this.connectButton.Click += OnClickConnectButton;
            this.disconnectButton.Click += OnClickDisConnectButton;
        }
    }
}
