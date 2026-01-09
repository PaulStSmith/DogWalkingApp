using DogWalkingApp.Resources;

namespace DogWalkingApp.UI;

partial class MainForm
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
        splitContainer1 = new SplitContainer();
        treeViewHierarchy = new TreeView();
        panel1 = new Panel();
        txtSearchTerm = new TextBox();
        label1 = new Label();
        splitContainer2 = new SplitContainer();
        panelClient = new Panel();
        btnClientSave = new Button();
        btnClientDelete = new Button();
        btnClientEdit = new Button();
        btnClientNew = new Button();
        txtClientPhone = new TextBox();
        lblClientPhone = new Label();
        txtClientName = new TextBox();
        lblClientName = new Label();
        lblClientTitle = new Label();
        splitContainer3 = new SplitContainer();
        panelDogs = new Panel();
        btnDogDelete = new Button();
        btnDogEdit = new Button();
        btnDogNew = new Button();
        dgvDogs = new DataGridView();
        lblDogsTitle = new Label();
        panelWalks = new Panel();
        btnWalkDelete = new Button();
        btnWalkEdit = new Button();
        btnWalkNew = new Button();
        dgvWalks = new DataGridView();
        lblWalksTitle = new Label();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
        splitContainer2.Panel1.SuspendLayout();
        splitContainer2.Panel2.SuspendLayout();
        splitContainer2.SuspendLayout();
        panelClient.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
        splitContainer3.Panel1.SuspendLayout();
        splitContainer3.Panel2.SuspendLayout();
        splitContainer3.SuspendLayout();
        panelDogs.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDogs).BeginInit();
        panelWalks.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvWalks).BeginInit();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(treeViewHierarchy);
        splitContainer1.Panel1.Controls.Add(panel1);
        splitContainer1.Panel1MinSize = 300;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(splitContainer2);
        splitContainer1.Panel2MinSize = 580;
        splitContainer1.Size = new Size(1000, 601);
        splitContainer1.SplitterDistance = 416;
        splitContainer1.TabIndex = 4;
        // 
        // treeViewHierarchy
        // 
        treeViewHierarchy.Dock = DockStyle.Fill;
        treeViewHierarchy.Font = new Font("Segoe UI", 9F);
        treeViewHierarchy.HideSelection = false;
        treeViewHierarchy.Location = new Point(0, 41);
        treeViewHierarchy.MinimumSize = new Size(300, 0);
        treeViewHierarchy.Name = "treeViewHierarchy";
        treeViewHierarchy.Size = new Size(416, 560);
        treeViewHierarchy.TabIndex = 1;
        // 
        // panel1
        // 
        panel1.Controls.Add(txtSearchTerm);
        panel1.Controls.Add(label1);
        panel1.Dock = DockStyle.Top;
        panel1.Location = new Point(0, 0);
        panel1.Name = "panel1";
        panel1.Size = new Size(416, 41);
        panel1.TabIndex = 2;
        // 
        // txtSearchTerm
        // 
        txtSearchTerm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSearchTerm.Location = new Point(87, 10);
        txtSearchTerm.Name = "txtSearchTerm";
        txtSearchTerm.Size = new Size(326, 23);
        txtSearchTerm.TabIndex = 2;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label1.Location = new Point(3, 11);
        label1.Name = "label1";
        label1.Size = new Size(0, 19);
        label1.TabIndex = 1;
        label1.Text = "🔍 Search";
        // 
        // splitContainer2
        // 
        splitContainer2.Dock = DockStyle.Fill;
        splitContainer2.Location = new Point(0, 0);
        splitContainer2.Name = "splitContainer2";
        splitContainer2.Orientation = Orientation.Horizontal;
        // 
        // splitContainer2.Panel1
        // 
        splitContainer2.Panel1.Controls.Add(panelClient);
        splitContainer2.Panel1MinSize = 160;
        // 
        // splitContainer2.Panel2
        // 
        splitContainer2.Panel2.Controls.Add(splitContainer3);
        splitContainer2.Size = new Size(580, 601);
        splitContainer2.SplitterDistance = 160;
        splitContainer2.TabIndex = 0;
        // 
        // panelClient
        // 
        panelClient.BorderStyle = BorderStyle.FixedSingle;
        panelClient.Controls.Add(btnClientSave);
        panelClient.Controls.Add(btnClientDelete);
        panelClient.Controls.Add(btnClientEdit);
        panelClient.Controls.Add(btnClientNew);
        panelClient.Controls.Add(txtClientPhone);
        panelClient.Controls.Add(lblClientPhone);
        panelClient.Controls.Add(txtClientName);
        panelClient.Controls.Add(lblClientName);
        panelClient.Controls.Add(lblClientTitle);
        panelClient.Dock = DockStyle.Fill;
        panelClient.Location = new Point(0, 0);
        panelClient.MinimumSize = new Size(580, 160);
        panelClient.Name = "panelClient";
        panelClient.Size = new Size(580, 160);
        panelClient.TabIndex = 2;
        // 
        // btnClientSave
        // 
        btnClientSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClientSave.Location = new Point(490, 113);
        btnClientSave.Name = "btnClientSave";
        btnClientSave.Size = new Size(75, 30);
        btnClientSave.TabIndex = 8;
        btnClientSave.Text = "💾 Save";
        btnClientSave.UseVisualStyleBackColor = true;
        // 
        // btnClientDelete
        // 
        btnClientDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClientDelete.Location = new Point(490, 77);
        btnClientDelete.Name = "btnClientDelete";
        btnClientDelete.Size = new Size(75, 30);
        btnClientDelete.TabIndex = 7;
        btnClientDelete.Text = "🗑️ Delete";
        btnClientDelete.UseVisualStyleBackColor = true;
        // 
        // btnClientEdit
        // 
        btnClientEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClientEdit.Location = new Point(490, 44);
        btnClientEdit.Name = "btnClientEdit";
        btnClientEdit.Size = new Size(75, 30);
        btnClientEdit.TabIndex = 6;
        btnClientEdit.Text = "✏️ Edit";
        btnClientEdit.UseVisualStyleBackColor = true;
        // 
        // btnClientNew
        // 
        btnClientNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClientNew.Location = new Point(490, 10);
        btnClientNew.Name = "btnClientNew";
        btnClientNew.Size = new Size(75, 30);
        btnClientNew.TabIndex = 5;
        btnClientNew.Text = "🆕 New";
        btnClientNew.UseVisualStyleBackColor = true;
        // 
        // txtClientPhone
        // 
        txtClientPhone.Location = new Point(80, 76);
        txtClientPhone.Name = "txtClientPhone";
        txtClientPhone.Size = new Size(150, 23);
        txtClientPhone.TabIndex = 4;
        // 
        // lblClientPhone
        // 
        lblClientPhone.AutoSize = true;
        lblClientPhone.Location = new Point(30, 79);
        lblClientPhone.Name = "lblClientPhone";
        lblClientPhone.Size = new Size(0, 15);
        lblClientPhone.TabIndex = 3;
        lblClientPhone.Text = "Phone:";
        // 
        // txtClientName
        // 
        txtClientName.Location = new Point(80, 47);
        txtClientName.Name = "txtClientName";
        txtClientName.Size = new Size(200, 23);
        txtClientName.TabIndex = 2;
        // 
        // lblClientName
        // 
        lblClientName.AutoSize = true;
        lblClientName.Location = new Point(32, 50);
        lblClientName.Name = "lblClientName";
        lblClientName.Size = new Size(0, 15);
        lblClientName.TabIndex = 1;
        lblClientName.Text = "Name:";
        // 
        // lblClientTitle
        // 
        lblClientTitle.AutoSize = true;
        lblClientTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblClientTitle.Location = new Point(10, 10);
        lblClientTitle.Name = "lblClientTitle";
        lblClientTitle.Size = new Size(0, 19);
        lblClientTitle.TabIndex = 0;
        lblClientTitle.Text = "🙍 Client Details";
        // 
        // splitContainer3
        // 
        splitContainer3.Dock = DockStyle.Fill;
        splitContainer3.Location = new Point(0, 0);
        splitContainer3.Name = "splitContainer3";
        splitContainer3.Orientation = Orientation.Horizontal;
        // 
        // splitContainer3.Panel1
        // 
        splitContainer3.Panel1.Controls.Add(panelDogs);
        splitContainer3.Panel1MinSize = 190;
        // 
        // splitContainer3.Panel2
        // 
        splitContainer3.Panel2.Controls.Add(panelWalks);
        splitContainer3.Size = new Size(580, 437);
        splitContainer3.SplitterDistance = 190;
        splitContainer3.TabIndex = 0;
        // 
        // panelDogs
        // 
        panelDogs.BorderStyle = BorderStyle.FixedSingle;
        panelDogs.Controls.Add(btnDogDelete);
        panelDogs.Controls.Add(btnDogEdit);
        panelDogs.Controls.Add(btnDogNew);
        panelDogs.Controls.Add(dgvDogs);
        panelDogs.Controls.Add(lblDogsTitle);
        panelDogs.Dock = DockStyle.Fill;
        panelDogs.Location = new Point(0, 0);
        panelDogs.MinimumSize = new Size(580, 190);
        panelDogs.Name = "panelDogs";
        panelDogs.Size = new Size(580, 190);
        panelDogs.TabIndex = 3;
        // 
        // btnDogDelete
        // 
        btnDogDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDogDelete.Location = new Point(490, 95);
        btnDogDelete.Name = "btnDogDelete";
        btnDogDelete.Size = new Size(75, 30);
        btnDogDelete.TabIndex = 4;
        btnDogDelete.Text = "🗑️ Delete";
        btnDogDelete.UseVisualStyleBackColor = true;
        // 
        // btnDogEdit
        // 
        btnDogEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDogEdit.Location = new Point(490, 65);
        btnDogEdit.Name = "btnDogEdit";
        btnDogEdit.Size = new Size(75, 30);
        btnDogEdit.TabIndex = 3;
        btnDogEdit.Text = "✏️ Edit";
        btnDogEdit.UseVisualStyleBackColor = true;
        // 
        // btnDogNew
        // 
        btnDogNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDogNew.Location = new Point(490, 35);
        btnDogNew.Name = "btnDogNew";
        btnDogNew.Size = new Size(75, 30);
        btnDogNew.TabIndex = 2;
        btnDogNew.Text = "🆕 New";
        btnDogNew.UseVisualStyleBackColor = true;
        // 
        // dgvDogs
        // 
        dgvDogs.AllowUserToAddRows = false;
        dgvDogs.AllowUserToDeleteRows = false;
        dgvDogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvDogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDogs.Location = new Point(10, 35);
        dgvDogs.Name = "dgvDogs";
        dgvDogs.ReadOnly = true;
        dgvDogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDogs.Size = new Size(470, 143);
        dgvDogs.TabIndex = 1;
        // 
        // lblDogsTitle
        // 
        lblDogsTitle.AutoSize = true;
        lblDogsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDogsTitle.Location = new Point(10, 10);
        lblDogsTitle.Name = "lblDogsTitle";
        lblDogsTitle.Size = new Size(0, 19);
        lblDogsTitle.TabIndex = 0;
        lblDogsTitle.Text = "🐕 Dogs for Selected Client";
        // 
        // panelWalks
        // 
        panelWalks.BorderStyle = BorderStyle.FixedSingle;
        panelWalks.Controls.Add(btnWalkDelete);
        panelWalks.Controls.Add(btnWalkEdit);
        panelWalks.Controls.Add(btnWalkNew);
        panelWalks.Controls.Add(dgvWalks);
        panelWalks.Controls.Add(lblWalksTitle);
        panelWalks.Dock = DockStyle.Fill;
        panelWalks.Location = new Point(0, 0);
        panelWalks.MinimumSize = new Size(580, 240);
        panelWalks.Name = "panelWalks";
        panelWalks.Size = new Size(580, 243);
        panelWalks.TabIndex = 4;
        // 
        // btnWalkDelete
        // 
        btnWalkDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnWalkDelete.Location = new Point(490, 95);
        btnWalkDelete.Name = "btnWalkDelete";
        btnWalkDelete.Size = new Size(75, 30);
        btnWalkDelete.TabIndex = 4;
        btnWalkDelete.Text = "🗑️ Delete";
        btnWalkDelete.UseVisualStyleBackColor = true;
        // 
        // btnWalkEdit
        // 
        btnWalkEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnWalkEdit.Location = new Point(490, 65);
        btnWalkEdit.Name = "btnWalkEdit";
        btnWalkEdit.Size = new Size(75, 30);
        btnWalkEdit.TabIndex = 3;
        btnWalkEdit.Text = "✏️ Edit";
        btnWalkEdit.UseVisualStyleBackColor = true;
        // 
        // btnWalkNew
        // 
        btnWalkNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnWalkNew.Location = new Point(490, 35);
        btnWalkNew.Name = "btnWalkNew";
        btnWalkNew.Size = new Size(75, 30);
        btnWalkNew.TabIndex = 2;
        btnWalkNew.Text = "🆕 New";
        btnWalkNew.UseVisualStyleBackColor = true;
        // 
        // dgvWalks
        // 
        dgvWalks.AllowUserToAddRows = false;
        dgvWalks.AllowUserToDeleteRows = false;
        dgvWalks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvWalks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvWalks.Location = new Point(10, 35);
        dgvWalks.Name = "dgvWalks";
        dgvWalks.ReadOnly = true;
        dgvWalks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvWalks.Size = new Size(470, 199);
        dgvWalks.TabIndex = 1;
        // 
        // lblWalksTitle
        // 
        lblWalksTitle.AutoSize = true;
        lblWalksTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblWalksTitle.Location = new Point(10, 10);
        lblWalksTitle.Name = "lblWalksTitle";
        lblWalksTitle.Size = new Size(0, 19);
        lblWalksTitle.TabIndex = 0;
        lblWalksTitle.Text = "🚶 Walks for Selected Dog";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 601);
        Controls.Add(splitContainer1);
        MinimumSize = new Size(1000, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Dog Walking Manager - Hierarchical View";
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        splitContainer2.Panel1.ResumeLayout(false);
        splitContainer2.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
        splitContainer2.ResumeLayout(false);
        panelClient.ResumeLayout(false);
        panelClient.PerformLayout();
        splitContainer3.Panel1.ResumeLayout(false);
        splitContainer3.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
        splitContainer3.ResumeLayout(false);
        panelDogs.ResumeLayout(false);
        panelDogs.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDogs).EndInit();
        panelWalks.ResumeLayout(false);
        panelWalks.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvWalks).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private SplitContainer splitContainer1;
    private TreeView treeViewHierarchy;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private Panel panelClient;
    private Button btnClientSave;
    private Button btnClientDelete;
    private Button btnClientEdit;
    private Button btnClientNew;
    private TextBox txtClientPhone;
    private Label lblClientPhone;
    private TextBox txtClientName;
    private Label lblClientName;
    private Label lblClientTitle;
    private Panel panelDogs;
    private Button btnDogDelete;
    private Button btnDogEdit;
    private Button btnDogNew;
    private DataGridView dgvDogs;
    private Label lblDogsTitle;
    private Panel panelWalks;
    private Button btnWalkDelete;
    private Button btnWalkEdit;
    private Button btnWalkNew;
    private DataGridView dgvWalks;
    private Label lblWalksTitle;
    private Panel panel1;
    private TextBox txtSearchTerm;
    private Label label1;
}