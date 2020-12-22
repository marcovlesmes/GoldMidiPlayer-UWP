using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldMidiPlayer
{
    class MainPageData: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _midiName;
        public string MidiName
        {
            get { return _midiName; }
            set
            {
                if (value == null)
                    _midiName = "MIDI NAME";
                else
                    _midiName = value;
            }
        }

        // GLOBAL VOLUME
        private int _globalVolume;
        private int _globalVolumeText;
        public int GlobalVolume {
            get {
                return _globalVolume;
            }
            set
            {
                float scaledValue = Utility.Linear((float)value, 0f, 127f, 0f, 1f);
                _globalVolume = (int)scaledValue;
                _globalVolumeText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalVolumeText"));
            }
        }
        public string GlobalVolumeText
        { get { return _globalVolumeText.ToString(); } }

        // GLOBAL PITCH
        private int _globalPitch;
        private int _globalPitchText;
        public int GlobalPitch
        {
            get
            {
                return _globalPitch;
            }
            set
            {
                int scaledValue = (int)Utility.Linear((float)value, -12f, 12f, 0f, 200f);
                _globalPitch = scaledValue;
                _globalPitchText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalPitchText"));
            }
        }
        public string GlobalPitchText
        { get { return _globalPitchText.ToString(); } }

        // GLOBAL TEMPO
        private int _globalTempo;
        private int _globalTempoText;
        public int GlobalTempo
        {
            get { return _globalTempo; }
            set 
            {
                int scaledValue = 60000000 / (int)value;
                _globalTempo = scaledValue;
                _globalTempoText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalTempoText"));
            }
        }
        public string GlobalTempoText
        { get { return _globalTempoText.ToString(); } }

        // CURRENT MIDI POSITION
        private double _midiLenght;
        private double _currentMidiPosition;
        public double CurrentMidiPosition
        {
            get { return _currentMidiPosition; }
            set
            {
                _currentMidiPosition = _midiLenght * (value * 0.01);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentMidiPositionText"));
            }
        }
        public string CurrentMidiPositionText
        {
            get { return Utility.SecondsToTime(_currentMidiPosition); }
        }
        public string MidiLenghtText
        {
            get { return Utility.SecondsToTime(_midiLenght); }
            set
            {
                _midiLenght = Utility.TimeToSeconds(value);
            }
        }

    }
}
