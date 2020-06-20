using System.Windows.Forms;
using EnvDTE;
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
            DataContext = new GitHubDialogViewModel();
            FilePath.Text = ConfigurationManager.AppSettings["DirectoryPath"];
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

                GitHubDialogViewModel.SaveDirectorySettings(op.FileName);
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
                    GitHubDialogViewModel.Download(fbd.SelectedPath + "\\MaterialDesignInXamlToolkit");
                    GitHubDialogViewModel.CreateExecutable(fbd.SelectedPath);
                    FilePath.Text = fbd.SelectedPath + appSettings.ExePath;

                    GitHubDialogViewModel.SaveDirectorySettings(fbd.SelectedPath);
                }
            }
        }
    }
}