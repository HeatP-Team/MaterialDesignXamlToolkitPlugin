using Microsoft.Win32;

namespace XamlToolkitPlugin
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for GitHubDialogWindowControl.
    /// </summary>
    public partial class GitHubDialogWindowControl : UserControl
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

            if (op.ShowDialog() == true)
                FilePath.Text = op.FileName;
        }
    }
}