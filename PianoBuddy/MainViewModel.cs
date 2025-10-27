using NAudio.Midi;
using PianoBuddy.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PianoBuddy
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MidiDeviceFinder _deviceFinder;
        private readonly MidiService _midiService;
        private readonly SynchronizationContext _uiContext;

        private string _devices;
        public string Devices
        {
            get => _devices;
            set { _devices = value; OnPropertyChanged(); }
        }

        private string _currentNote = "No note played yet";
        public string CurrentNote
        {
            get => _currentNote;
            set { _currentNote = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();

            var provider = new MidiDeviceProvider();
            _deviceFinder = new MidiDeviceFinder(provider);
            _midiService = new MidiService(i => new MidiInWrapper(i));

            _midiService.NoteReceived += OnNoteReceived;

            DetectMidiDevices();
            StartFirstMidiDevice();
        }

        private void DetectMidiDevices()
        {
            Devices = _deviceFinder.FindDevices();
        }

        private void StartFirstMidiDevice()
        {
            _midiService.Start(0);
        }

        private void OnNoteReceived(object sender, NoteEvent note)
        {
            _uiContext.Post(_ =>
            {
                CurrentNote = $"Note: {note.NoteName} ({note.NoteNumber})";
            }, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
