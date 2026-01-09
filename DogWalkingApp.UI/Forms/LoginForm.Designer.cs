using DogWalkingApp.Resources;

namespace DogWalkingApp.UI;

partial class LoginForm
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
        txtUsername = new TextBox();
        txtPassword = new TextBox();
        lblUsername = new Label();
        lblPassword = new Label();
        btnLogin = new Button();
        btnCancel = new Button();
        lblTitle = new Label();
        grpLogin = new GroupBox();
        lblErrorMessage = new Label();
        grpLogin.SuspendLayout();
        SuspendLayout();
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(80, 30);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(200, 23);
        txtUsername.TabIndex = 0;
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(80, 65);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '•';
        txtPassword.Size = new Size(200, 23);
        txtPassword.TabIndex = 1;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(15, 33);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(63, 15);
        lblUsername.TabIndex = 2;
        lblUsername.Text = "Username:";
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(15, 68);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(60, 15);
        lblPassword.TabIndex = 3;
        lblPassword.Text = "Password:";
        // 
        // btnLogin
        // 
        btnLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnLogin.Location = new Point(125, 140);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(75, 23);
        btnLogin.TabIndex = 2;
        btnLogin.Text = ButtonText.LoginWithKey;
        btnLogin.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCancel.Location = new Point(205, 140);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 3;
        btnCancel.Text = ButtonText.CancelWithKey;
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(65, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(181, 21);
        lblTitle.TabIndex = 6;
        lblTitle.Text = "Dog Walking Manager";
        // 
        // grpLogin
        // 
        grpLogin.Controls.Add(lblErrorMessage);
        grpLogin.Controls.Add(lblUsername);
        grpLogin.Controls.Add(txtUsername);
        grpLogin.Controls.Add(lblPassword);
        grpLogin.Controls.Add(txtPassword);
        grpLogin.Controls.Add(btnLogin);
        grpLogin.Controls.Add(btnCancel);
        grpLogin.Location = new Point(15, 45);
        grpLogin.Name = "grpLogin";
        grpLogin.Size = new Size(295, 175);
        grpLogin.TabIndex = 8;
        grpLogin.TabStop = false;
        grpLogin.Text = Labels.LoginCredentials;
        // 
        // lblErrorMessage
        // 
        lblErrorMessage.AutoSize = true;
        lblErrorMessage.ForeColor = Color.Red;
        lblErrorMessage.Location = new Point(15, 104);
        lblErrorMessage.Name = "lblErrorMessage";
        lblErrorMessage.Size = new Size(0, 15);
        lblErrorMessage.TabIndex = 8;
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(324, 232);
        Controls.Add(grpLogin);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = FormTitles.Login;
        grpLogin.ResumeLayout(false);
        grpLogin.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtUsername;
    private TextBox txtPassword;
    private Label lblUsername;
    private Label lblPassword;
    private Button btnLogin;
    private Button btnCancel;
    private Label lblTitle;
    private GroupBox grpLogin;
    private Label lblErrorMessage;
}