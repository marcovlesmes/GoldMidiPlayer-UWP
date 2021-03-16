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
    #region STATES

    [Flags]
    public enum States { 
            Idle = 0b_0000_0000,
            Midi_loaded = 0b_0000_0001,
            Sound_font_loaded = 0b_0000_0010,
            All_loaded = 0b_0000_0011
    }
    #endregion
public class MainModule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region MAIN VIEW VALUES
        private States _state;
        public States state
        {
            get { return _state; }
            set
            {
                _state = value;
            }
        }
        private int _globalVolume;
        public int GlobalVolume
        {
            get { return _globalVolume; }
            set
            {
                _globalVolume = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalVolume"));
            }
        }
        private int _globalPitch;
        public int GlobalPitch
        {
            get { return _globalPitch; }
            set
            {
                _globalPitch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalPitch"));
            }
        }
        private int _globalTempo;
        public int GlobalTempo
        {
            get { return _globalTempo; }
            set
            {
                _globalTempo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GlobalTempo"));
            }
        }
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }
        private string _displayKey;
        public string DisplayKey
        {
            get
            {
                if (_displayKey != null)
                    return Utility.ExtractKey(_displayKey);
                return "N/A";
            }
            set
            {
                _displayKey = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayKey"));
            }
        }
        private double _positionInTime;
        public double PositionInTime
        {
            get { return _positionInTime; }
            set
            {
                _positionInTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PositionInTime"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PositionInTimeToString"));
            }
        }
        public string PositionInTimeToString
        {
            get 
            {
                return Utility.SecondsToTime(_positionInTime);
            }
        }
        private double _midiLenght;
        public double MidiLenght
        {
            get { return _midiLenght; }
            set
            {
                _midiLenght = value;
                Debug.WriteLine("Setting value in MidiLenght: {0}", value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MidiLenght"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MidiLenghtToString"));
            }
        }
        public string MidiLenghtToString
        {
            get
            {
                return Utility.SecondsToTime(_midiLenght);
            }
        }
        
        private PlaylistModel _activePlaylist;
        public PlaylistModel ActivePlaylist
        {
            get { return _activePlaylist; }
            set
            {
                _activePlaylist = value;
            }
        }
        #endregion
        
        #region METHODS

        internal void InitPlaylist()
        {
            List<MidiFile> songs = new List<MidiFile>();
            ActivePlaylist = new PlaylistModel("Default", songs);
        }

        internal void AddMidiFile(MidiFile midiFile)
        {
            ActivePlaylist.AddSong(midiFile);
            DisplayName = midiFile.Name;
            DisplayKey = midiFile.TimeSignatureAsString;
            MidiLenght = midiFile.Lenght;
        }

        public MidiFile GetMidiFile()
        {
            return ActivePlaylist.GetCurrentSong();
        }
        #endregion
    }
}
