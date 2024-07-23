using System.Diagnostics;

namespace CSharpSnippets.FormElements
{
    public partial class ListBoxSelectionDemo : Form
    {
        public ListBoxSelectionDemo()
        {
            InitializeComponent();
        }

        private void mylist_DoubleClick(object sender, EventArgs e)
        {
            int index = mylist.SelectedIndex;
            Debug.WriteLine($"{index}");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
