using System.Diagnostics;
using System.Windows.Forms;

namespace CSharpSnippets.FormElements
{
    public partial class MenuAndContextMenuDemo : Form
    {
        public MenuAndContextMenuDemo()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = new Point(500, 100);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"test1");
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"test2");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"settings");
        }

        private void TestFormMenuStrip_Load(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine($"context menu opening");
        }

        private void startMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"start monitor");
        }

        private void pauseMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"pause monitor");
        }

        private void closeMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"close monitor");
        }
    }
}
