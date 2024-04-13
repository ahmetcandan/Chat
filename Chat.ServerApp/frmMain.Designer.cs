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
        BtnStart = new Button();
        TxtPortNo = new TextBox();
        TxtMessages = new TextBox();
        grpConnection = new GroupBox();
        label1 = new Label();
        grpMessages = new GroupBox();
        frpUsers = new GroupBox();
        LvClients = new ListView();
        clientId = new ColumnHeader();
        nickName = new ColumnHeader();
        ipAddress = new ColumnHeader();
        MenuClients = new ContextMenuStrip(components);
        banToolStripMenuItem = new ToolStripMenuItem();
        NotifyIcon1 = new NotifyIcon(components);
        NotifyMenu = new ContextMenuStrip(components);
        openToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        ErrorProvider1 = new ErrorProvider(components);
        grpConnection.SuspendLayout();
        grpMessages.SuspendLayout();
        frpUsers.SuspendLayout();
        MenuClients.SuspendLayout();
        NotifyMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)ErrorProvider1).BeginInit();
        SuspendLayout();
        // 
        // BtnStart
        // 
        BtnStart.BackColor = Color.LimeGreen;
        BtnStart.FlatStyle = FlatStyle.Flat;
        BtnStart.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnStart.Location = new Point(287, 24);
        BtnStart.Margin = new Padding(4, 3, 4, 3);
        BtnStart.Name = "BtnStart";
        BtnStart.Size = new Size(99, 27);
        BtnStart.TabIndex = 0;
        BtnStart.Text = "Start";
        BtnStart.UseVisualStyleBackColor = false;
        BtnStart.Click += BtnStart_Click;
        // 
        // TxtPortNo
        // 
        TxtPortNo.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtPortNo.Location = new Point(163, 24);
        TxtPortNo.Margin = new Padding(4, 3, 4, 3);
        TxtPortNo.Name = "TxtPortNo";
        TxtPortNo.Size = new Size(116, 23);
        TxtPortNo.TabIndex = 6;
        TxtPortNo.Text = "42001";
        TxtPortNo.TextAlign = HorizontalAlignment.Right;
        // 
        // TxtMessages
        // 
        TxtMessages.AcceptsReturn = true;
        TxtMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TxtMessages.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtMessages.Location = new Point(13, 28);
        TxtMessages.Margin = new Padding(4, 3, 4, 3);
        TxtMessages.Multiline = true;
        TxtMessages.Name = "TxtMessages";
        TxtMessages.ReadOnly = true;
        TxtMessages.ScrollBars = ScrollBars.Vertical;
        TxtMessages.Size = new Size(764, 434);
        TxtMessages.TabIndex = 7;
        // 
        // grpConnection
        // 
        grpConnection.Controls.Add(label1);
        grpConnection.Controls.Add(BtnStart);
        grpConnection.Controls.Add(TxtPortNo);
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
        grpMessages.Controls.Add(TxtMessages);
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
        frpUsers.Controls.Add(LvClients);
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
        // LvClients
        // 
        LvClients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LvClients.Columns.AddRange(new ColumnHeader[] { clientId, nickName, ipAddress });
        LvClients.ContextMenuStrip = MenuClients;
        LvClients.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        LvClients.FullRowSelect = true;
        LvClients.GridLines = true;
        LvClients.Location = new Point(13, 28);
        LvClients.Margin = new Padding(4, 3, 4, 3);
        LvClients.Name = "LvClients";
        LvClients.Size = new Size(373, 357);
        LvClients.TabIndex = 9;
        LvClients.UseCompatibleStateImageBehavior = false;
        LvClients.View = View.Details;
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
        // MenuClients
        // 
        MenuClients.Items.AddRange(new ToolStripItem[] { banToolStripMenuItem });
        MenuClients.Name = "menuClients";
        MenuClients.Size = new Size(99, 26);
        MenuClients.Opening += MenuClients_Opening;
        // 
        // banToolStripMenuItem
        // 
        banToolStripMenuItem.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        banToolStripMenuItem.Name = "banToolStripMenuItem";
        banToolStripMenuItem.Size = new Size(180, 22);
        banToolStripMenuItem.Text = "Ban";
        banToolStripMenuItem.Click += BanToolStripMenuItem_Click;
        // 
        // NotifyIcon1
        // 
        NotifyIcon1.ContextMenuStrip = NotifyMenu;
        NotifyIcon1.Icon = (Icon)resources.GetObject("NotifyIcon1.Icon");
        NotifyIcon1.Text = "Chat | Server";
        NotifyIcon1.Visible = true;
        NotifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
        // 
        // NotifyMenu
        // 
        NotifyMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        NotifyMenu.Name = "notifyMenu";
        NotifyMenu.Size = new Size(105, 54);
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
        exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // ErrorProvider1
        // 
        ErrorProvider1.ContainerControl = this;
        // 
        // FrmMain
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
        Name = "FrmMain";
        Text = "Server | Chat";
        FormClosing += FrmMain_FormClosing;
        Load += FrmMain_Load;
        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        grpMessages.ResumeLayout(false);
        grpMessages.PerformLayout();
        frpUsers.ResumeLayout(false);
        MenuClients.ResumeLayout(false);
        NotifyMenu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)ErrorProvider1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button BtnStart;
    private System.Windows.Forms.TextBox TxtPortNo;
    private System.Windows.Forms.TextBox TxtMessages;
    private System.Windows.Forms.GroupBox grpConnection;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox grpMessages;
    private System.Windows.Forms.GroupBox frpUsers;
    private System.Windows.Forms.ListView LvClients;
    private System.Windows.Forms.ColumnHeader nickName;
    private System.Windows.Forms.ColumnHeader ipAddress;
    private System.Windows.Forms.NotifyIcon NotifyIcon1;
    private System.Windows.Forms.ContextMenuStrip NotifyMenu;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ErrorProvider ErrorProvider1;
    private System.Windows.Forms.ColumnHeader clientId;
    private System.Windows.Forms.ContextMenuStrip MenuClients;
    private System.Windows.Forms.ToolStripMenuItem banToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
}
