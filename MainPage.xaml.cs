using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.WindowManagement.Preview;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Common;
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoldMidiPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {

        public static Dictionary<UIContext, AppWindow> appWindows = new Dictionary<UIContext, AppWindow>();

        private ThreadPoolTimer RefreshCurrentPlaying;
        public MainModule MainModule;
        public BassManager bassManager = new BassManager();
        public MidiFile CurrentFile;
        #region STYLES VARIABLES
        SolidColorBrush PrimaryColorText = new SolidColorBrush(Colors.White);
        SolidColorBrush SecondaryColorText = new SolidColorBrush(Colors.DarkSlateGray);
        #endregion

        private States state
        {
            get { return MainModule.state; }
            set
            {
                MainModule.state = value;
                UpdateState();
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            MainModule = new MainModule();
            state = States.Idle;


        }

        public MainModule GetMainModule()
        {
            return this.MainModule;
        }

        public async Task<bool> ShowWindow(Type type, ToggleButton toggleButton)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame frame = new Frame();
            frame.Navigate(type, this);
            ElementCompositionPreview.SetAppWindowContent(appWindow, frame);
            UIContext uIContext = frame.UIContext;
            appWindows.Add(uIContext, appWindow);
            appWindow.Closed += delegate
                {
                    MainPage.appWindows.Remove(uIContext);
                    frame.Content = null;
                    appWindow = null;
                    toggleButton.IsChecked = false;
                };
            try
            {
                Debug.WriteLine("Is de Nuget Pakage");
                await appWindow.TryShowAsync();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Exeption in ShowWindow");
                Debug.WriteLine(exception.Message);
            }
            return true;
        }

        public async Task<bool> CloseWindow(AppWindow window)
        {
            await window.CloseAsync();
            return true;
        }

        public void SetGlobalVolume(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            Slider slider = sender as Slider;
            double value = slider.Value;
            bassManager.SetGlobalVolume(value);
        }

        public void SetGlobalPitch(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            Slider slider = sender as Slider;
            double value = slider.Value;
            bassManager.SetGlobalPitch(value);
        }

        public void SetGlobalTempo(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            Slider slider = sender as Slider;
            double value = slider.Value;
            bassManager.SetGlobalTempo(value);
        }

        private async void MixerBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            if (toggleButton.IsChecked == true)
                await ShowWindow(typeof(MixerPage), toggleButton);
            else
                await CloseWindow(MixerPage.window);
        }

        private void MixerBtn_Checked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Checked Mixer");
        }

        private void PlayMidi(object sender, RoutedEventArgs e)
        {
            bool IsPlaying = bassManager.Play();
            ToggleButton tb = sender as ToggleButton;

            if (tb.IsChecked == false)
            {
                PlayButton.IsChecked = true;
                PauseButton.IsChecked = false;
            }

            PlayButton.IsEnabled = false;
            TimeSpan interval = TimeSpan.FromSeconds(1);
            RefreshCurrentPlaying = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                // Update Midi position
                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        // Update UI
                        if (bassManager.IsPlaying())
                        {
                            MainModule.PositionInTime = bassManager.GetMidiPosition();
                            bool CheckEndOfPlaying = bassManager.IsPlaying();
                            if (!CheckEndOfPlaying)
                            {
                                RefreshCurrentPlaying.Cancel();
                            }
                        }
                    });
            }, interval);
        }

        private async void OpenMidi(object sender, RoutedEventArgs e)
        {
            try
            {
                MidiFile midiFile = await bassManager.LoadFile();
                PlaylistModel playlist = MainModule.ActivePlaylist;
                
                if (playlist == null)
                    MainModule.InitPlaylist();
                
                if (midiFile != null)
                {
                    MainModule.AddMidiFile(midiFile);
                    if (state == States.Idle || state == States.Midi_loaded)
                        state = States.Midi_loaded;
                    else
                        state = States.All_loaded;
                }
            }
            catch (Exception exep)
            {
                Debug.WriteLine(exep.Message);
            }
        }

        private void UpdateState()
        {
            Debug.WriteLine(MainModule.state);
            switch (MainModule.state)
            {
                case States.Idle:
                    GlobalVolText.Foreground = SecondaryColorText;
                    GlobalVolumeSlider.IsEnabled = false;
                    OpenMidiButton.IsEnabled = true;
                    SaveMidiButton.IsEnabled = false;
                    OpenSoundfontButton.IsEnabled = true;
                    GoldMidiPageButton.IsEnabled = true;
                    Mp3ExportButton.IsEnabled = false;
                    MixerButton.IsEnabled = false;
                    PianoButton.IsEnabled = true;
                    SettingsButton.IsEnabled = true;
                    PlaylistButton.IsEnabled = false;
                    KeyText.Foreground = SecondaryColorText;
                    MidiNameText.Foreground = SecondaryColorText;
                    MidiTimeSignatureText.Foreground = SecondaryColorText;
                    MidiCurrentTimeText.Foreground = SecondaryColorText;
                    MidiPositionSlider.IsEnabled = false;
                    MidiLenghtText.Foreground = SecondaryColorText;
                    AutoplayButton.IsEnabled = false;
                    RepeatAllButton.IsEnabled = false;
                    RepeatSongButton.IsEnabled = false;
                    PreviusButton.IsEnabled = false;
                    RewindButton.IsEnabled = false;
                    ForwardButton.IsEnabled = false;
                    NextButton.IsEnabled = false;
                    StopButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    PauseButton.IsEnabled = false;
                    GlobalPitchText.Foreground = SecondaryColorText;
                    GlobalPitchSlider.IsEnabled = false;
                    GlobalTempoText.Foreground = SecondaryColorText;
                    GlobalTempoSlider.IsEnabled = false;
                    Debug.WriteLine("Updated to Idle State");
                    break;
                case States.Midi_loaded:
                    SaveMidiButton.IsEnabled = true;
                    MixerButton.IsEnabled = true;
                    PlaylistButton.IsEnabled = true;
                    KeyText.Foreground = PrimaryColorText;
                    MidiNameText.Foreground = PrimaryColorText;
                    MidiTimeSignatureText.Foreground = PrimaryColorText;
                    MidiCurrentTimeText.Foreground = PrimaryColorText;
                    MidiPositionSlider.IsEnabled = true;
                    MidiLenghtText.Foreground = PrimaryColorText;
                    AutoplayButton.IsEnabled = true;
                    RepeatAllButton.IsEnabled = true;
                    RepeatSongButton.IsEnabled = true;
                    GlobalTempoSlider.Value = MainModule.GetMidiFile().GlobalTempo;
                    Debug.WriteLine("Updated to Midi Loaded");
                    break;
                case States.Sound_font_loaded:
                    Debug.WriteLine("Updated to SoundFont Loaded");
                    break;
                case States.All_loaded:
                    GlobalVolText.Foreground = PrimaryColorText;
                    GlobalVolumeSlider.IsEnabled = true;
                    OpenMidiButton.IsEnabled = true;
                    SaveMidiButton.IsEnabled = true;
                    OpenSoundfontButton.IsEnabled = true;
                    GoldMidiPageButton.IsEnabled = true;
                    Mp3ExportButton.IsEnabled = true;
                    MixerButton.IsEnabled = true;
                    PianoButton.IsEnabled = true;
                    SettingsButton.IsEnabled = true;
                    PlaylistButton.IsEnabled = true;
                    KeyText.Foreground = PrimaryColorText;
                    MidiNameText.Foreground = PrimaryColorText;
                    MidiTimeSignatureText.Foreground = PrimaryColorText;
                    MidiCurrentTimeText.Foreground = PrimaryColorText;
                    MidiPositionSlider.IsEnabled = true;
                    MidiLenghtText.Foreground = PrimaryColorText;
                    AutoplayButton.IsEnabled = true;
                    RepeatAllButton.IsEnabled = true;
                    RepeatSongButton.IsEnabled = true;
                    PreviusButton.IsEnabled = true;
                    RewindButton.IsEnabled = true;
                    ForwardButton.IsEnabled = true;
                    NextButton.IsEnabled = true;
                    StopButton.IsEnabled = true;
                    PlayButton.IsEnabled = true;
                    PauseButton.IsEnabled = true;
                    GlobalPitchText.Foreground = PrimaryColorText;
                    GlobalPitchSlider.IsEnabled = true;
                    GlobalTempoText.Foreground = PrimaryColorText;
                    GlobalTempoSlider.IsEnabled = true;
                    Debug.WriteLine("Updating to All Loaded!");
                    break;
                default:
                    Debug.WriteLine("Dont exist this state tin UpdateState from MainPage");
                    break;
            }
        }

        private async void OpenFont(object sender, RoutedEventArgs e)
        {
            bool success = await bassManager.LoadFonts();
            if (success)
                if (state == States.Idle || state == States.Sound_font_loaded)
                    state = States.Sound_font_loaded;
                else
                    state = States.All_loaded;
        }

        private void StopMidi(object sender, RoutedEventArgs e)
        {
            bool IsStoped = bassManager.Stop();
            if (IsStoped && RefreshCurrentPlaying != null)
            {
                RefreshCurrentPlaying.Cancel();
                MidiPositionSlider.Value = 0;
                PlayButton.IsEnabled = true;
                PlayButton.IsChecked = false;
                PauseButton.IsChecked = false;
            }
        }

        private void PauseMidi(object sender, RoutedEventArgs e)
        {
            bool isPlaying = bassManager.IsPlaying();
            if (isPlaying)
            {
                bassManager.Pause();
                PlayButton.IsEnabled = true;
                if (RefreshCurrentPlaying != null)
                    RefreshCurrentPlaying.Cancel();
            }
            else
            {
                PlayMidi(sender, e);
            }
        }

        private async void GoToWebsite(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = @"https://www.goldmidisf2.com/";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }

        private void SetMidiPosition(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            double value = slider.Value;
            bassManager.SetMidiPosition(value);
        }

        private async void ExportMp3(object sender, RoutedEventArgs e)
        {
            int response = await bassManager.ExportMp3();
            Debug.WriteLine(response);
        }

        private async void OpenPlayListScreen(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            if (toggleButton.IsChecked == true)
                await ShowWindow(typeof(PlayList), toggleButton);
            else
                await CloseWindow(PlayList.window);
        }

        private void Rewind(object sender, RoutedEventArgs e)
        {
            bool isPlaying = bassManager.IsPlaying();
            if (isPlaying)
                MainModule.PositionInTime = bassManager.Rewind();
        }

        private void Forward(object sender, RoutedEventArgs e)
        {
            bool isPlaying = bassManager.IsPlaying();
            if (isPlaying)
                MainModule.PositionInTime = bassManager.Forward();
        }

        private void SaveMidiFile(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Debug.WriteLine(MainModule.GetMidiFile().Name);
        }
    }
}
