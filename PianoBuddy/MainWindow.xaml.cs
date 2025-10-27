using System;
using System.Linq;
using System.Windows;
using NAudio.Midi;
using PianoBuddy.Services; 

namespace PianoBuddy
{
    public partial class MainWindow : Window
    {

        private readonly MainViewModel _vm;

        private MidiDeviceFinder _deviceFinder;
        private MidiService _midiService;

        public MainWindow()
        {
            InitializeComponent(); 
            _vm = new MainViewModel();
            DataContext = _vm;
            
            DetectMidiDevices();
            StartFirstMidiDevice();
        }

        private void DetectMidiDevices()
        {
            var provider = new MidiDeviceProvider();
            _deviceFinder = new MidiDeviceFinder(provider);

            _vm.Devices = _deviceFinder.FindDevices();
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
                _vm.CurrentNote = $"Note: {note.NoteName} ({note.NoteNumber})";
            });
        }
    }
}