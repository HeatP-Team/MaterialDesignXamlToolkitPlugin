﻿using System;
using System.Configuration;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

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
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists("XamlToolkit"))
            {
                userSettingsStore.CreateCollection("XamlToolkit");
                userSettingsStore.SetString("XamlToolkit", "Directory", directoryPath);
            }
            else
            {
                userSettingsStore.SetString("XamlToolkit", "Directory", directoryPath);
            }
        }
    }
}
