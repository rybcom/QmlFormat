using System.Threading.Tasks;

namespace QmlFormat
{
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
                    try
                    {
                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = await OptionsHelper.GetFormatPathAsync();
                        process.StartInfo.Arguments = await OptionsHelper.GetArgsAsync(filePath);
                        process.Start();
                    }
                    catch (Exception ex)
                    {
                        await VS.MessageBox.ShowErrorAsync("qmlformat error", ex.Message);
                    }

                }
            }
        }
    }
}
