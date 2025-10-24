using NAudio.Midi;
using PianoBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoBuddy.Tests
{
    public class MidiInMessageHandlerTests
    {

        [Fact]
        public void TestMapMidiEventToNoteString()
        {
            var handler = new MidiInMessageHandler();
            string received = null;
            handler.NoteReceived += s => received = s;

            var noteEvent = new NoteOnEvent(0, 1, 60, 100, 0);

            handler.HandleMessage(noteEvent);
        }

        [Fact]
        public void TestMapMidiEventToNoteString_NoAction()
        {
            var handler = new MidiInMessageHandler();
            bool eventFired = false;
            handler.NoteReceived += _ => eventFired = true;

            var controlChange = new ControlChangeEvent(0, 1, MidiController.Pan, 127);

            handler.HandleMessage(controlChange);

            Assert.False(eventFired);
        }
    }
}
