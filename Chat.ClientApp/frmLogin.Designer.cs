namespace Chat.ClientApp;

partial class FrmLogin
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
        GrpConnection = new GroupBox();
        label3 = new Label();
        label2 = new Label();
        label1 = new Label();
        BtnConnect = new Button();
        TxtIPAddress = new TextBox();
        TxtNick = new TextBox();
        TxtPortNo = new TextBox();
        ErrorProvider1 = new ErrorProvider(components);
        GrpConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)ErrorProvider1).BeginInit();
        SuspendLayout();
        // 
        // GrpConnection
        // 
        GrpConnection.Controls.Add(label3);
        GrpConnection.Controls.Add(label2);
        GrpConnection.Controls.Add(label1);
        GrpConnection.Controls.Add(BtnConnect);
        GrpConnection.Controls.Add(TxtIPAddress);
        GrpConnection.Controls.Add(TxtNick);
        GrpConnection.Controls.Add(TxtPortNo);
        GrpConnection.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        GrpConnection.Location = new Point(14, 14);
        GrpConnection.Margin = new Padding(4, 3, 4, 3);
        GrpConnection.Name = "GrpConnection";
        GrpConnection.Padding = new Padding(4, 3, 4, 3);
        GrpConnection.Size = new Size(391, 218);
        GrpConnection.TabIndex = 10;
        GrpConnection.TabStop = false;
        GrpConnection.Text = "Connection";
        // 
        // label3
        // 
        label3.BackColor = Color.SkyBlue;
        label3.BorderStyle = BorderStyle.FixedSingle;
        label3.FlatStyle = FlatStyle.Flat;
        label3.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label3.Location = new Point(13, 91);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(164, 26);
        label3.TabIndex = 7;
        label3.Text = "Nick : ";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label2
        // 
        label2.BackColor = Color.SkyBlue;
        label2.BorderStyle = BorderStyle.FixedSingle;
        label2.FlatStyle = FlatStyle.Flat;
        label2.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label2.Location = new Point(13, 58);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(164, 26);
        label2.TabIndex = 7;
        label2.Text = "Port No : ";
        label2.TextAlign = ContentAlignment.MiddleRight;
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
        label1.Size = new Size(164, 26);
        label1.TabIndex = 7;
        label1.Text = "IP Address : ";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // BtnConnect
        // 
        BtnConnect.BackColor = Color.LimeGreen;
        BtnConnect.FlatStyle = FlatStyle.Flat;
        BtnConnect.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnConnect.Location = new Point(231, 155);
        BtnConnect.Margin = new Padding(4, 3, 4, 3);
        BtnConnect.Name = "BtnConnect";
        BtnConnect.Size = new Size(141, 39);
        BtnConnect.TabIndex = 3;
        BtnConnect.Text = "Connect";
        BtnConnect.UseVisualStyleBackColor = false;
        BtnConnect.Click += BtnConnect_Click;
        // 
        // TxtIPAddress
        // 
        TxtIPAddress.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtIPAddress.Location = new Point(184, 24);
        TxtIPAddress.Margin = new Padding(4, 3, 4, 3);
        TxtIPAddress.Name = "TxtIPAddress";
        TxtIPAddress.Size = new Size(187, 23);
        TxtIPAddress.TabIndex = 0;
        TxtIPAddress.Text = "127.0.0.1";
        TxtIPAddress.KeyPress += Txt_KeyPress;
        // 
        // TxtNick
        // 
        TxtNick.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtNick.Location = new Point(184, 91);
        TxtNick.Margin = new Padding(4, 3, 4, 3);
        TxtNick.Name = "TxtNick";
        TxtNick.Size = new Size(187, 23);
        TxtNick.TabIndex = 2;
        TxtNick.TextAlign = HorizontalAlignment.Right;
        TxtNick.KeyPress += Txt_KeyPress;
        // 
        // TxtPortNo
        // 
        TxtPortNo.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtPortNo.Location = new Point(184, 58);
        TxtPortNo.Margin = new Padding(4, 3, 4, 3);
        TxtPortNo.Name = "TxtPortNo";
        TxtPortNo.Size = new Size(187, 23);
        TxtPortNo.TabIndex = 1;
        TxtPortNo.Text = "42001";
        TxtPortNo.TextAlign = HorizontalAlignment.Right;
        TxtPortNo.KeyPress += Txt_KeyPress;
        // 
        // ErrorProvider1
        // 
        ErrorProvider1.ContainerControl = this;
        // 
        // FrmLogin
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Azure;
        ClientSize = new Size(420, 246);
        Controls.Add(GrpConnection);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Margin = new Padding(4, 3, 4, 3);
        Name = "FrmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Login";
        GrpConnection.ResumeLayout(false);
        GrpConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)ErrorProvider1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox GrpConnection;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button BtnConnect;
    private System.Windows.Forms.TextBox TxtIPAddress;
    private System.Windows.Forms.TextBox TxtPortNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TxtNick;
    private System.Windows.Forms.ErrorProvider ErrorProvider1;
}