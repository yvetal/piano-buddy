
using NAudio.Midi;
using PianoBuddy.Services;

namespace PianoBuddy.Tests
{
    public class MidiNoteMapperTests
    {
        [Fact]
        public void TestMapMidiEventToNoteString()
        {
            var handler = new MidiNoteMapper();
            var noteEvent = new NoteOnEvent(0, 1, 60, 100, 0);
            var result = handler.GetNoteString(noteEvent);

            Assert.Equal("Note: C5 (60)", result);
        }
    }
}