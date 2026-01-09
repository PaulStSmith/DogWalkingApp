using DogWalkingApp.UI.Constants;

namespace DogWalkingApp.UI.Forms;

partial class WalkEditorForm
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
        lblTitle = new Label();
        lblDogInfo = new Label();
        lblDateTime = new Label();
        dtpWalkDateTime = new DateTimePicker();
        lblDuration = new Label();
        numDuration = new NumericUpDown();
        lblMinutes = new Label();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblValidationErrors = new Label();
        ((System.ComponentModel.ISupportInitialize)numDuration).BeginInit();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(12, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(125, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "🚶 Walk Editor";
        // 
        // lblDogInfo
        // 
        lblDogInfo.AutoSize = true;
        lblDogInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblDogInfo.Location = new Point(12, 48);
        lblDogInfo.Name = "lblDogInfo";
        lblDogInfo.Size = new Size(87, 15);
        lblDogInfo.TabIndex = 1;
        lblDogInfo.Text = "Walk for: [Dog]";
        // 
        // lblDateTime
        // 
        lblDateTime.AutoSize = true;
        lblDateTime.Location = new Point(12, 73);
        lblDateTime.Name = "lblDateTime";
        lblDateTime.Size = new Size(66, 15);
        lblDateTime.TabIndex = 2;
        lblDateTime.Text = "Date/Time:";
        // 
        // dtpWalkDateTime
        // 
        dtpWalkDateTime.CustomFormat = "yyyy-MM-dd HH:mm";
        dtpWalkDateTime.Format = DateTimePickerFormat.Custom;
        dtpWalkDateTime.Location = new Point(120, 72);
        dtpWalkDateTime.Name = "dtpWalkDateTime";
        dtpWalkDateTime.ShowUpDown = true;
        dtpWalkDateTime.Size = new Size(150, 23);
        dtpWalkDateTime.TabIndex = 3;
        // 
        // lblDuration
        // 
        lblDuration.AutoSize = true;
        lblDuration.Location = new Point(12, 101);
        lblDuration.Name = "lblDuration";
        lblDuration.Size = new Size(56, 15);
        lblDuration.TabIndex = 4;
        lblDuration.Text = "Duration:";
        // 
        // numDuration
        // 
        numDuration.Location = new Point(120, 101);
        numDuration.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
        numDuration.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        numDuration.Name = "numDuration";
        numDuration.Size = new Size(80, 23);
        numDuration.TabIndex = 5;
        numDuration.Value = new decimal(new int[] { 30, 0, 0, 0 });
        // 
        // lblMinutes
        // 
        lblMinutes.AutoSize = true;
        lblMinutes.Location = new Point(210, 103);
        lblMinutes.Name = "lblMinutes";
        lblMinutes.Size = new Size(50, 15);
        lblMinutes.TabIndex = 6;
        lblMinutes.Text = "minutes";
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 130);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 7;
        lblNotes.Text = "Notes:";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(12, 150);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(356, 77);
        txtNotes.TabIndex = 8;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(208, 319);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 9;
        btnSave.Text = "💾 Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(293, 319);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "❌ Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblValidationErrors
        // 
        lblValidationErrors.ForeColor = Color.Red;
        lblValidationErrors.Location = new Point(12, 230);
        lblValidationErrors.Name = "lblValidationErrors";
        lblValidationErrors.Size = new Size(356, 86);
        lblValidationErrors.TabIndex = 11;
        // 
        // WalkEditorForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(380, 361);
        Controls.Add(lblValidationErrors);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(lblMinutes);
        Controls.Add(numDuration);
        Controls.Add(lblDuration);
        Controls.Add(dtpWalkDateTime);
        Controls.Add(lblDateTime);
        Controls.Add(lblDogInfo);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "WalkEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Walk Editor";
        ((System.ComponentModel.ISupportInitialize)numDuration).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblDogInfo;
    private Label lblDateTime;
    private DateTimePicker dtpWalkDateTime;
    private Label lblDuration;
    private NumericUpDown numDuration;
    private Label lblMinutes;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnCancel;
    private Label lblValidationErrors;
}