using Moq;
using PianoBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Tests
{
    public class MidiDeviceFinderTests
    {
        [Fact]
        public void TestFindDevices()
        {
            var mockProvider = new Mock<IMidiDeviceProvider>();
            mockProvider.Setup(p => p.GetDeviceCount()).Returns(2);
            mockProvider.Setup(p => p.GetDeviceName(0)).Returns("Keyboard");
            mockProvider.Setup(p => p.GetDeviceName(1)).Returns("DrumPad");

            var detector = new MidiDeviceFinder(mockProvider.Object);
            var devices = detector.FindDevices();   

            Assert.Equal("MIDI Devices:\n0: Keyboard\n1: DrumPad\n", devices);
        }
    }
}
