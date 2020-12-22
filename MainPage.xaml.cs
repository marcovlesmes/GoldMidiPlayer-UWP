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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoldMidiPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        private AppWindow appWindow;
        private Frame MixerFrame = new Frame();
        private BassManager bassManager = new BassManager();
        public MainPage()
        {
            this.InitializeComponent();
            
            //ApplicationView.PreferredLaunchViewSize = new Size(800, 174);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(800, 174));
        }

        private void RefreshMainScreen(MidiFile midiFile)
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
            if (appWindow == null)
            {
                // Create Window
                appWindow = await AppWindow.TryCreateAsync();
                appWindow.Closed += delegate { appWindow = null; MixerFrame.Content = null; MixerBtn.IsChecked = false; };
                WindowManagementPreview.SetPreferredMinSize(appWindow, new Size(800, 250));
                // appWindow.RequestSize(new Size(800, 250));
                MixerFrame.Navigate(typeof(MixerPage));
                ElementCompositionPreview.SetAppWindowContent(appWindow, MixerFrame);

                Color darkBlue = Color.FromArgb(255, 5, 13, 16);
                appWindow.TitleBar.ButtonBackgroundColor = darkBlue;
                appWindow.TitleBar.BackgroundColor = darkBlue;

            }
            else
            {
                MixerBtn.IsChecked = true;
            }
            try
            {
                await appWindow.TryShowAsync();
            }
            catch (Exception systemException)
            {
                Debug.WriteLine(systemException.Message);
            }
        }

        private void MixerBtn_Checked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Checked Mixer");
        }

        private void PlayMidi(object sender, RoutedEventArgs e)
        {
            bassManager.Play();
        }

        private async void OpenMidi(object sender, RoutedEventArgs e)
        {
            MidiFile midiFile = await bassManager.LoadFile();
            RefreshMainScreen(midiFile);
        }

        private async void OpenFont(object sender, RoutedEventArgs e)
        {
            await bassManager.LoadFonts();
        }

        private void StopMidi(object sender, RoutedEventArgs e)
        {
            bassManager.Stop();
        }

        private void PauseMidi(object sender, RoutedEventArgs e)
        {
            bassManager.Pause();
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
    }
}
