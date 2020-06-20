using System;
using System.Configuration;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Settings;

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
        public static void Download(string directoryPath)
        {
            var appSettings = new AppSettings();
            var process = Process.Start("git", $"clone {appSettings.GitPath} {directoryPath}");
            process?.WaitForExit();
        }

        public static void CreateExecutable(string directoryPath)
        {
            var appSettings = new AppSettings();
            var process = Process.Start("dotnet", $"build {directoryPath}{appSettings.SlnPath}");
            process?.WaitForExit();
        }

        public ICommand RunCommand
            => new DelegateCommand(obj => Run(obj as string), 
                obj => ((string)obj).Any());

        public static void SaveDirectorySettings(string directoryPath)
        {
            var appSettings = new AppSettings();
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = @"C:\Users\Admin\Documents\Programming\CSharp\XamlToolkitPlugin\directory.config"
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            try
            {
                config.AppSettings.Settings["DirectoryPath"].Value = directoryPath;
            }
            catch (Exception)
            {
                config.AppSettings.Settings.Add("DirectoryPath", directoryPath);
            }

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
