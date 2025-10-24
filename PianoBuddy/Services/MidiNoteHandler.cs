using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Services
{
    public class MidiNoteHandler
    {
        public string GetNoteString(NoteEvent noteEvent)
        {
            return $"Note: {noteEvent.NoteName} ({noteEvent.NoteNumber})";
        }
    }
}
