using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace XamlToolkitPlugin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <summary>
    /// Interaction logic for GitHubDialogWindowControl.
    /// </summary>
    public partial class GitHubDialogWindowControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDialogWindowControl"/> class.
        /// </summary>
        public GitHubDialogWindowControl()
        {
            InitializeComponent();

            DataContext = new GitHubDialogViewModel();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            var op = new OpenFileDialog
            {
                Title = "Select Xaml Toolkit",
                Filter = "Xaml Toolkit (MaterialDesignDemo.exe)|MaterialDesignDemo.exe"
            };

            if (op.ShowDialog() != true || string.IsNullOrWhiteSpace(op.FileName))
                return;

            FilePath.Text = op.FileName;
            GitHubDialogViewModel.SaveDirectorySettings(GitHubDialogViewModel.GetDirectoryNameFromExe(op.FileName));
        }

        private void DownloadOnClick(object sender, RoutedEventArgs e)
        {
            var appSettings = new AppSettings();
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) 
                    return;

                GitHubDialogViewModel.Download(Path.Combine(fbd.SelectedPath,"MaterialDesignInXamlToolkit"));
                GitHubDialogViewModel.BuildProject(fbd.SelectedPath);

                FilePath.Text = Path.Combine(fbd.SelectedPath, appSettings.ExePath);
                GitHubDialogViewModel.SaveDirectorySettings(fbd.SelectedPath);
            }
        }

        private void GitHubDialogWindowControl_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if ((bool) args.NewValue == false)
                return;

            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists("XamlToolkit"))
                return;

            var directory = userSettingsStore.GetString("XamlToolkit", "Directory");
            if (string.IsNullOrWhiteSpace(directory))
                return;

            try
            {
                GitHubDialogViewModel.UpdateDirectory(directory);
                GitHubDialogViewModel.Run(Path.Combine(directory, AppSettings.Default.ExePath));
                Window.GetWindow(this).Close();
            }
            catch (Exception)
            {
                GitHubDialogViewModel.SaveDirectorySettings("");
            }
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}