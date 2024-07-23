using System.Diagnostics;

namespace CSharpSnippets.FormElements
{
    public partial class ListBoxSelectionDemo : MainForm
    {
        // constructor of parent class is called implicitly
        public ListBoxSelectionDemo()
        {
            InitializeComponent();
        }

        private void mylist_DoubleClick(object sender, EventArgs e)
        {
            int index = mylist.SelectedIndex;
            Debug.WriteLine($"{index}");
        }

        // -- close on Esc is inherited from parent
        /*
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        */
    }
}
