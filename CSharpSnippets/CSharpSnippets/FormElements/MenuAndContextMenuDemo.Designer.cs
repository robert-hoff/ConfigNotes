namespace CSharpSnippets.FormElements
{
    partial class MenuAndContextMenuDemo
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            startToolStripMenuItem = new ToolStripMenuItem();
            test1ToolStripMenuItem = new ToolStripMenuItem();
            test2ToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1 = new ContextMenuStrip(components);
            startMonitorToolStripMenuItem = new ToolStripMenuItem();
            pauseMonitorToolStripMenuItem = new ToolStripMenuItem();
            closeMonitorToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.Items.AddRange(new ToolStripItem[] {
                startToolStripMenuItem,
                settingsToolStripMenuItem
            });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            //
            // startToolStripMenuItem
            //
            startToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                test1ToolStripMenuItem,
                test2ToolStripMenuItem
            });
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(50, 20);
            startToolStripMenuItem.Text = "menu";
            //
            // test1ToolStripMenuItem
            //
            test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            test1ToolStripMenuItem.Size = new Size(180, 22);
            test1ToolStripMenuItem.Text = "test1";
            test1ToolStripMenuItem.Click += test1ToolStripMenuItem_Click;
            //
            // test2ToolStripMenuItem
            //
            test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            test2ToolStripMenuItem.Size = new Size(180, 22);
            test2ToolStripMenuItem.Text = "test2";
            test2ToolStripMenuItem.Click += test2ToolStripMenuItem_Click;
            //
            // settingsToolStripMenuItem
            //
            settingsToolStripMenuItem.AutoToolTip = true;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(60, 20);
            settingsToolStripMenuItem.Text = "settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            //
            // contextMenuStrip1
            //
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
                startMonitorToolStripMenuItem,
                pauseMonitorToolStripMenuItem,
                closeMonitorToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(152, 70);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            //
            // startMonitorToolStripMenuItem
            //
            startMonitorToolStripMenuItem.Name = "startMonitorToolStripMenuItem";
            startMonitorToolStripMenuItem.Size = new Size(151, 22);
            startMonitorToolStripMenuItem.Text = "start monitor";
            startMonitorToolStripMenuItem.Click += startMonitorToolStripMenuItem_Click;
            //
            // pauseMonitorToolStripMenuItem
            //
            pauseMonitorToolStripMenuItem.Name = "pauseMonitorToolStripMenuItem";
            pauseMonitorToolStripMenuItem.Size = new Size(151, 22);
            pauseMonitorToolStripMenuItem.Text = "pause monitor";
            pauseMonitorToolStripMenuItem.Click += pauseMonitorToolStripMenuItem_Click;
            //
            // closeMonitorToolStripMenuItem
            //
            closeMonitorToolStripMenuItem.Name = "closeMonitorToolStripMenuItem";
            closeMonitorToolStripMenuItem.Size = new Size(151, 22);
            closeMonitorToolStripMenuItem.Text = "close monitor";
            closeMonitorToolStripMenuItem.Click += closeMonitorToolStripMenuItem_Click;
            //
            // TestFormMenuStrip
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TestFormMenuStrip";
            Text = "TestFormMenuStrip";
            MouseDown += TestFormMenuStrip_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem test1ToolStripMenuItem;
        private ToolStripMenuItem test2ToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem startMonitorToolStripMenuItem;
        private ToolStripMenuItem pauseMonitorToolStripMenuItem;
        private ToolStripMenuItem closeMonitorToolStripMenuItem;
    }
}
