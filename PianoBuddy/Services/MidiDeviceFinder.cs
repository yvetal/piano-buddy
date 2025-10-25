using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Services
{
    public interface IMidiDeviceProvider
    {
        string GetDeviceName(int index);
        int GetDeviceCount();
    }

    public class MidiDeviceProvider : IMidiDeviceProvider
    {
        public string GetDeviceName(int index) => MidiIn.DeviceInfo(index).ProductName;
        public int GetDeviceCount() => MidiIn.NumberOfDevices;
    }
    public class MidiDeviceFinder
    {
        private readonly IMidiDeviceProvider _deviceProvider;
        public MidiDeviceFinder(IMidiDeviceProvider deviceProvider)
        {
            _deviceProvider = deviceProvider;
        }
        public string FindDevices()
        {
            string deviceList = "MIDI Devices:\n";
            int deviceCount = _deviceProvider.GetDeviceCount();
            for (int i = 0; i < deviceCount; i++)
            {
                deviceList += $"{i}: {_deviceProvider.GetDeviceName(i)}\n";
            }
            return deviceList;
        }
    }
}
