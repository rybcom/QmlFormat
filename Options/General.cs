using System.ComponentModel;
using System.Runtime.InteropServices;

namespace QmlFormat
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "QmlFormat", "General", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public enum QtVersion
    {
        Qt5,
        Qt6
    }

    public class General : BaseOptionModel<General>
    {
        [Category("qmlformat")]
        [DisplayName("Qt version")]
        [Description("version of installed qt libraries")]
        [DefaultValue(QtVersion.Qt6)]
        public QtVersion QtVersion { get; set; }

        [Category("qmlformat")]
        [DisplayName("qmlformat.exe path")]
        [Description("Full path to qmlformat.exe")]
        [DefaultValue("")]
        public string QmlFormatPath { get; set; }

        [Category("option")]
        [DisplayName("verbose")]
        [Description("--verbose option")]
        [DefaultValue(true)]
        public bool Verbose { get; set; }

        [Category("option")]
        [DisplayName("force")]
        [Description("--force option")]
        [DefaultValue(true)]
        public bool Force { get; set; }

        [Category("option")]
        [DisplayName("tabs")]
        [Description("--tabs option")]
        [DefaultValue(false)]
        public bool Tabs { get; set; }

        [Category("option")]
        [DisplayName("indent width")]
        [Description("--indent-width option")]
        [DefaultValue(4)]
        public uint IndentWidth { get; set; }

        [Category("option")]
        [DisplayName("normalize/no sort")]
        [Description("--normalize option in Qt6   --no-sort imports in Qt5")]
        [DefaultValue(false)]
        public bool Normalize { get; set; }
    }
}
