using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BluetoothConnectivity.Models;
using BluetoothConnectivity.Services.Interfaces;
using InTheHand.Net.Sockets;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;

namespace BluetoothConnectivity.Services
{
    public class BluetoothSenderService : IBluetoothSenderService
    {
        private NetworkStream _bluetoothStream;
        private BluetoothClient _bluetoothClient;

        public bool Connect(Device device, Guid serviceId = default(Guid))
        {
            _bluetoothClient = _bluetoothClient ?? new BluetoothClient();
            serviceId = serviceId.Equals(default(Guid)) ? BluetoothService.SerialPort : serviceId;
            var endPoint = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, serviceId);

            if (!_bluetoothClient.Connected)
                _bluetoothClient.Connect(endPoint);

            return _bluetoothClient.Connected;
        }

        public void Disconnect()
        {
            _bluetoothStream?.Close();
            _bluetoothClient?.Close();
        }

        public async Task<IList<Device>> GetDevices()
        {
            var task = Task.Run(() =>
            {
                var devices = new List<Device>();
                using (var bluetoothClient = new BluetoothClient())
                {
                    var array = bluetoothClient.DiscoverDevices();
                    var count = array.Length;
                    for (var i = 0; i < count; i++)
                    {
                        devices.Add(new Device(array[i]));
                    }
                }
                return devices;
            });
            return await task;
        }

        public bool Pair(Device device, string pin = "0000")
            => BluetoothSecurity.PairRequest(device.DeviceInfo.DeviceAddress, pin);

        public async Task<bool> Send(Device device, char signal)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            if (_bluetoothClient == null)
                throw new ArgumentNullException("client");

            var task = Task.Run(() =>
            {
                _bluetoothStream = _bluetoothClient.GetStream();

                if (_bluetoothClient.Connected && _bluetoothStream != null)
                {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(signal.ToString());
                    _bluetoothStream.Write(buffer, 0, buffer.Length);
                    _bluetoothStream.Flush();
                    return true;
                }
                return false;
            });
            return await task;
        }
    }
}
