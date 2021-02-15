using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace GoldMidiPlayer
{
    class MainPageData : INotifyPropertyChanged
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

        // CHANNEL VOLUME
        private int[] _channelsVolume = new int[16] { 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127 };
        private int[] _previusChannelsVolume = new int[16] { 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127 };


        public int Channel0Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(0); }
            set {
                _channelsVolume[0] = value;
                Debug.WriteLine("Channel0VolumeSetted");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel0Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel0Mute"));
            }
        }

        private int PreviusChannel0Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(0); }
            set { _previusChannelsVolume[0] = value; }
        }

        public bool Channel0Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(0) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel0Volume = Channel0Volume;
                    Channel0Volume = 0;
                }
                else
                {
                    Channel0Volume = PreviusChannel0Volume;
                }
            }
        }

        public int Channel1Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(1); }
            set
            {
                _channelsVolume[1] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel1Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel1Mute"));
            }
        }

        private int PreviusChannel1Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(1); }
            set { _previusChannelsVolume[1] = value; }
        }

        public bool Channel1Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(1) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel1Volume = Channel1Volume;
                    Channel1Volume = 0;
                }
                else
                {
                    Channel1Volume = PreviusChannel1Volume;
                }
            }
        }

        public int Channel2Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(2); }
            set
            {
                _channelsVolume[2] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel2Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel2Mute"));
            }
        }

        private int PreviusChannel2Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(2); }
            set { _previusChannelsVolume[2] = value; }
        }

        public bool Channel2Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(2) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel2Volume = Channel2Volume;
                    Channel2Volume = 0;
                }
                else
                {
                    Channel2Volume = PreviusChannel2Volume;
                }
            }
        }

        public int Channel3Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(3); }
            set
            {
                _channelsVolume[3] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel3Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel3Mute"));
            }
        }

        private int PreviusChannel3Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(3); }
            set { _previusChannelsVolume[3] = value; }
        }

        public bool Channel3Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(3) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel3Volume = Channel3Volume;
                    Channel3Volume = 0;
                }
                else
                {
                    Channel3Volume = PreviusChannel3Volume;
                }
            }
        }

        public int Channel4Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(4); }
            set
            {
                _channelsVolume[4] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel4Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel4Mute"));
            }
        }

        private int PreviusChannel4Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(4); }
            set { _previusChannelsVolume[4] = value; }
        }

        public bool Channel4Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(4) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel4Volume = Channel4Volume;
                    Channel4Volume = 0;
                }
                else
                {
                    Channel4Volume = PreviusChannel4Volume;
                }
            }
        }

        public int Channel5Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(5); }
            set
            {
                _channelsVolume[5] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel5Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel5Mute"));
            }
        }

        private int PreviusChannel5Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(5); }
            set { _previusChannelsVolume[5] = value; }
        }

        public bool Channel5Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(5) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel5Volume = Channel5Volume;
                    Channel5Volume = 0;
                }
                else
                {
                    Channel5Volume = PreviusChannel5Volume;
                }
            }
        }

        public int Channel6Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(6); }
            set
            {
                _channelsVolume[6] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel6Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel6Mute"));
            }
        }

        private int PreviusChannel6Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(6); }
            set { _previusChannelsVolume[6] = value; }
        }

        public bool Channel6Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(6) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel6Volume = Channel6Volume;
                    Channel6Volume = 0;
                }
                else
                {
                    Channel6Volume = PreviusChannel6Volume;
                }
            }
        }

        public int Channel7Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(7); }
            set
            {
                _channelsVolume[7] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel7Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel7Mute"));
            }
        }

        private int PreviusChannel7Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(7); }
            set { _previusChannelsVolume[7] = value; }
        }

        public bool Channel7Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(7) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel7Volume = Channel7Volume;
                    Channel7Volume = 0;
                }
                else
                {
                    Channel7Volume = PreviusChannel7Volume;
                }
            }
        }

        public int Channel8Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(8); }
            set
            {
                _channelsVolume[8] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel8Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel8Mute"));
            }
        }

        private int PreviusChannel8Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(8); }
            set { _previusChannelsVolume[8] = value; }
        }

        public bool Channel8Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(8) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel8Volume = Channel8Volume;
                    Channel8Volume = 0;
                }
                else
                {
                    Channel8Volume = PreviusChannel8Volume;
                }
            }
        }

        public int Channel9Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(9); }
            set
            {
                _channelsVolume[9] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel9Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel9Mute"));
            }
        }

        private int PreviusChannel9Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(9); }
            set { _previusChannelsVolume[9] = value; }
        }

        public bool Channel9Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(9) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel9Volume = Channel9Volume;
                    Channel9Volume = 0;
                }
                else
                {
                    Channel9Volume = PreviusChannel9Volume;
                }
            }
        }

        public int Channel10Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(10); }
            set
            {
                _channelsVolume[10] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel10Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel10Mute"));
            }
        }

        private int PreviusChannel10Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(10); }
            set { _previusChannelsVolume[10] = value; }
        }

        public bool Channel10Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(10) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel10Volume = Channel10Volume;
                    Channel10Volume = 0;
                }
                else
                {
                    Channel10Volume = PreviusChannel10Volume;
                }
            }
        }

        public int Channel11Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(11); }
            set
            {
                _channelsVolume[11] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel11Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel11Mute"));
            }
        }

        private int PreviusChannel11Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(11); }
            set { _previusChannelsVolume[11] = value; }
        }

        public bool Channel11Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(11) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel11Volume = Channel11Volume;
                    Channel11Volume = 0;
                }
                else
                {
                    Channel11Volume = PreviusChannel11Volume;
                }
            }
        }

        public int Channel12Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(12); }
            set
            {
                _channelsVolume[12] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel12Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel12Mute"));
            }
        }

        private int PreviusChannel12Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(12); }
            set { _previusChannelsVolume[12] = value; }
        }

        public bool Channel12Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(12) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel12Volume = Channel12Volume;
                    Channel12Volume = 0;
                }
                else
                {
                    Channel12Volume = PreviusChannel12Volume;
                }
            }
        }

        public int Channel13Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(13); }
            set
            {
                _channelsVolume[13] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel13Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel13Mute"));
            }
        }

        private int PreviusChannel13Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(13); }
            set { _previusChannelsVolume[13] = value; }
        }

        public bool Channel13Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(13) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel13Volume = Channel13Volume;
                    Channel13Volume = 0;
                }
                else
                {
                    Channel13Volume = PreviusChannel13Volume;
                }
            }
        }

        public int Channel14Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(14); }
            set
            {
                _channelsVolume[14] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel14Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel14Mute"));
            }
        }

        private int PreviusChannel14Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(14); }
            set { _previusChannelsVolume[14] = value; }
        }

        public bool Channel14Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(14) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel14Volume = Channel14Volume;
                    Channel14Volume = 0;
                }
                else
                {
                    Channel14Volume = PreviusChannel14Volume;
                }
            }
        }

        public int Channel15Volume
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(15); }
            set
            {
                _channelsVolume[15] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel15Volume"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Channel15Mute"));
            }
        }

        private int PreviusChannel15Volume
        {
            get { return _previusChannelsVolume.ElementAtOrDefault<int>(15); }
            set { _previusChannelsVolume[15] = value; }
        }

        public bool Channel15Mute
        {
            get { return _channelsVolume.ElementAtOrDefault<int>(15) == 0; }
            set
            {
                if (value)
                {
                    PreviusChannel15Volume = Channel15Volume;
                    Channel15Volume = 0;
                }
                else
                {
                    Channel15Volume = PreviusChannel15Volume;
                }
            }
        }

        // SOLO CHANNELS
        private bool[] _soloChannels = new bool[16];
        public bool SoloChannel1
        {
            get { return _soloChannels[0]; }
            set { _soloChannels[0] = value; }
        }



        public List<ProgramsModel> Programs { get; private set; }
        public List<CategoryModel> Categories { get; private set; }
        private List<ProgramsModel> _activePrograms;
        public List<ProgramsModel> ActivePrograms
        {
            get
            {
                return _activePrograms;
            }
            set
            {
                _activePrograms = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActivePrograms"));
            }
        }
        public CategoryModel ActiveCategory { get; set; }

        public void SetActivePrograms(int categoryIndex)
        {
            CategoryModel category = Categories.ElementAt(categoryIndex);
            ActiveCategory = category;
            ActivePrograms = Programs.GetRange(category.StartIndex, category.Count);
        }

        private List<PlaylistModel> _playlistManager;
        
        public Array PlaylistManager
        {
            get
            {
                return _playlistManager.ToArray();
            }
            set
            {
                Array playlistToSet = (Array)value;
                _playlistManager = playlistToSet.OfType<PlaylistModel>().ToList();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlaylistManager"));
            }
        }
        
        private PlaylistModel _activePlaylist = new PlaylistModel("", new List<MidiFile>());
        public PlaylistModel ActivePlaylist
        {
            get
            {
                return _activePlaylist;
            }
            private set
            {
                _activePlaylist = value;
            }
        }

        public void SetPlaylist(PlaylistModel playlist)
        {
            Debug.WriteLine("Playlist Name: {0} :: Playlist Songs: {1}", playlist.Name, playlist.Songs.Count);
            
            if (_playlistManager.Contains(playlist))
            {
                Debug.WriteLine("Contiene playlist con el mismo nombre");
                _playlistManager.Remove(playlist);
            }
            Debug.WriteLine("Añadiendo nueva ActivePlaylist");
            ActivePlaylist = playlist;
            _playlistManager.Add(playlist);
            PlaylistManager = _playlistManager.ToArray();
            Debug.WriteLine("Finalizado metodo SetPlaylist. Playlist.Count: {0}", _playlistManager.Count);
            Debug.WriteLine("Active Playlist: {0} with {1} songs.", ActivePlaylist.Name, ActivePlaylist.Songs.Count);
        }

        public PlaylistModel GetPlaylist(string playlistName)
        {
            PlaylistModel playlistFinded = null;
            //Debug.WriteLine("Playlis Foreach [ NameToFind: {0} ] [ PlaylistsCount: {1} ]", playlistName, PlaylistManager.Count);
            foreach (PlaylistModel playlist in PlaylistManager)
            {
                Debug.WriteLine("playlist.Name: {0}, {1} :playlistName", playlist.Name, playlistName);
                if (playlist.Name == playlistName)
                {
                    Debug.WriteLine("Coincidencia encontrada en MainPageData.GetPlaylist");
                    playlistFinded = playlist;
                    break;
                }
            }
            return playlistFinded;
        }

        public void RemovePlayList(PlaylistModel playlist)
        {
            Debug.WriteLine("Deleting Playlist NAME [{0}] | ActivePlayList: [{1}]", playlist.Name, ActivePlaylist.Name);
            if (playlist == ActivePlaylist)
            {
                _playlistManager.Remove(playlist);
                PlaylistManager = _playlistManager.ToArray();
                Debug.WriteLine("RemovePlaylist Ended");
            }
        }

        MidiFile ActiveMidiFile;

        public MainPageData()
        {
            List<MidiFile> midiFiles = new List<MidiFile>();
            List<PlaylistModel> playlists = new List<PlaylistModel>();
            PlaylistModel init_playlist = new PlaylistModel("Rock", midiFiles);
            playlists.Add(init_playlist);
            PlaylistManager = playlists.ToArray();
            Programs = new List<ProgramsModel>();
            Categories = new List<CategoryModel>();

            var gmFile = System.Xml.Linq.XDocument.Load(@"Assets\general_midi_category.xml");
            XName rootName = "GENERAL-MIDI";
            XName categoriesName = "CATEGORY";
            XName programName = "PROGRAM";
            XName categoryName = "NAME";

            var xRoot = gmFile.Element(rootName);
            var xCategories = xRoot.Descendants(categoriesName);

            int programIndex = 0;
            int categoryIndex = 0;
            foreach (XElement category in xCategories)
            {
                var xName = category.Element(categoryName).Value;
                int programByCategoryIndex = 0;
                foreach (XElement program in category.Descendants(programName))
                { 
                    programByCategoryIndex += 1;
                    var childs = program.Descendants();
                    programIndex = int.Parse(childs.ElementAt(0).Value);
                    // Debug.WriteLine("Programa {0}, con index {2} de la categoria {1}", programByCategoryIndex, xName, programIndex);
                    string program_name = childs.ElementAt(1).Value;
                    Programs.Add(new ProgramsModel(programIndex, program_name));
                }
                var generalIndex = programIndex - programByCategoryIndex;
                categoryIndex += 1;
                Categories.Add(new CategoryModel(categoryIndex, xName.ToString(), generalIndex, programByCategoryIndex));
            }
        }
    }
}
