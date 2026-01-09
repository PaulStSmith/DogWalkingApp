using DogWalkingApp.UI.Constants;

namespace DogWalkingApp.UI.Forms;

partial class DogEditorForm
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
        lblName = new Label();
        txtName = new TextBox();
        lblBreed = new Label();
        txtBreed = new TextBox();
        lblAge = new Label();
        numAge = new NumericUpDown();
        btnSave = new Button();
        btnCancel = new Button();
        lblTitle = new Label();
        lblValidationErrors = new Label();
        ((System.ComponentModel.ISupportInitialize)numAge).BeginInit();
        SuspendLayout();
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(12, 59);
        lblName.Name = "lblName";
        lblName.Size = new Size(42, 15);
        lblName.TabIndex = 0;
        lblName.Text = "Name:";
        // 
        // txtName
        // 
        txtName.Location = new Point(100, 57);
        txtName.Name = "txtName";
        txtName.Size = new Size(238, 23);
        txtName.TabIndex = 1;
        // 
        // lblBreed
        // 
        lblBreed.AutoSize = true;
        lblBreed.Location = new Point(12, 88);
        lblBreed.Name = "lblBreed";
        lblBreed.Size = new Size(40, 15);
        lblBreed.TabIndex = 2;
        lblBreed.Text = "Breed:";
        // 
        // txtBreed
        // 
        txtBreed.Location = new Point(100, 86);
        txtBreed.Name = "txtBreed";
        txtBreed.Size = new Size(238, 23);
        txtBreed.TabIndex = 3;
        // 
        // lblAge
        // 
        lblAge.AutoSize = true;
        lblAge.Location = new Point(12, 116);
        lblAge.Name = "lblAge";
        lblAge.Size = new Size(31, 15);
        lblAge.TabIndex = 4;
        lblAge.Text = "Age:";
        // 
        // numAge
        // 
        numAge.Location = new Point(100, 115);
        numAge.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
        numAge.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numAge.Name = "numAge";
        numAge.Size = new Size(80, 23);
        numAge.TabIndex = 5;
        numAge.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // btnSave
        // 
        btnSave.Location = new Point(178, 198);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 6;
        btnSave.Text = "💾 Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(263, 198);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "❌ Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(12, 19);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(119, 21);
        lblTitle.TabIndex = 8;
        lblTitle.Text = "🐕 Dog Editor";
        // 
        // lblValidationErrors
        // 
        lblValidationErrors.ForeColor = Color.Red;
        lblValidationErrors.Location = new Point(12, 141);
        lblValidationErrors.Name = "lblValidationErrors";
        lblValidationErrors.Size = new Size(326, 54);
        lblValidationErrors.TabIndex = 9;
        // 
        // DogEditorForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(350, 240);
        Controls.Add(lblValidationErrors);
        Controls.Add(lblTitle);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(numAge);
        Controls.Add(lblAge);
        Controls.Add(txtBreed);
        Controls.Add(lblBreed);
        Controls.Add(txtName);
        Controls.Add(lblName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DogEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Dog Editor";
        ((System.ComponentModel.ISupportInitialize)numAge).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblName;
    private TextBox txtName;
    private Label lblBreed;
    private TextBox txtBreed;
    private Label lblAge;
    private NumericUpDown numAge;
    private Button btnSave;
    private Button btnCancel;
    private Label lblTitle;
    private Label lblValidationErrors;
}