using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LegoExprEV3.Model
{
    public class EV3
    {
        public bool IsConnected { get; private set; }
        private Brick connector;
        private Collection<string> _Ports = new Collection<string>();
        public Action<int, int> moveAction;

        /// <summary>
        /// EV3コンストラクタ
        /// </summary>
        public EV3()
        {
            IsConnected = false;
            InitializeEv3();
        }

        /// <summary>
        /// EV3の初期化、ポート一覧の登録
        /// </summary>
        public void InitializeEv3()
        {
            if (SerialPort.GetPortNames().Length == 0)
            {
                throw new Exception("EV3の接続先が見つかりません。");
            }

            foreach (string portName in SerialPort.GetPortNames())
            {
                _Ports.Add(portName);
            }
        }

        /// <summary>
        /// 指定されたポートのEV3に接続
        /// </summary>
        /// <param name="selectedPort">指定されたポート</param>
        public void ConnectEv3(string selectedPort)
        {
            connector = new Brick(new BluetoothCommunication(selectedPort));
            Action connectAction = new Action(async () =>
            {
                try
                {
                    await connector.ConnectAsync();
                    IsConnected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            connectAction();
            SetMoveAction();
        }

        /// <summary>
        /// 接続されたEV3と切断
        /// </summary>
        public void DisconnectEv3()
        {
            try
            {
                if (! IsConnected) { return; }
                connector.Disconnect();
                connector = null;
                IsConnected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ポート一覧の取得
        /// </summary>
        /// <returns>ポートのリストの返却</returns>
        public Collection<string> getPorts()
        {
            return _Ports;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void SetMoveAction()
        {
            Action<int, int> _moveAction = new Action<int, int>(async (left, right) =>
            {
                await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, left);
                await connector.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, right);
            });
            this.moveAction = _moveAction;
        }
    }
}
