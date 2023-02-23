using System.ComponentModel;
using System.Configuration;

namespace CSharpSnippets.FormElements
{
    public partial class MainForm : Form
    {
        private readonly FormSettings Settings;

        public MainForm()
        {
            InitializeComponent();
            // restore previous size and position of the application window
            Settings = new FormSettings();
            StartPosition = FormStartPosition.Manual;
            Location = Settings.FormLocation;
            Size = Settings.FormSize;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.FormLocation = Location;
            Settings.FormSize = Size;
            Settings.Save();
            base.OnClosing(e);
        }

        private sealed class FormSettings : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            [DefaultSettingValue("300, 100")]
            public Point FormLocation
            {
                get { return (Point) this[nameof(FormLocation)]; }
                set { this[nameof(FormLocation)] = value; }
            }
            [UserScopedSetting()]
            [DefaultSettingValue("800, 500")]
            public Size FormSize
            {
                get { return (Size) this[nameof(FormSize)]; }
                set { this[nameof(FormSize)] = value; }
            }
        }
    }
}

