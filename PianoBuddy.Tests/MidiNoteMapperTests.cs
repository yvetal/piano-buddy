using PianoBuddy.Services;

namespace PianoBuddy.Tests
{
    public class MidiNoteMapperTests
    {
        [Fact]
        public void ConvertNoteNumberToName()
        {
            var handler = new MidiNoteMapper();
            string noteLetter = handler.GetNoteLetter(60);
            Assert.Equal("C5", noteLetter);
            string noteLetter2 = handler.GetNoteLetter(49);
            Assert.Equal("C#4", noteLetter2);
        }
    }
}