﻿using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GoldMidiPlayer
{
    public sealed partial class MixerPage : Page
    {
        private MainPage mainPage;
        private ToggleButton currentTbChannelProgram;
        public static AppWindow window;

        public MixerPage()
        {
            this.InitializeComponent();
            Loaded += MixerPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                mainPage = e.Parameter as MainPage;
                // appWindow = mainPage.appWindow as AppWindow;
            }
            base.OnNavigatedTo(e);
        }

        private void MixerPage_Loaded(object sender, RoutedEventArgs e)
        {
            window = MainPage.appWindows[this.UIContext];
            //RefreshMixerScreen(mainPage.GetMainModule().GetMidiFile());
        }

        public void RefreshMixerScreen(MidiFile midiFile)
        {
            Slider[] sliders = new Slider[16] { VolCh0, VolCh1, VolCh2, VolCh3, VolCh4, VolCh5, VolCh6, VolCh7, VolCh8, VolCh9, VolCh10, VolCh11, VolCh12, VolCh13, VolCh14, VolCh15 };
            for (int i = 0; i < midiFile.ChannelsVolume.Count; i++)
            {
                Debug.WriteLine("Valor volumen {0}", midiFile.ChannelsVolume.ElementAt(i));
                Slider currentSlider = sliders.ElementAt(i);
                currentSlider.IsEnabled = true;
                currentSlider.Value = midiFile.ChannelsVolume.ElementAt(i);
            }
            
            VolCh0.IsEnabled = true;
            VolCh0Text.Text = midiFile.ChannelsVolume.ElementAtOrDefault<int>(0).ToString();
            PanCh0.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(0));
            ReverbCh0.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(0));
            ChorusCh0.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(0));
            VolCh1.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(1);
            PanCh1.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(1));
            ReverbCh1.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(1));
            ChorusCh1.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(1));
            VolCh2.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(2);
            PanCh2.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(2));
            ReverbCh2.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(2));
            ChorusCh2.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(2));
            VolCh3.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(3);
            PanCh3.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(3));
            ReverbCh3.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(3));
            ChorusCh3.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(3));
            VolCh4.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(4);
            PanCh4.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(4));
            ReverbCh4.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(4));
            ChorusCh4.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(4));
            VolCh5.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(5);
            PanCh5.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(5));
            ReverbCh5.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(5));
            ChorusCh5.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(5));
            VolCh6.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(6);
            PanCh6.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(6));
            ReverbCh6.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(6));
            ChorusCh6.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(6));
            VolCh7.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(7);
            PanCh7.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(7));
            ReverbCh7.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(7));
            ChorusCh7.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(7));
            VolCh8.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(8);
            PanCh8.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(8));
            ReverbCh8.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(8));
            ChorusCh8.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(8));
            VolCh9.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(9);
            PanCh9.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(9));
            ReverbCh9.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(9));
            ChorusCh9.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(9));
            VolCh10.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(10);
            PanCh10.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(10));
            ReverbCh10.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(10));
            ChorusCh10.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(10));
            VolCh11.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(11);
            PanCh11.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(11));
            ReverbCh11.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(11));
            ChorusCh11.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(11));
            VolCh12.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(12);
            PanCh12.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(12));
            ReverbCh12.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(12));
            ChorusCh12.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(12));
            VolCh13.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(13);
            PanCh13.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(13));
            ReverbCh13.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(13));
            ChorusCh13.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(13));
            VolCh14.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(14);
            PanCh14.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(14));
            ReverbCh14.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(14));
            ChorusCh14.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(14));
            VolCh15.Value = midiFile.ChannelsVolume.ElementAtOrDefault<int>(15);
            PanCh15.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsPan.ElementAtOrDefault<int>(15));
            ReverbCh15.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsReverb.ElementAtOrDefault<int>(15));
            ChorusCh15.Angle = Utility.ValueToRotationKnob(midiFile.ChannelsChorus.ElementAtOrDefault<int>(15));
        }

        private void SetChannelReverb(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            DialControl dialControl = sender as DialControl;
            int channel = GetChannel(dialControl);
            if (mainPage.bassManager != null)
                mainPage.bassManager.SetChannelReverb(channel, dialControl.Angle);
        }

        private void SetChannelChorus(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            DialControl dialControl = sender as DialControl;
            int channel = GetChannel(dialControl);
            if (mainPage.bassManager != null)
                mainPage.bassManager.SetChannelChorus(channel, dialControl.Angle);
        }

        private int GetChannel(Control control)
        {
            string pattern = @"\d{1,}$";
            string name = control.Name;
            Match m = Regex.Match(name, pattern);
            return Int16.Parse(m.Value);
        }

        private void SetChannelPan(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            DialControl dialControl = sender as DialControl;
            int channel = GetChannel(dialControl);
            if (mainPage.bassManager != null)
                mainPage.bassManager.SetChannelPan(channel, dialControl.Angle);
        }

        private void SetChannelVolume(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            int channel = GetChannel(slider);
            bool enabled = false;
            if (mainPage.CurrentFile != null)
            {
                if (channel < mainPage.CurrentFile.ChannelsVolume.Count)
                {
                    mainPage.CurrentFile.ChannelsVolume[channel] = (int)slider.Value;
                    if (mainPage.bassManager != null)
                        mainPage.bassManager.SetChannelVolume(channel, slider.Value);
                    enabled = true;
                }
            }
            if (!enabled)
            {
                slider.Value = 0;
                slider.IsEnabled = false;
            }

        }

        private void SoloChannel(object sender, RoutedEventArgs e)
        {
            bool?[] channelsStatus = new bool?[16]
            {
                SoloCh0.IsChecked,
                SoloCh1.IsChecked,
                SoloCh2.IsChecked,
                SoloCh3.IsChecked,
                SoloCh4.IsChecked,
                SoloCh5.IsChecked,
                SoloCh6.IsChecked,
                SoloCh7.IsChecked,
                SoloCh8.IsChecked,
                SoloCh9.IsChecked,
                SoloCh10.IsChecked,
                SoloCh11.IsChecked,
                SoloCh12.IsChecked,
                SoloCh13.IsChecked,
                SoloCh14.IsChecked,
                SoloCh15.IsChecked
            };
            ToggleButton tb = sender as ToggleButton;
            int channel = GetChannel(tb);
            if (mainPage.CurrentFile != null)
                mainPage.bassManager.SetChannelSolo(channel, channelsStatus);
        }

        private async void ToggleProgramControl(object sender, RoutedEventArgs e)
        {
            ToggleButton[] programButtons = new ToggleButton[16]
            {
                ProgramCh0,
                ProgramCh1,
                ProgramCh2,
                ProgramCh3,
                ProgramCh4,
                ProgramCh5,
                ProgramCh6,
                ProgramCh7,
                ProgramCh8,
                ProgramCh9,
                ProgramCh10,
                ProgramCh11,
                ProgramCh12,
                ProgramCh13,
                ProgramCh14,
                ProgramCh15,
            };
            ToggleButton tb = sender as ToggleButton;
            int channel = GetChannel(tb);
            currentTbChannelProgram = tb;
            bool openWindow = false;
            Debug.WriteLine("Start ToggleProgrammControl");
            for (int i = 0; i < programButtons.Length; i++)
            {
                if (i != channel && programButtons[i].IsChecked == true)
                {
                    programButtons[i].IsChecked = false;
                    openWindow = true;
                }
            }
            Debug.WriteLine("ProgramButtons checked. [ Open Window: {0} ], [ tb.IsCheked: {1} ]", openWindow, tb.IsChecked);
            if (openWindow || tb.IsChecked == false)
            {
                Debug.WriteLine("Closing WINDOW");
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                {
                    await mainPage.CloseWindow(InstrumentPage.window);
                });
                currentTbChannelProgram = null;
            }

            if (tb.IsChecked == true)
            {
                Debug.WriteLine("Opening WINDOW");
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    Debug.WriteLine("MainPage?: {0}", mainPage);
                    Debug.WriteLine(typeof(InstrumentPage));
                    await mainPage.ShowWindow(typeof(InstrumentPage), tb);
                    Debug.WriteLine("XP");
                });
            }
        }

        private void SetChannelProgram(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            int channel = GetChannel(currentTbChannelProgram);
            mainPage.bassManager.SetChannelProgram(channel, lv.SelectedIndex);
            var ProgramsList = lv.ItemsSource as List<ProgramsModel>;
            currentTbChannelProgram.Content = ProgramsList.ElementAt(lv.SelectedIndex).Name;
        }
    }
}
