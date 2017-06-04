using BluetoothConnectivity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluetoothConnectivity.Services.Interfaces
{
    public interface IBluetoothSenderService
    {
        bool Pair(Device device, string pin);
        bool Connect(Device device, Guid serviceId);
        void Disconnect();
        Task<IList<Device>> GetDevices();
        Task<bool> Send(Device device, char signal);
    }
}
