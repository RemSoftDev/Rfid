using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid.Helpers
{
    class HelperPortDataReceived: Page
    {
        private SerialPort mySerialPort = new SerialPort("COM3");
        private bool dataReceivedHandlerChecked = false;

        public long ParseInData { get; set; }


        public HelperPortDataReceived()
        {
            
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            string[] abb =  SerialPort.GetPortNames();


            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            try
            {
                mySerialPort.Open();
                
                while (!dataReceivedHandlerChecked)
                {

                }

                mySerialPort.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Can't connect");
            }
        }

        private void DataReceivedHandler(
                            object sender,
                            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            ParseInData = Convert.ToInt64(sp.ReadLine());
            dataReceivedHandlerChecked = true;

        }

    }
}
