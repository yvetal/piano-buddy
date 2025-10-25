using System;
using System.Linq;
using System.Windows;
using NAudio.Midi;
using PianoBuddy.Services; 

namespace PianoBuddy
{
    public partial class MainWindow : Window
    {
        private MidiDeviceFinder _deviceFinder;
        private MidiService _midiService;

        public MainWindow()
        {
            InitializeComponent();
            DetectMidiDevices();
            StartFirstMidiDevice();
        }

        private void DetectMidiDevices()
        {
            var provider = new MidiDeviceProvider();
            _deviceFinder = new MidiDeviceFinder(provider);

            DevicesTextBlock.Text = _deviceFinder.FindDevices();
        }

        private void StartFirstMidiDevice()
        {
            _midiService = new MidiService(i => new MidiInWrapper(i)); // Wrap NAudio MidiIn
            _midiService.NoteReceived += OnNoteReceived;
            _midiService.Start(0);
        }

        private void OnNoteReceived(object sender, NoteEvent note)
        {
            Dispatcher.Invoke(() =>
            {
                NoteTextBlock.Text = $"Note: {note.NoteName} ({note.NoteNumber})";
            });
        }
    }
}