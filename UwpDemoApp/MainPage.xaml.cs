using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpDemoApp
{
    public sealed partial class MainPage : Page
    {
        public AppCore ApplicationCore;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e?.Parameter is AppCore appCore)
            {
                ApplicationCore = appCore;
            }
        }

        public async void OnOpenClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await ApplicationCore.OpenAsync(currentPasswordText.Text).ConfigureAwait(true);
                PrintInfo("DB opened");
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }
        }

        public async void OnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await ApplicationCore.ChangePasswordAsync(newPasswordText.Text).ConfigureAwait(true);
                PrintInfo("DB rebult. Password changed");
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }
        }

        public async void OnWriteTextClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await ApplicationCore.WriteText(textToWrite.Text);
                PrintInfo("Text write to DB succeeded");
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }
        }

        public async void OnReadText(object sender, RoutedEventArgs e)
        {
            try
            {
                var textRecords = await ApplicationCore.ReadText().ConfigureAwait(true);
                string text = string.Join('\n', textRecords);
                PrintInfo($"Text read from DB\n{text}");
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }
        }

        public async void OnDeleteDb(object sender, RoutedEventArgs e)
        {
            try
            {
                await ApplicationCore.DeleteDbFiles().ConfigureAwait(true);
                PrintInfo($"DB data file deleted");
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }
        }

        private void PrintInfo(string infoText)
        {
            infoBox.Text = $"Info: {infoText}";
        }

        private void PrintError(Exception exception)
        {
            infoBox.Text = $"Error: {exception.Message}";
        }
    }
}
