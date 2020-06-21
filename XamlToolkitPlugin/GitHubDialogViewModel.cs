using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace XamlToolkitPlugin
{
    public class GitHubDialogViewModel
    {
        public static void Run(string filePath)
        {
            Process.Start(filePath);
        }

        public static void Download(string directoryPath)
        {
            var appSettings = new AppSettings();
            var process = Process.Start("git", $"clone {appSettings.GitPath} {directoryPath}");
            process?.WaitForExit();
        }

        public static void BuildProject(string directoryPath)
        {
            var appSettings = new AppSettings();
            Process.Start("dotnet", $"build {Path.Combine(directoryPath, appSettings.SlnPath)}")?.WaitForExit();
            Process.Start(new ProcessStartInfo("dotnet", "build-server shutdown") {CreateNoWindow = true})?.WaitForExit();
        }

        public ICommand RunCommand
            => new DelegateCommand(obj => Run(obj as string),
                obj => ((string) obj).Any());

        public static void SaveDirectorySettings(string directoryPath)
        {
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore =
                settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists("XamlToolkit"))
                userSettingsStore.CreateCollection("XamlToolkit");

            userSettingsStore.SetString("XamlToolkit", "Directory", directoryPath);
        }

        public static string GetDirectoryNameFromExe(string exePath)
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(exePath))))));
        }

        public static void UpdateDirectory(string directory)
        {
            var gitDirectory = Path.Combine(directory, "MaterialDesignInXamlToolkit");

            var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/C cd {gitDirectory} && git pull") {CreateNoWindow = false});

            process?.WaitForExit();

            if(process?.ExitCode != 0)
                BuildProject(directory);
        }
    }
}
