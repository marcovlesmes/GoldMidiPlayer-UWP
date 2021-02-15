using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GoldMidiPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayList : Page
    {
        public static AppWindow window;
        private MainPage mainPage;

        public PlayList()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                mainPage = e.Parameter as MainPage;
                // appWindow = mainPage.appWindow as AppWindow;
                if (mainPage.CurrentFile != null)
                    //RefreshMixerScreen(mainPage.CurrentFile);
                    Debug.WriteLine("Current file not null in Playlist page OnNavigatedTo");
            }
            base.OnNavigatedTo(e);
        }

        private void NewPlaylist(object sender, RoutedEventArgs e)
        {
            InputText.Focus(FocusState.Pointer);
            this.ToggleWriteState();
        }

        private void AddPlaylist(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MainPageData dataContext = button.DataContext as MainPageData;
            string playlistName = InputText.Text;

            if (!Utility.IsStringEmpty(playlistName))
            {
                Debug.WriteLine("Adding playlist");
                PlaylistModel playlist = new PlaylistModel(playlistName, new List<MidiFile>());
                dataContext.SetPlaylist(playlist);
                Debug.WriteLine("Actualizando dataContext");
                this.DataContext = dataContext;
            }
            this.ToggleWriteState();
            InputText.Text = string.Empty;
        }

        private void EditPlaylist(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MainPageData dataContext = button.DataContext as MainPageData;
        }

        private void DeletePlaylist(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MainPageData dataContext = button.DataContext as MainPageData;
            var selectedItem = PlaylistListView.SelectedItem;
            if (selectedItem != null)
            {
                PlaylistModel playlist = dataContext.GetPlaylist(selectedItem.ToString());
                if (playlist != null)
                    dataContext.RemovePlayList(playlist);

            }
        }

        private void ToggleWriteState()
        {
            ConfirmNewButton.IsEnabled = ConfirmNewButton.IsEnabled ? false : true;
            CancelButton.IsEnabled = CancelButton.IsEnabled ? false : true;
            InputText.IsEnabled = InputText.IsEnabled ? false : true;
            NewButton.IsEnabled = NewButton.IsEnabled ? false : true;
            RenameButton.IsEnabled = RenameButton.IsEnabled ? false : true;
            EraseButton.IsEnabled = EraseButton.IsEnabled ? false : true;
            PlaylistListView.IsEnabled = PlaylistListView.IsEnabled ? false : true;
        }

        private void SetActivePlaylist(object sender, SelectionChangedEventArgs e)
        {
            ComboBox button = sender as ComboBox;
            MainPageData dataContext = button.DataContext as MainPageData;
            var selectedItem = PlaylistListView.SelectedItem;
            if (selectedItem != null)
            {
                PlaylistModel playlist = dataContext.GetPlaylist(selectedItem.ToString());
                if (playlist != null)
                    dataContext.SetPlaylist(playlist);
            }
            
        }

        private void CancelAction(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MainPageData dataContext = button.DataContext as MainPageData;
            this.ToggleWriteState();
        }
    }
}
