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
    public sealed partial class InstrumentPage : Page
    {
        public InstrumentPage()
        {
            this.InitializeComponent();
            Loaded += InstrumentPage_Loaded;
        }

        public static AppWindow window { get; internal set; }

        private void InstrumentPage_Loaded(object sender, RoutedEventArgs e)
        {
            window = MainPage.appWindows[this.UIContext];
        }

        private void ChangeCategory(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            int selectedIndexCategory = listView.SelectedIndex;
            MainPageData dataContext = listView.DataContext as MainPageData;
            dataContext.SetActivePrograms(selectedIndexCategory);
        }

        private void SetChannelProgram(object sender, SelectionChangedEventArgs e)
        {
            ListView listview = sender as ListView;
            int selectedIndexProgram = listview.SelectedIndex;
            if (selectedIndexProgram != -1)
            {
                MainPageData dataContext = listview.DataContext as MainPageData;
                int program = dataContext.ActiveCategory.StartIndex + selectedIndexProgram;
                Debug.WriteLine("index: {0} - Program: {1}", program, dataContext.Programs.ElementAt(program));

            }
            Debug.WriteLine(selectedIndexProgram);
            Debug.WriteLine("Set Program");
        }
    }
}
