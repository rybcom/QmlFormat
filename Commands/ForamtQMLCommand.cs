using System.Threading.Tasks;

namespace QmlFormat
{

    internal static class Tools
    {
        internal static bool IsQmlFile(string filePath)
        {
            var extension = System.IO.Path.GetExtension(filePath);
            return extension == ".qml";
        }

        internal static async Task RunExternalFormatAsync(string filePath)
        {
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = await OptionsHelper.GetFormatPathAsync();
                process.StartInfo.Arguments = await OptionsHelper.GetArgsAsync(filePath);
                process.Start();

                await VS.StatusBar.ShowMessageAsync($"Qml Format applied on {filePath}");
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowErrorAsync("qmlformat error", ex.Message);
            }
        }
    }

    internal static class OptionsHelper
    {
        public async static Task<string> GetArgsAsync(string filePath)
        {
            var options = await General.GetLiveInstanceAsync();

            var verbose = options.Verbose ? "-V" : "";
            var force = options.Force ? "-f" : "";
            var tabs = options.Tabs ? "-t" : "";
            var indentWidth = $"-w {options.IndentWidth}";
            var normalize = options.Normalize ? "-n" : "";

            string args;
            if (options.QtVersion == QtVersion.Qt5)
                args = $"{verbose} {normalize} {force} -i \"{filePath}\"";
            else if (options.QtVersion == QtVersion.Qt6)
                args = $"{verbose} {normalize} {force} {tabs} {indentWidth} -i \"{filePath}\"";
            else
                args = $"{verbose} {normalize} {force} {tabs} {indentWidth} -i \"{filePath}\"";

            return args;
        }

        public async static Task<string> GetFormatPathAsync()
        {
            var options = await General.GetLiveInstanceAsync();
            return Environment.ExpandEnvironmentVariables(options.QmlFormatPath);
        }
    }

    [Command(PackageIds.FormatQMLCommand)]
    internal sealed class FormatQMLCommand : BaseCommand<FormatQMLCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView != null)
            {
                var filePath = docView.FilePath;
                if (filePath != null)
                {
                    // run qml format only for .qml files
                    if (!Tools.IsQmlFile(filePath))
                    {
                        await VS.StatusBar.ShowMessageAsync(
                            $"QML Format : File {filePath} is not supported. Only .qml files are supported.");
                        return;
                    }

                    // save the file before formatting due to external upcoming changes
                    docView.Document.Save();

                    await Tools.RunExternalFormatAsync(filePath);
                }
            }
        }
    }
}
