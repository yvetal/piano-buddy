using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Services
{
    public interface IMidiIn
    {
        event EventHandler<MidiInMessageEventArgs> MessageReceived;
        void Start();
        void Stop();
        void Dispose();
    }
    public class MidiInWrapper : IMidiIn
    {
        private readonly MidiIn _midiIn;

        public event EventHandler<MidiInMessageEventArgs> MessageReceived
        {
            add { _midiIn.MessageReceived += value; }
            remove { _midiIn.MessageReceived -= value; }
        }

        public MidiInWrapper(int deviceIndex)
        {
            _midiIn = new MidiIn(deviceIndex);
        }

        public void Start() => _midiIn.Start();
        public void Stop() => _midiIn.Stop();
        public void Dispose() => _midiIn.Dispose();
    }
    public class MidiService
    {
        private readonly Func<int, IMidiIn> _midiInFactory;
        private IMidiIn _midiIn;
        public event EventHandler<NoteEvent> NoteReceived;

        public MidiService(Func<int, IMidiIn> midiInFactory)
        {
            _midiInFactory = midiInFactory;
        }
        public void Start(int deviceIndex)
        {
            _midiIn = _midiInFactory(deviceIndex);
            _midiIn.MessageReceived += OnMessageReceived;
            _midiIn.Start();
        }
        public void Stop()
        {
            _midiIn?.Stop();
            _midiIn?.Dispose();
        }
        private void OnMessageReceived(object sender, MidiInMessageEventArgs e)
        {
            var midiEvent = MidiEvent.FromRawMessage(e.RawMessage);

            if (midiEvent is NoteEvent noteEvent)
            {
                NoteReceived?.Invoke(this, noteEvent);
            }
        }
    }
}
