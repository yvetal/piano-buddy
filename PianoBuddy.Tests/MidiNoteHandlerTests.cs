
using NAudio.Midi;
using PianoBuddy.Services;

namespace PianoBuddy.Tests
{
    public class MidiNoteHandlerTests
    {
        [Fact]
        public void Test1()
        {
            var handler = new MidiNoteHandler();
            var noteEvent = new NoteOnEvent(0, 1, 60, 100, 0);
            var result = handler.GetNoteString(noteEvent);

            Assert.Equal("Note: C5 (60)", result);
        }
    }
}