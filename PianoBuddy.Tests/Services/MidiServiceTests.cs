using NAudio.Midi;
using PianoBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Tests.Services
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
    public class MidiServiceFixture : IDisposable
    {
        public FakeMidiIn FakeDevice { get; }
        public MidiService Service { get; }

        public MidiServiceFixture()
        {
            FakeDevice = new FakeMidiIn(0);
            Service = new MidiService(index => FakeDevice);
            Service.Start(0);
        }

        public void Dispose()
        {
            Service.Stop();
        }
    }

    public class MidiServiceTests : IClassFixture<MidiServiceFixture>
    {
        private readonly MidiServiceFixture _fixture;
        public MidiServiceTests(MidiServiceFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void TestStart()
        {
            Assert.True(_fixture.FakeDevice.Started);
            Assert.Equal(0, _fixture.FakeDevice.Index);
        }
        [Fact]
        public void NoteReceived_EventIsRaised_WhenMidiMessageArrives()
        {
            NoteEvent receivedNote = null;
            _fixture.Service.NoteReceived += (s, e) => receivedNote = e;

            var noteOn = new NoteOnEvent(0, 1, 64, 127, 0);
            var message = new MidiInMessageEventArgs(noteOn.GetAsShortMessage(), 0);

            _fixture.FakeDevice.SimulateMessage(message);

            Assert.NotNull(receivedNote);
            CompareNotes(receivedNote, noteOn);
        }

        private static void CompareNotes(NoteEvent receivedNote, NoteOnEvent noteOn)
        {
            Assert.Equal(noteOn.NoteNumber, receivedNote.NoteNumber);
            Assert.Equal(noteOn.Velocity, receivedNote.Velocity);
            Assert.Equal(noteOn.Channel, receivedNote.Channel);
            Assert.Equal(noteOn.CommandCode, receivedNote.CommandCode);
        }

        [Fact]
        public void Stop_ShouldStopAndDisposeMidiDevice()
        {
            _fixture.Service.Stop();

            Assert.False(_fixture.FakeDevice.Started);
            Assert.True(_fixture.FakeDevice.Disposed);
        }
    }
}
