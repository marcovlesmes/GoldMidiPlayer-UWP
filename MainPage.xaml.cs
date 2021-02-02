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
        public BassManager bassManager = new BassManager();
        public MidiFile CurrentFile;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task<bool> ShowWindow(Type type, ToggleButton toggleButton)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame frame = new Frame();
            frame.Navigate(type, this);
            ElementCompositionPreview.SetAppWindowContent(appWindow, frame);
            appWindows.Add(frame.UIContext, appWindow);
            appWindow.Closed += delegate
                {
                    MainPage.appWindows.Remove(frame.UIContext);
                    frame.Content = null;
                    appWindow = null;
                    toggleButton.IsChecked = false;
                };
            try
            {
                await appWindow.TryShowAsync();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            return true;
        }

        private async Task<bool> CloseWindow(AppWindow window)
        {
            await window.CloseAsync();
            return true;
        }

        private async void RefreshMainScreen(MidiFile midiFile)
        {
            if (midiFile != null)
            {
                GlobalVolumeSlider.Value = midiFile.GlobalVolume;
                GlobalTempoSlider.Value = midiFile.GlobalTempo;
                GlobalPitchSlider.Value = 0;
                MidiPositionSlider.Value = 0;
                MidiNameText.Text = midiFile.Name;
                MidiTimeSignatureText.Text = midiFile.TimeSignatureAsString;
                SolidColorBrush white = new SolidColorBrush(Colors.White);
                MidiNameText.Foreground = white;
                MidiCurrentTimeText.Foreground = white;
                MidiTimeSignatureText.Foreground = white;
                GlobalVolText.Foreground = white;
                GlobalPitchText.Foreground = white;
                GlobalTempoText.Foreground = white;
                MidiLenghtText.Text = Utility.SecondsToTime(midiFile.Lenght);
                if (MixerPage.window != null)
                {
                    await CloseWindow(MixerPage.window);
                    await ShowWindow(typeof(MixerPage), MixerBtn);
                }

                //Debug.Write("MixerIsOpen: ", (MixerFrame.Content != null).ToString());
            }
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
            
            if (IsPlaying)
            {
                TimeSpan interval = TimeSpan.FromSeconds(1);
                RefreshCurrentPlaying = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {
                    // Update Midi position
                    Dispatcher.RunAsync(CoreDispatcherPriority.High,
                        () =>
                        {
                            // Update UI
                            if (bassManager.IsPlaying())
                            {
                                double Position = bassManager.GetMidiPosition();
                                float MidiLenght = Utility.TimeToSeconds(MidiLenghtText.Text);
                                double NewPosition = Utility.Linear((float)Position, 0f, MidiLenght, 0f, 100f);
                                MidiPositionSlider.Value = Position;
                            }
                        });
                }, interval);
            }
        }

        private async void OpenMidi(object sender, RoutedEventArgs e)
        {
            try
            {
                MidiFile midiFile = await bassManager.LoadFile();
                CurrentFile = midiFile;
                RefreshMainScreen(midiFile);
            }
            catch (Exception exep)
            {
                Debug.WriteLine(exep.Message);
            }
        }

        private async void OpenFont(object sender, RoutedEventArgs e)
        {
            await bassManager.LoadFonts();
        }

        private void StopMidi(object sender, RoutedEventArgs e)
        {
            bool IsStoped = bassManager.Stop();
            if (IsStoped && RefreshCurrentPlaying != null)
            {
                RefreshCurrentPlaying.Cancel();
                MidiPositionSlider.Value = 0;
            }
        }

        private void PauseMidi(object sender, RoutedEventArgs e)
        {
            bool isPlaying = bassManager.IsPlaying();
            if (isPlaying)
            {
                bassManager.Pause();
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
            if (CurrentFile != null)
            {
                int response = await bassManager.ExportMp3();
                Debug.WriteLine(response);
            }
        }
    }
}
