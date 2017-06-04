using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluetoothConnectivity.Services.Interfaces
{
    public interface IBluetoothReceiverService
    {
        Task<IList<Device>> GetDevices();

        /// <summary>
        /// Sends the data to the Receiver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="content">The content.</param>
        /// <returns>If was sent or not.</returns>
        Task<bool> Send(Device device, string content);
    }
}
