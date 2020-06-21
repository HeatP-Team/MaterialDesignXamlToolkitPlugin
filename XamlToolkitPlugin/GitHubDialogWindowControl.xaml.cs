using System;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace XamlToolkitPlugin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Configuration;

    /// <summary>
    /// Interaction logic for GitHubDialogWindowControl.
    /// </summary>
    public partial class GitHubDialogWindowControl : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDialogWindowControl"/> class.
        /// </summary>
        public GitHubDialogWindowControl()
        {
            InitializeComponent();
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (userSettingsStore.CollectionExists("XamlToolkit"))
            {
                var directory = userSettingsStore.GetString("XamlToolkit", "Directory");
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    try
                    {
                        GitHubDialogViewModel.Run(Path.Combine(directory, AppSettings.Default.ExePath));
                    }
                    catch (Exception)
                    {
                        GitHubDialogViewModel.SaveDirectorySettings("");
                    }
                }
            }

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

            if (op.ShowDialog() == true && !string.IsNullOrWhiteSpace(op.FileName))
            {
                FilePath.Text = op.FileName;

                GitHubDialogViewModel.SaveDirectorySettings(GitHubDialogViewModel.GetDirectoryNameFromExe(op.FileName));
            }
        }

        private void DowloadOnClick(object sender, RoutedEventArgs e)
        {
            var appSettings = new AppSettings();
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    GitHubDialogViewModel.Download(Path.Combine(fbd.SelectedPath,"MaterialDesignInXamlToolkit"));
                    GitHubDialogViewModel.BuildProject(fbd.SelectedPath);
                    FilePath.Text = Path.Combine(fbd.SelectedPath, appSettings.ExePath);

                    GitHubDialogViewModel.SaveDirectorySettings(fbd.SelectedPath);
                }
            }
        }
    }
}