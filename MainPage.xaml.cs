using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.WindowManagement.Preview;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

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
        public MainPage()
        {
            this.InitializeComponent();
            
            //ApplicationView.PreferredLaunchViewSize = new Size(800, 174);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(800, 174));
        }

        private void SetSliderValue(object slider, TextBlock text)
        {
            Slider slide = slider as Slider;
            if (slide != null)
            {
                if (slide.Value != 0)
                {
                    text.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    text.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
                text.Text = Convert.ToString(slide.Value);
            }
        }

        public void SetGlobalVolume(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            this.SetSliderValue(sender, GlobalVolText);
        }

        public void SetGlobalPitch(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            this.SetSliderValue(sender, GlobalPitchText);
        }

        public void SetGlobalTempo(object sender, RangeBaseValueChangedEventArgs eventArgs)
        {
            this.SetSliderValue(sender, GlobalTempoText);
        }

        private async void MixerBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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

                appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                

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
                DebugText.Text = systemException.Message;
            }
        }

        private void MixerBtn_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Console.WriteLine("Checked Mixer");
        }
    }
}
