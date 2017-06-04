using BluetoothConnectivity.Services;
using BluetoothConnectivity.Services.Interfaces;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothConnectivity
{
    class Program
    {
        static void Main(string[] args)
        {
            var _service = new BluetoothSenderService();

            Console.WriteLine("Looking for the devices...");
            var discoveredDevices = _service.GetDevices().Result;

            Console.WriteLine("Retrieving CRIUS_BT...");
            var carDevice = discoveredDevices.Single(d => d.Name.Equals("CRIUS_BT"));

            if (!carDevice.Remembered)
                _service.Pair(carDevice);

            Console.WriteLine("Connecting...");
            _service.Connect(carDevice, BluetoothService.SerialPort);

            if (carDevice.IsConnected)
            {
                Console.WriteLine("Connected...");
                var signal = '0';
                while(signal != 'q')
                {
                    var input = Console.ReadLine();
                    if(input.Length > 0)
                    {
                        signal = input[0];
                        var sended = _service.Send(carDevice, signal).Result;
                        if (sended)
                            Console.WriteLine($"Sended signal: {signal}");
                        else
                            Console.WriteLine("Signal not sended properly.");
                    }
                }
            }
            _service.Disconnect();
        }
    }
}
