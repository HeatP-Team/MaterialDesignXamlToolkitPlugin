using System.IO;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;

namespace XamlToolkitPlugin
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("caa108c7-ceab-4aad-99a7-1a8ecea92c1d")]
    public class GitHubDialogWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDialogWindow"/> class.
        /// </summary>
        public GitHubDialogWindow() : base(null)
        {
            Caption = "Material Design Xaml Toolkit";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new GitHubDialogWindowControl();
        }
    }
}
