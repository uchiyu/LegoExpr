using System;
using System.Linq;
using System.Windows;
using System.IO.Ports;
using System.Collections;
using Lego.Ev3.Desktop;
using Lego.Ev3.Core;
using System.Threading.Tasks;

namespace SampleEV3
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// EV3の接続状態
        /// true:接続中
        /// false:未接続
        /// </summary>
        private bool isConnected = false;
        private Brick connector;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // EV3接続ポートの初期化　…　(1)
                InitializeEv3();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }


        private void InitializeEv3()
        {
            // EV3はシリアルポートを使用してBluetooth接続する
            if (SerialPort.GetPortNames().Length == 0)
            {
                throw new Exception(
                    "EV3の接続先が見つかりません。");
            }

            portComboBox.IsEnabled = true;
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = false;

            // シリアルポートの一覧を表示する
            foreach (string portName in SerialPort.GetPortNames())
            {
                portComboBox.Items.Add(portName);
            }

            portComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 接続ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender,RoutedEventArgs e)
        {
            ConnectEv3();
        }

        /// <summary>
        /// EV3に接続　…　(2)
        /// </summary>
        private void ConnectEv3()
        {

            Dispatcher.Invoke(new Action(async () =>
            {
                string portName = (string)portComboBox.SelectedItem;
                connector = new Brick(new BluetoothCommunication(portName));
 
                try
                {
                    await connector.ConnectAsync();
                    isConnected = true;
                    isConnectedText.Text = "接続中";

                    portComboBox.IsEnabled = false;
                    connectButton.IsEnabled = false;
                    disconnectButton.IsEnabled = true;

                    forwardButton.IsEnabled = true;
                    leftButton.IsEnabled = true;
                    rightButton.IsEnabled = true;
                    backButton.IsEnabled = true;
                    stopButton.IsEnabled = true;
                    closeGripperButton.IsEnabled = true;
                    openGripperButton.IsEnabled = true;
                    stopGripperButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }));


        }

        /// <summary>
        /// 切断ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disconnectButton_Click(object sender,RoutedEventArgs e)
        {
            DisconnectEv3();
        }

        /// <summary>
        /// EV3から切断　…　(3)
        /// </summary>
        private void DisconnectEv3()
        {
            try
            {
                if (isConnected)
                {
                    StopEv3();

                    connector.Disconnect();
                    connector = null;
                    isConnected = false;
                }
                isConnectedText.Text = "";

                portComboBox.IsEnabled = true;
                connectButton.IsEnabled = true;
                disconnectButton.IsEnabled = false;

                forwardButton.IsEnabled = false;
                leftButton.IsEnabled = false;
                rightButton.IsEnabled = false;
                backButton.IsEnabled = false;
                stopButton.IsEnabled = false;
                closeGripperButton.IsEnabled = false;
                openGripperButton.IsEnabled = false;
                stopGripperButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 前進ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forwardButton_Click(object sender,RoutedEventArgs e)
        {
            // EV3に前進の指示を出す　…　(4)
            SetPower(50, 50);
        }

        /// <summary>
        /// 後退ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backButton_Click(object sender,RoutedEventArgs e)
        {
            // EV3に後退の指示を出す　…　(4)
            SetPower(-40, -40);
        }

        /// <summary>
        /// 左旋回ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftButton_Click(object sender,RoutedEventArgs e)
        {
            // EV3に左旋回の指示を出す　…　(4)
            SetPower(25, 50);
        }

        /// <summary>
        /// 右旋回ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rightButton_Click(object sender,RoutedEventArgs e)
        {
            // EV3に右旋回の指示を出す　…　(4)
            SetPower(50, 25);
        }

        /// <summary>
        /// EV3にモーターの出力の指示を出す　…　(4)
        /// </summary>
        private void SetPower(int left, int right)
        {
            if (isConnected)
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    // 左モーター
                    await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, left);
                    // 右モーター
                    await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, right);
                }));
                
            }
        }

        /// <summary>
        /// 停止ボタンクリック時のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopButton_Click(object sender,RoutedEventArgs e)
        {
            if (isConnected)
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    await connector.DirectCommand.StopMotorAsync(OutputPort.C | OutputPort.B, true);
                }));
            }
        }

        /// <summary>
        /// EV3のグリッパーモーターに閉じる指示を出す　…　(5)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeGripperButton_Click(object sender,RoutedEventArgs e)
        {
            if (isConnected)
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, 50);
                }));
            }
        }

        /// <summary>
        /// EV3のグリッパーモーターに開く指示を出す　…　(5)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGripperButton_Click(object sender,RoutedEventArgs e)
        {
            if (isConnected)
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -35);
                }));
            }
        }

        /// <summary>
        /// EV3のグリッパーモーターに停止指示を出す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopGripperButton_Click(object sender,RoutedEventArgs e)
        {
            if (isConnected)
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    await connector.DirectCommand.StopMotorAsync(OutputPort.A, true);
                }));
            }
        }

        /// <summary>
        /// EV3に停止の指示を出す　…　(6)
        /// </summary>
        private void StopEv3()
        {
            if (isConnected)
            {
                // モーターの回転を止める
                Task task_stop = new Task(() =>
                {
                    connector.DirectCommand.StopMotorAsync(OutputPort.All, true);
                });
                task_stop.Start();
                task_stop.Wait();
            }
        }

        /// <summary>
        /// Windowがクローズされた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            DisconnectEv3();
        }

    }
}
