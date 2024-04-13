namespace Chat.ClientApp;

partial class FrmMessage
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMessage));
        TxtMessage = new TextBox();
        BtnSendMessage = new Button();
        TxtMessages = new TextBox();
        SuspendLayout();
        // 
        // TxtMessage
        // 
        TxtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TxtMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TxtMessage.Location = new Point(14, 505);
        TxtMessage.Margin = new Padding(4, 3, 4, 3);
        TxtMessage.Name = "TxtMessage";
        TxtMessage.Size = new Size(610, 23);
        TxtMessage.TabIndex = 10;
        TxtMessage.KeyPress += TxtMessage_KeyPress;
        // 
        // BtnSendMessage
        // 
        BtnSendMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnSendMessage.BackColor = Color.LimeGreen;
        BtnSendMessage.FlatStyle = FlatStyle.Flat;
        BtnSendMessage.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnSendMessage.Location = new Point(624, 505);
        BtnSendMessage.Margin = new Padding(4, 3, 4, 3);
        BtnSendMessage.Name = "BtnSendMessage";
        BtnSendMessage.Size = new Size(74, 27);
        BtnSendMessage.TabIndex = 9;
        BtnSendMessage.Text = "Send";
        BtnSendMessage.UseVisualStyleBackColor = false;
        BtnSendMessage.Click += BtnSendMessage_Click;
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
        TxtMessages.Size = new Size(683, 484);
        TxtMessages.TabIndex = 8;
        TxtMessages.TextChanged += TxtMessages_TextChanged;
        // 
        // FrmMessage
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(712, 546);
        Controls.Add(TxtMessage);
        Controls.Add(BtnSendMessage);
        Controls.Add(TxtMessages);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MinimumSize = new Size(441, 372);
        Name = "FrmMessage";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrmMessage";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TextBox TxtMessage;
    private System.Windows.Forms.Button BtnSendMessage;
    private System.Windows.Forms.TextBox TxtMessages;
}