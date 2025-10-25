using NAudio.Midi;
using PianoBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Tests
{
    public class FakeMidiIn : IMidiIn
    {
        public event EventHandler<MidiInMessageEventArgs> MessageReceived = delegate { };
        public bool Started { get; private set; } = false;
        public bool Disposed { get; private set; }

        public int Index { get; }

        public FakeMidiIn(int index = 0)
        {
            Index = index;
        }
        public void Start() => Started = true;
        public void Stop() => Started = false;
        public void Dispose() => Disposed = true;
        public void SimulateMessage(MidiInMessageEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
    public class MidiServiceTests
    {

        [Fact]
        public void TestStart()
        {
            var fakeDevice = new FakeMidiIn(0);
            var service = new MidiService(index => fakeDevice);
            
            service.Start(0);

            Assert.True(fakeDevice.Started);
            Assert.Equal(0, fakeDevice.Index);

        }
        [Fact]
        public void NoteReceived_EventIsRaised_WhenMidiMessageArrives()
        {
            var fakeDevice = new FakeMidiIn(0);
            var service = new MidiService(index => fakeDevice);
            
            NoteEvent receivedNote = null;
            service.NoteReceived += (s, e) => receivedNote = e;

            service.Start(0);

            var noteOn = new NoteOnEvent(0, 1, 64, 127, 0);
            int raw = noteOn.GetAsShortMessage();
            var message = new MidiInMessageEventArgs(raw, 0);

            fakeDevice.SimulateMessage(message);

            Assert.NotNull(receivedNote);

            Assert.Equal(noteOn.NoteNumber, receivedNote.NoteNumber);
            Assert.Equal(noteOn.Velocity, receivedNote.Velocity);
            Assert.Equal(noteOn.Channel, receivedNote.Channel);
            Assert.Equal(noteOn.CommandCode, receivedNote.CommandCode);
        }
        [Fact]
        public void Stop_ShouldStopAndDisposeMidiDevice()
        {
            // Arrange
            var fakeDevice = new FakeMidiIn(0);
            var service = new MidiService(i => fakeDevice);
            service.Start(0);

            // Act
            service.Stop();

            // Assert
            Assert.False(fakeDevice.Started);    // Stop() sets Started to false
            Assert.True(fakeDevice.Disposed);    // Dispose() should have been called
        }
    }
}
