using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace FilenameExtractorVSIX.Commands
{
    internal sealed class CopyFilenameWithoutExtensionCommand
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("e7c8a1d2-5f3b-4a9e-8c6d-2b1f0e9d8c7a");

        private readonly AsyncPackage package;
        private static ResourceManager resourceManager;

        private CopyFilenameWithoutExtensionCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            // Initialize ResourceManager for localization
            resourceManager = new ResourceManager("FilenameExtractorVSIX.VSPackage", typeof(CopyFilenameWithoutExtensionCommand).Assembly);

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static CopyFilenameWithoutExtensionCommand Instance { get; private set; }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => this.package;

        /// <summary>
        /// Gets a localized string from resources.
        /// </summary>
        private static string GetLocalizedString(string name)
        {
            try
            {
                return resourceManager?.GetString(name, CultureInfo.CurrentUICulture) ?? name;
            }
            catch
            {
                return name;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CopyFilenameWithoutExtensionCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                EnvDTE80.DTE2 dte = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                string fullPath = null;

                // Try to get the selected file from Solution Explorer
                if (dte?.SelectedItems != null && dte.SelectedItems.Count > 0)
                {
                    foreach (EnvDTE.SelectedItem selectedItem in dte.SelectedItems)
                    {
                        if (selectedItem.ProjectItem != null)
                        {
                            fullPath = selectedItem.ProjectItem.FileNames[1];
                            break;
                        }
                    }
                }

                // If no selection in Explorer, use the active document
                if (string.IsNullOrEmpty(fullPath) && dte?.ActiveDocument != null)
                {
                    fullPath = dte.ActiveDocument.FullName;
                }

                if (string.IsNullOrEmpty(fullPath))
                {
                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        GetLocalizedString("ErrorNoFileSelected"),
                        GetLocalizedString("ErrorTitle"),
                        OLEMSGICON.OLEMSGICON_WARNING,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return;
                }

                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);

                Clipboard.SetText(filenameWithoutExtension);

                dte.StatusBar.Text = string.Format(GetLocalizedString("StatusCopied"), filenameWithoutExtension);
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    string.Format(GetLocalizedString("ErrorCopyFailed"), ex.Message),
                    GetLocalizedString("ErrorTitle"),
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }
    }
}
