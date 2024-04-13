namespace Chat.ClientApp;

partial class frmMain
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
        txtMessages = new TextBox();
        txtMessage = new TextBox();
        lvClients = new ListView();
        Nick = new ColumnHeader();
        notifyIcon1 = new NotifyIcon(components);
        notifyMenu = new ContextMenuStrip(components);
        openToolStripMenuItem = new ToolStripMenuItem();
        setStatusToolStripMenuItem = new ToolStripMenuItem();
        availableToolStripMenuItem = new ToolStripMenuItem();
        busyToolStripMenuItem = new ToolStripMenuItem();
        awayToolStripMenuItem = new ToolStripMenuItem();
        doNotDistrubToolStripMenuItem = new ToolStripMenuItem();
        invisibleToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        btnSendMessage = new Button();
        notifyMenu.SuspendLayout();
        SuspendLayout();
        // 
        // txtMessages
        // 
        txtMessages.AcceptsReturn = true;
        txtMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessages.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtMessages.Location = new Point(14, 14);
        txtMessages.Margin = new Padding(4, 3, 4, 3);
        txtMessages.Multiline = true;
        txtMessages.Name = "txtMessages";
        txtMessages.ReadOnly = true;
        txtMessages.ScrollBars = ScrollBars.Vertical;
        txtMessages.Size = new Size(616, 514);
        txtMessages.TabIndex = 4;
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtMessage.Location = new Point(14, 535);
        txtMessage.Margin = new Padding(4, 3, 4, 3);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(543, 23);
        txtMessage.TabIndex = 7;
        txtMessage.KeyPress += TxtMessage_KeyPress;
        // 
        // lvClients
        // 
        lvClients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        lvClients.Columns.AddRange(new ColumnHeader[] { Nick });
        lvClients.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lvClients.FullRowSelect = true;
        lvClients.GridLines = true;
        lvClients.Location = new Point(638, 14);
        lvClients.Margin = new Padding(4, 3, 4, 3);
        lvClients.Name = "lvClients";
        lvClients.Size = new Size(373, 547);
        lvClients.TabIndex = 9;
        lvClients.UseCompatibleStateImageBehavior = false;
        lvClients.View = View.Details;
        lvClients.DoubleClick += LvClients_DoubleClick;
        // 
        // Nick
        // 
        Nick.Text = "Nick";
        Nick.TextAlign = HorizontalAlignment.Right;
        Nick.Width = 316;
        // 
        // notifyIcon1
        // 
        notifyIcon1.ContextMenuStrip = notifyMenu;
        notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
        notifyIcon1.Text = "Chat | Client";
        notifyIcon1.Visible = true;
        notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
        // 
        // notifyMenu
        // 
        notifyMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, setStatusToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        notifyMenu.Name = "notifyMenu";
        notifyMenu.Size = new Size(126, 76);
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.Size = new Size(125, 22);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
        // 
        // setStatusToolStripMenuItem
        // 
        setStatusToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { availableToolStripMenuItem, busyToolStripMenuItem, awayToolStripMenuItem, doNotDistrubToolStripMenuItem, invisibleToolStripMenuItem });
        setStatusToolStripMenuItem.Name = "setStatusToolStripMenuItem";
        setStatusToolStripMenuItem.Size = new Size(125, 22);
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
        toolStripSeparator1.Size = new Size(122, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(125, 22);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // btnSendMessage
        // 
        btnSendMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSendMessage.BackColor = Color.LimeGreen;
        btnSendMessage.FlatStyle = FlatStyle.Flat;
        btnSendMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnSendMessage.Location = new Point(558, 535);
        btnSendMessage.Margin = new Padding(4, 3, 4, 3);
        btnSendMessage.Name = "btnSendMessage";
        btnSendMessage.Size = new Size(74, 27);
        btnSendMessage.TabIndex = 5;
        btnSendMessage.Text = "Send";
        btnSendMessage.UseVisualStyleBackColor = false;
        btnSendMessage.Click += BtnSendMessage_Click;
        // 
        // frmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1026, 577);
        Controls.Add(lvClients);
        Controls.Add(txtMessage);
        Controls.Add(btnSendMessage);
        Controls.Add(txtMessages);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MinimumSize = new Size(680, 321);
        Name = "frmMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Chat";
        FormClosing += FrmMain_FormClosing;
        Load += FrmMain_Load;
        notifyMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private System.Windows.Forms.TextBox txtMessages;
    private System.Windows.Forms.Button btnSendMessage;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.ListView lvClients;
    private System.Windows.Forms.ColumnHeader Nick;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.ContextMenuStrip notifyMenu;
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
