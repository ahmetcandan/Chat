namespace Chat.ClientApp;

partial class FrmMain
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
        TxtMessages = new TextBox();
        TxtMessage = new TextBox();
        LvClients = new ListView();
        Nick = new ColumnHeader();
        NotifyIcon1 = new NotifyIcon(components);
        NotifyMenu = new ContextMenuStrip(components);
        openToolStripMenuItem = new ToolStripMenuItem();
        setStatusToolStripMenuItem = new ToolStripMenuItem();
        availableToolStripMenuItem = new ToolStripMenuItem();
        busyToolStripMenuItem = new ToolStripMenuItem();
        awayToolStripMenuItem = new ToolStripMenuItem();
        doNotDistrubToolStripMenuItem = new ToolStripMenuItem();
        invisibleToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        BtnSendMessage = new Button();
        NotifyMenu.SuspendLayout();
        SuspendLayout();
        // 
        // TxtMessages
        // 
        TxtMessages.AcceptsReturn = true;
        TxtMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TxtMessages.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtMessages.Location = new Point(14, 14);
        TxtMessages.Margin = new Padding(4, 3, 4, 3);
        TxtMessages.Multiline = true;
        TxtMessages.Name = "TxtMessages";
        TxtMessages.ReadOnly = true;
        TxtMessages.ScrollBars = ScrollBars.Vertical;
        TxtMessages.Size = new Size(616, 514);
        TxtMessages.TabIndex = 4;
        // 
        // TxtMessage
        // 
        TxtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TxtMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtMessage.Location = new Point(14, 535);
        TxtMessage.Margin = new Padding(4, 3, 4, 3);
        TxtMessage.Name = "TxtMessage";
        TxtMessage.Size = new Size(543, 23);
        TxtMessage.TabIndex = 7;
        TxtMessage.KeyPress += TxtMessage_KeyPress;
        // 
        // LvClients
        // 
        LvClients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        LvClients.Columns.AddRange(new ColumnHeader[] { Nick });
        LvClients.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        LvClients.FullRowSelect = true;
        LvClients.GridLines = true;
        LvClients.Location = new Point(638, 14);
        LvClients.Margin = new Padding(4, 3, 4, 3);
        LvClients.Name = "LvClients";
        LvClients.Size = new Size(373, 547);
        LvClients.TabIndex = 9;
        LvClients.UseCompatibleStateImageBehavior = false;
        LvClients.View = View.Details;
        LvClients.DoubleClick += LvClients_DoubleClick;
        // 
        // Nick
        // 
        Nick.Text = "Nick";
        Nick.TextAlign = HorizontalAlignment.Right;
        Nick.Width = 316;
        // 
        // NotifyIcon1
        // 
        NotifyIcon1.ContextMenuStrip = NotifyMenu;
        NotifyIcon1.Icon = (Icon)resources.GetObject("NotifyIcon1.Icon");
        NotifyIcon1.Text = "Chat | Client";
        NotifyIcon1.Visible = true;
        NotifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
        // 
        // NotifyMenu
        // 
        NotifyMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, setStatusToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        NotifyMenu.Name = "notifyMenu";
        NotifyMenu.Size = new Size(181, 98);
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.Size = new Size(180, 22);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
        // 
        // setStatusToolStripMenuItem
        // 
        setStatusToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { availableToolStripMenuItem, busyToolStripMenuItem, awayToolStripMenuItem, doNotDistrubToolStripMenuItem, invisibleToolStripMenuItem });
        setStatusToolStripMenuItem.Name = "setStatusToolStripMenuItem";
        setStatusToolStripMenuItem.Size = new Size(180, 22);
        setStatusToolStripMenuItem.Text = "Set Status";
        // 
        // availableToolStripMenuItem
        // 
        availableToolStripMenuItem.Image = Properties.Resources.line;
        availableToolStripMenuItem.Name = "availableToolStripMenuItem";
        availableToolStripMenuItem.Size = new Size(150, 22);
        availableToolStripMenuItem.Text = "Available";
        availableToolStripMenuItem.Click += AvailableToolStripMenuItem_Click;
        // 
        // busyToolStripMenuItem
        // 
        busyToolStripMenuItem.Image = Properties.Resources.red;
        busyToolStripMenuItem.Name = "busyToolStripMenuItem";
        busyToolStripMenuItem.Size = new Size(150, 22);
        busyToolStripMenuItem.Text = "Busy";
        busyToolStripMenuItem.Click += BusyToolStripMenuItem_Click;
        // 
        // awayToolStripMenuItem
        // 
        awayToolStripMenuItem.Image = Properties.Resources.yellow;
        awayToolStripMenuItem.Name = "awayToolStripMenuItem";
        awayToolStripMenuItem.Size = new Size(150, 22);
        awayToolStripMenuItem.Text = "Away";
        awayToolStripMenuItem.Click += AwayToolStripMenuItem_Click;
        // 
        // doNotDistrubToolStripMenuItem
        // 
        doNotDistrubToolStripMenuItem.Image = Properties.Resources.dnd;
        doNotDistrubToolStripMenuItem.Name = "doNotDistrubToolStripMenuItem";
        doNotDistrubToolStripMenuItem.Size = new Size(150, 22);
        doNotDistrubToolStripMenuItem.Text = "Do not distrub";
        doNotDistrubToolStripMenuItem.Click += DoNotDistrubToolStripMenuItem_Click;
        // 
        // invisibleToolStripMenuItem
        // 
        invisibleToolStripMenuItem.Image = Properties.Resources.gray;
        invisibleToolStripMenuItem.Name = "invisibleToolStripMenuItem";
        invisibleToolStripMenuItem.Size = new Size(150, 22);
        invisibleToolStripMenuItem.Text = "Invisible";
        invisibleToolStripMenuItem.Click += InvisibleToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(177, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(180, 22);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // BtnSendMessage
        // 
        BtnSendMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnSendMessage.BackColor = Color.LimeGreen;
        BtnSendMessage.FlatStyle = FlatStyle.Flat;
        BtnSendMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnSendMessage.Location = new Point(558, 535);
        BtnSendMessage.Margin = new Padding(4, 3, 4, 3);
        BtnSendMessage.Name = "BtnSendMessage";
        BtnSendMessage.Size = new Size(74, 27);
        BtnSendMessage.TabIndex = 5;
        BtnSendMessage.Text = "Send";
        BtnSendMessage.UseVisualStyleBackColor = false;
        BtnSendMessage.Click += BtnSendMessage_Click;
        // 
        // FrmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1026, 577);
        Controls.Add(LvClients);
        Controls.Add(TxtMessage);
        Controls.Add(BtnSendMessage);
        Controls.Add(TxtMessages);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MinimumSize = new Size(680, 321);
        Name = "FrmMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chat";
        FormClosing += FrmMain_FormClosing;
        Load += FrmMain_Load;
        NotifyMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private System.Windows.Forms.TextBox TxtMessages;
    private System.Windows.Forms.Button BtnSendMessage;
    private System.Windows.Forms.TextBox TxtMessage;
    private System.Windows.Forms.ListView LvClients;
    private System.Windows.Forms.ColumnHeader Nick;
    private System.Windows.Forms.NotifyIcon NotifyIcon1;
    private System.Windows.Forms.ContextMenuStrip NotifyMenu;
    private System.Windows.Forms.ToolStripMenuItem setStatusToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem availableToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem busyToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem awayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem doNotDistrubToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem invisibleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
}
