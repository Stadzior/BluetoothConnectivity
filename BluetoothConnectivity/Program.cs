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
            var radioB = BluetoothRadio.AllRadios;
            var _service = new BluetoothSenderService();
            var _service2 = new BluetoothSenderService();

            Console.WriteLine("Looking for the devices...");
            var discoveredDevices = _service.GetDevices().Result;
            var arduinoName = "HC-06";
            var phoneName = "ALCATEL ONETOUCH POP C7";
            Console.WriteLine($"Retrieving {arduinoName}...");
            var arduinoDevice = discoveredDevices.Single(d => d.Name.Equals(arduinoName));
            var phoneDevice = discoveredDevices.Single(d => d.Name.Equals(phoneName));

            if (!arduinoDevice.Remembered)
                _service.Pair(arduinoDevice);

            if (!phoneDevice.Remembered)
                _service.Pair(phoneDevice);

            Console.WriteLine($"Connecting to {arduinoName}..."); 
            var connectedToArduino = _service.Connect(arduinoDevice, BluetoothService.SerialPort);
            Console.WriteLine($"Connecting to {phoneName}...");
            var connectedToPhone = _service2.Connect(phoneDevice, BluetoothService.SerialPort);

            if (connectedToArduino && connectedToPhone)
            {
                Console.WriteLine("Connected...");
                var signal = '0';
                while(signal != 'q')
                {
                    var input = Console.ReadLine();
                    if(input.Length > 0)
                    {
                        signal = input[0];
                        var sended = _service.Send(arduinoDevice, signal).Result && _service2.Send(phoneDevice, signal).Result;
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
