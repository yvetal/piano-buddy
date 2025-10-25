using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Services
{
    public class MidiInMessageHandler
    {
        public event Action<string> NoteReceived;

        public void HandleMessage(MidiEvent midiEvent)
        {
            if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
            {
                string noteString = GetNoteString(noteOn);
                NoteReceived?.Invoke(noteString);
            }
        }
        private string GetNoteString(NoteOnEvent noteEvent)
        {
            return $"Note: {noteEvent.NoteName} ({noteEvent.NoteNumber})";
        }
    }
}
