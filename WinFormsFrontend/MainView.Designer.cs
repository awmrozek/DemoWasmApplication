namespace WinFormsFrontend
{
    partial class MainView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            toolStripContainer1 = new ToolStripContainer();
            toolStrip1 = new ToolStrip();
            tsOpenFile = new ToolStripButton();
            cbPickRecentFile = new ToolStripComboBox();
            tSeparator = new ToolStripSeparator();
            cbPickFilter = new ToolStripComboBox();
            tbApply = new ToolStripButton();
            tbClear = new ToolStripButton();
            imageList1 = new ImageList(components);
            pictureBox = new PictureBox();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(toolStrip1);
            toolStripContainer1.ContentPanel.Margin = new Padding(2);
            toolStripContainer1.ContentPanel.Size = new Size(959, 45);
            toolStripContainer1.Location = new Point(9, 9);
            toolStripContainer1.Margin = new Padding(2);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(959, 70);
            toolStripContainer1.TabIndex = 2;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.None;
            toolStrip1.ImageScalingSize = new Size(40, 40);
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsOpenFile, cbPickRecentFile, tSeparator, cbPickFilter, tbApply, tbClear });
            toolStrip1.Location = new Point(-2, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(809, 50);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "Open file...";
            toolStrip1.ItemClicked += toolStrip1_ItemClicked;
            // 
            // tsOpenFile
            // 
            tsOpenFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsOpenFile.Image = (Image)resources.GetObject("tsOpenFile.Image");
            tsOpenFile.ImageTransparentColor = Color.Magenta;
            tsOpenFile.Name = "tsOpenFile";
            tsOpenFile.Size = new Size(46, 44);
            tsOpenFile.Text = "toolStripButton1";
            tsOpenFile.Click += tsOpenFile_Click;
            // 
            // cbPickRecentFile
            // 
            cbPickRecentFile.Name = "cbPickRecentFile";
            cbPickRecentFile.Size = new Size(300, 50);
            cbPickRecentFile.DropDownClosed += cbPickRecentFile_DropDownClosed;
            // 
            // tSeparator
            // 
            tSeparator.Name = "tSeparator";
            tSeparator.Size = new Size(6, 50);
            // 
            // cbPickFilter
            // 
            cbPickFilter.Name = "cbPickFilter";
            cbPickFilter.Size = new Size(200, 50);
            // 
            // tbApply
            // 
            tbApply.Image = (Image)resources.GetObject("tbApply.Image");
            tbApply.ImageTransparentColor = Color.Magenta;
            tbApply.Name = "tbApply";
            tbApply.Size = new Size(119, 44);
            tbApply.Text = "Apply";
            tbApply.Click += tbApply_Click;
            // 
            // tbClear
            // 
            tbClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbClear.Image = (Image)resources.GetObject("tbClear.Image");
            tbClear.ImageTransparentColor = Color.Magenta;
            tbClear.Name = "tbClear";
            tbClear.Size = new Size(46, 44);
            tbClear.Text = "Clear";
            tbClear.Click += tbClear_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.BackColor = SystemColors.ControlDarkDark;
            pictureBox.Location = new Point(9, 79);
            pictureBox.Margin = new Padding(2);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(957, 555);
            pictureBox.TabIndex = 4;
            pictureBox.TabStop = false;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(977, 645);
            Controls.Add(pictureBox);
            Controls.Add(toolStripContainer1);
            Margin = new Padding(2);
            Name = "MainView";
            Text = "Image Transformer Demo";
            Load += MainView_Load;
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.ContentPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton tsOpenFile;
        private ToolStripButton tbApply;
        private ImageList imageList1;
        private ToolStripComboBox cbPickRecentFile;
        private ToolStripSeparator tSeparator;
        private ToolStripComboBox cbPickFilter;
        private ToolStripButton tbClear;
        private PictureBox pictureBox;
    }
}
