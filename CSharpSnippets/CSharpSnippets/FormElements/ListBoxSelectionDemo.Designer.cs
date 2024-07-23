using System.Windows.Forms;

namespace CSharpSnippets.FormElements
{
    partial class ListBoxSelectionDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            mylist = new ListBox();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            //
            // panel1
            //
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(mylist);
            panel1.Location = new Point(15, 45);
            panel1.Name = "panel1";
            panel1.Size = new Size(570, 285);
            panel1.TabIndex = 2;
            //
            // mylist
            //
            mylist.Dock = DockStyle.Fill;
            mylist.FormattingEnabled = true;
            mylist.ItemHeight = 15;
            mylist.Items.AddRange(new object[] {
                "market 1",
                "market 2",
                "market 3",
                "market 4"});
            mylist.Location = new Point(0, 0);
            mylist.Name = "mylist";
            mylist.Size = new Size(570, 285);
            mylist.TabIndex = 1;
            mylist.DoubleClick += mylist_DoubleClick;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(136, 15);
            label1.TabIndex = 0;
            label1.Text = "Double click on market to show index ...";
            //
            // TestFormListBox
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 340);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "TestFormListBox";
            Text = "Select Market";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Panel panel1;
        private Label label1;
        private ListBox mylist;
    }
}
