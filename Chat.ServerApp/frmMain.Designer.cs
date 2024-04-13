namespace Chat.ServerApp;

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
        btnStart = new Button();
        txtPortNo = new TextBox();
        txtMessages = new TextBox();
        grpConnection = new GroupBox();
        label1 = new Label();
        grpMessages = new GroupBox();
        frpUsers = new GroupBox();
        lvClients = new ListView();
        clientId = new ColumnHeader();
        nickName = new ColumnHeader();
        ipAddress = new ColumnHeader();
        menuClients = new ContextMenuStrip(components);
        banToolStripMenuItem = new ToolStripMenuItem();
        notifyIcon1 = new NotifyIcon(components);
        notifyMenu = new ContextMenuStrip(components);
        openToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        errorProvider1 = new ErrorProvider(components);
        grpConnection.SuspendLayout();
        grpMessages.SuspendLayout();
        frpUsers.SuspendLayout();
        menuClients.SuspendLayout();
        notifyMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.BackColor = Color.LimeGreen;
        btnStart.FlatStyle = FlatStyle.Flat;
        btnStart.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnStart.Location = new Point(287, 24);
        btnStart.Margin = new Padding(4, 3, 4, 3);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(99, 27);
        btnStart.TabIndex = 0;
        btnStart.Text = "Start";
        btnStart.UseVisualStyleBackColor = false;
        btnStart.Click += BtnStart_Click;
        // 
        // txtPortNo
        // 
        txtPortNo.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtPortNo.Location = new Point(163, 24);
        txtPortNo.Margin = new Padding(4, 3, 4, 3);
        txtPortNo.Name = "txtPortNo";
        txtPortNo.Size = new Size(116, 23);
        txtPortNo.TabIndex = 6;
        txtPortNo.Text = "42001";
        txtPortNo.TextAlign = HorizontalAlignment.Right;
        // 
        // txtMessages
        // 
        txtMessages.AcceptsReturn = true;
        txtMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessages.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtMessages.Location = new Point(13, 28);
        txtMessages.Margin = new Padding(4, 3, 4, 3);
        txtMessages.Multiline = true;
        txtMessages.Name = "txtMessages";
        txtMessages.ReadOnly = true;
        txtMessages.ScrollBars = ScrollBars.Vertical;
        txtMessages.Size = new Size(764, 434);
        txtMessages.TabIndex = 7;
        // 
        // grpConnection
        // 
        grpConnection.Controls.Add(label1);
        grpConnection.Controls.Add(btnStart);
        grpConnection.Controls.Add(txtPortNo);
        grpConnection.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        grpConnection.Location = new Point(14, 14);
        grpConnection.Margin = new Padding(4, 3, 4, 3);
        grpConnection.Name = "grpConnection";
        grpConnection.Padding = new Padding(4, 3, 4, 3);
        grpConnection.Size = new Size(402, 70);
        grpConnection.TabIndex = 9;
        grpConnection.TabStop = false;
        grpConnection.Text = "Connection";
        // 
        // label1
        // 
        label1.BackColor = Color.SkyBlue;
        label1.BorderStyle = BorderStyle.FixedSingle;
        label1.FlatStyle = FlatStyle.Flat;
        label1.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label1.Location = new Point(13, 24);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(143, 26);
        label1.TabIndex = 7;
        label1.Text = "Port No : ";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // grpMessages
        // 
        grpMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpMessages.Controls.Add(txtMessages);
        grpMessages.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        grpMessages.Location = new Point(424, 14);
        grpMessages.Margin = new Padding(4, 3, 4, 3);
        grpMessages.Name = "grpMessages";
        grpMessages.Padding = new Padding(4, 3, 4, 3);
        grpMessages.Size = new Size(796, 482);
        grpMessages.TabIndex = 10;
        grpMessages.TabStop = false;
        grpMessages.Text = "Messages";
        // 
        // frpUsers
        // 
        frpUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        frpUsers.Controls.Add(lvClients);
        frpUsers.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        frpUsers.Location = new Point(14, 91);
        frpUsers.Margin = new Padding(4, 3, 4, 3);
        frpUsers.Name = "frpUsers";
        frpUsers.Padding = new Padding(4, 3, 4, 3);
        frpUsers.Size = new Size(402, 405);
        frpUsers.TabIndex = 11;
        frpUsers.TabStop = false;
        frpUsers.Text = "Clients";
        // 
        // lvClients
        // 
        lvClients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lvClients.Columns.AddRange(new ColumnHeader[] { clientId, nickName, ipAddress });
        lvClients.ContextMenuStrip = menuClients;
        lvClients.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lvClients.FullRowSelect = true;
        lvClients.GridLines = true;
        lvClients.Location = new Point(13, 28);
        lvClients.Margin = new Padding(4, 3, 4, 3);
        lvClients.Name = "lvClients";
        lvClients.Size = new Size(373, 357);
        lvClients.TabIndex = 9;
        lvClients.UseCompatibleStateImageBehavior = false;
        lvClients.View = View.Details;
        // 
        // clientId
        // 
        clientId.DisplayIndex = 2;
        clientId.Text = "Client Id";
        clientId.TextAlign = HorizontalAlignment.Right;
        clientId.Width = 0;
        // 
        // nickName
        // 
        nickName.DisplayIndex = 0;
        nickName.Text = "Nick";
        nickName.Width = 144;
        // 
        // ipAddress
        // 
        ipAddress.DisplayIndex = 1;
        ipAddress.Text = "IP";
        ipAddress.Width = 168;
        // 
        // menuClients
        // 
        menuClients.Items.AddRange(new ToolStripItem[] { banToolStripMenuItem });
        menuClients.Name = "menuClients";
        menuClients.Size = new Size(99, 26);
        menuClients.Opening += MenuClients_Opening;
        // 
        // banToolStripMenuItem
        // 
        banToolStripMenuItem.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        banToolStripMenuItem.Name = "banToolStripMenuItem";
        banToolStripMenuItem.Size = new Size(98, 22);
        banToolStripMenuItem.Text = "Ban";
        banToolStripMenuItem.Click += BanToolStripMenuItem_Click;
        // 
        // notifyIcon1
        // 
        notifyIcon1.ContextMenuStrip = notifyMenu;
        notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
        notifyIcon1.Text = "Chat | Server";
        notifyIcon1.Visible = true;
        notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
        // 
        // notifyMenu
        // 
        notifyMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        notifyMenu.Name = "notifyMenu";
        notifyMenu.Size = new Size(105, 54);
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.Size = new Size(104, 22);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(101, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(104, 22);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // errorProvider1
        // 
        errorProvider1.ContainerControl = this;
        // 
        // frmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.GhostWhite;
        ClientSize = new Size(1233, 510);
        Controls.Add(frpUsers);
        Controls.Add(grpMessages);
        Controls.Add(grpConnection);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MinimumSize = new Size(709, 288);
        Name = "frmMain";
        Text = "Server | Chat";
        FormClosing += FrmMain_FormClosing;
        Load += FrmMain_Load;
        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        grpMessages.ResumeLayout(false);
        grpMessages.PerformLayout();
        frpUsers.ResumeLayout(false);
        menuClients.ResumeLayout(false);
        notifyMenu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.TextBox txtPortNo;
    private System.Windows.Forms.TextBox txtMessages;
    private System.Windows.Forms.GroupBox grpConnection;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox grpMessages;
    private System.Windows.Forms.GroupBox frpUsers;
    private System.Windows.Forms.ListView lvClients;
    private System.Windows.Forms.ColumnHeader nickName;
    private System.Windows.Forms.ColumnHeader ipAddress;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.ContextMenuStrip notifyMenu;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.ColumnHeader clientId;
    private System.Windows.Forms.ContextMenuStrip menuClients;
    private System.Windows.Forms.ToolStripMenuItem banToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
}
