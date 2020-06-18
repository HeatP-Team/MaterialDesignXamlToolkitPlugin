using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;

namespace XamlToolkitPlugin
{
    public class GitHubDialogViewModel
    {
        private bool IsDownloaded { get; }

        public GitHubDialogViewModel()
        {
            //TODO Check file existing in IsDownloaded from App config
        }

        private void Run(string filePath)
        {
            var toolKit = new Process()
            {
                StartInfo = {FileName = filePath},
                EnableRaisingEvents = true
            };

            toolKit.Start();

            //TODO Add onProcessEnd event
        }
        private void Download()
        {
            //TODO GitHub clone to Config Location
        }

        public ICommand RunCommand
            => new DelegateCommand(obj => Run(obj as string), 
                obj => ((string)obj).Any());
        public ICommand DownloadCommand
            => new DelegateCommand(Download);
    }
}
