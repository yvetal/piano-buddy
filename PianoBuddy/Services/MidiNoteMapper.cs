using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Services
{
    public class MidiNoteMapper
    {
        public MidiNoteMapper() { }

        private static readonly string[] NoteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public string GetNoteLetter(int noteNumber)
        {
            return NoteNames[noteNumber%12] + (noteNumber/12).ToString();
            
        }
    }
}
