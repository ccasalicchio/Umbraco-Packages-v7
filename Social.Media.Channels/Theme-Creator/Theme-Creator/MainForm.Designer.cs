using System.Windows.Forms;
namespace Theme_Creator
{
    partial class MainForm
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

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnAutoSet = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.txtCreationDate = new System.Windows.Forms.TextBox();
            this.txtThemeDescription = new System.Windows.Forms.TextBox();
            this.txtThemeID = new System.Windows.Forms.TextBox();
            this.lstIcons = new System.Windows.Forms.ListBox();
            this.errMsg = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblResults = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lnkBrowse = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtImg = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errMsg)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.errMsg.SetIconAlignment(this.btnClose, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.btnClose.Image = global::Theme_Creator.Properties.Resources.Close;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnClose.Location = new System.Drawing.Point(428, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(33, 33);
            this.btnClose.TabIndex = 16;
            this.toolTip.SetToolTip(this.btnClose, "Close");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            this.btnClose.MouseHover += new System.EventHandler(this.btnClose_MouseHover);
            // 
            // btnMinimize
            // 
            this.btnMinimize.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.errMsg.SetIconAlignment(this.btnMinimize, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.btnMinimize.Image = global::Theme_Creator.Properties.Resources.Minus;
            this.btnMinimize.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnMinimize.Location = new System.Drawing.Point(395, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(33, 33);
            this.btnMinimize.TabIndex = 17;
            this.toolTip.SetToolTip(this.btnMinimize, "Minimize");
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
            this.btnMinimize.MouseHover += new System.EventHandler(this.btnMinimize_MouseHover);
            // 
            // btnAutoSet
            // 
            this.btnAutoSet.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnAutoSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSet.Image = global::Theme_Creator.Properties.Resources.Auto_Document_Check;
            this.btnAutoSet.Location = new System.Drawing.Point(11, 321);
            this.btnAutoSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoSet.Name = "btnAutoSet";
            this.btnAutoSet.Size = new System.Drawing.Size(87, 41);
            this.btnAutoSet.TabIndex = 19;
            this.toolTip.SetToolTip(this.btnAutoSet, "Set All Channels");
            this.btnAutoSet.UseVisualStyleBackColor = true;
            this.btnAutoSet.Click += new System.EventHandler(this.btnAutoSet_Click);
            // 
            // btnSet
            // 
            this.btnSet.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.Image = global::Theme_Creator.Properties.Resources.Document_Check;
            this.btnSet.Location = new System.Drawing.Point(119, 321);
            this.btnSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(87, 41);
            this.btnSet.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnSet, "Set Channel Data");
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Location = new System.Drawing.Point(71, 238);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(122, 25);
            this.txtName.TabIndex = 1;
            this.toolTip.SetToolTip(this.txtName, "Name of the Channel");
            // 
            // txtID
            // 
            this.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtID.Location = new System.Drawing.Point(71, 190);
            this.txtID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(122, 25);
            this.txtID.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtID, "ID of the Channel (unique)");
            // 
            // btnGenerate
            // 
            this.btnGenerate.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Image = global::Theme_Creator.Properties.Resources.Data_Import;
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenerate.Location = new System.Drawing.Point(16, 318);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(212, 69);
            this.btnGenerate.TabIndex = 28;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnGenerate, "Generate Xml of Theme");
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFolder.Image = global::Theme_Creator.Properties.Resources.Folder_Add;
            this.btnOpenFolder.Location = new System.Drawing.Point(14, 25);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(35, 31);
            this.btnOpenFolder.TabIndex = 16;
            this.toolTip.SetToolTip(this.btnOpenFolder, "Open Folder");
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrl.Location = new System.Drawing.Point(88, 238);
            this.txtUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(102, 25);
            this.txtUrl.TabIndex = 26;
            this.toolTip.SetToolTip(this.txtUrl, "The website where the icons can be downloaded");
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Location = new System.Drawing.Point(88, 195);
            this.txtCreatedBy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.Size = new System.Drawing.Size(102, 25);
            this.txtCreatedBy.TabIndex = 24;
            this.toolTip.SetToolTip(this.txtCreatedBy, "The Creator of the Icons");
            // 
            // txtCreationDate
            // 
            this.txtCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreationDate.Location = new System.Drawing.Point(88, 152);
            this.txtCreationDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtCreationDate.Name = "txtCreationDate";
            this.txtCreationDate.Size = new System.Drawing.Size(102, 25);
            this.txtCreationDate.TabIndex = 21;
            this.toolTip.SetToolTip(this.txtCreationDate, "Date the Icons were created (yyyy-mm-dd)");
            // 
            // txtThemeDescription
            // 
            this.txtThemeDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThemeDescription.Location = new System.Drawing.Point(88, 108);
            this.txtThemeDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtThemeDescription.Name = "txtThemeDescription";
            this.txtThemeDescription.Size = new System.Drawing.Size(102, 25);
            this.txtThemeDescription.TabIndex = 19;
            this.toolTip.SetToolTip(this.txtThemeDescription, "Description of the Theme");
            // 
            // txtThemeID
            // 
            this.txtThemeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThemeID.Location = new System.Drawing.Point(88, 65);
            this.txtThemeID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtThemeID.Name = "txtThemeID";
            this.txtThemeID.Size = new System.Drawing.Size(102, 25);
            this.txtThemeID.TabIndex = 18;
            this.toolTip.SetToolTip(this.txtThemeID, "Unique ID of the Theme");
            // 
            // lstIcons
            // 
            this.lstIcons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstIcons.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstIcons.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstIcons.FormattingEnabled = true;
            this.lstIcons.ItemHeight = 17;
            this.lstIcons.Location = new System.Drawing.Point(11, 58);
            this.lstIcons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstIcons.Name = "lstIcons";
            this.lstIcons.Size = new System.Drawing.Size(195, 121);
            this.lstIcons.TabIndex = 31;
            this.toolTip.SetToolTip(this.lstIcons, "List of icons in the specific folder");
            this.lstIcons.SelectedIndexChanged += new System.EventHandler(this.lstIcons_SelectedIndexChanged);
            // 
            // errMsg
            // 
            this.errMsg.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lnkBrowse);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnGenerate);
            this.panel1.Location = new System.Drawing.Point(1, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 418);
            this.panel1.TabIndex = 18;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOpenFolder);
            this.groupBox2.Controls.Add(this.txtCreationDate);
            this.groupBox2.Controls.Add(this.lblResults);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtUrl);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtCreatedBy);
            this.groupBox2.Controls.Add(this.txtThemeID);
            this.groupBox2.Controls.Add(this.txtThemeDescription);
            this.groupBox2.Location = new System.Drawing.Point(16, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 297);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Package Details";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(11, 273);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(0, 17);
            this.lblResults.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Theme ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "Creation Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Created By";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Url";
            // 
            // lnkBrowse
            // 
            this.lnkBrowse.AutoSize = true;
            this.lnkBrowse.Location = new System.Drawing.Point(13, 389);
            this.lnkBrowse.Name = "lnkBrowse";
            this.lnkBrowse.Size = new System.Drawing.Size(0, 17);
            this.lnkBrowse.TabIndex = 32;
            this.lnkBrowse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBrowse_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.lstIcons);
            this.groupBox1.Controls.Add(this.btnAutoSet);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtID);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtImg);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(234, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(210, 373);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel Details";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Image = global::Theme_Creator.Properties.Resources.Bullets;
            this.pictureBox1.Location = new System.Drawing.Point(11, 28);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 22);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(21, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(21, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Name";
            // 
            // txtImg
            // 
            this.txtImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImg.Enabled = false;
            this.txtImg.Location = new System.Drawing.Point(71, 286);
            this.txtImg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.txtImg.Name = "txtImg";
            this.txtImg.Size = new System.Drawing.Size(122, 25);
            this.txtImg.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Image";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.btnMinimize);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Location = new System.Drawing.Point(1, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(461, 54);
            this.panel2.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(283, 37);
            this.label9.TabIndex = 18;
            this.label9.Text = "Theme XML Generator";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(463, 478);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Opacity = 0.9D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errMsg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ErrorProvider errMsg;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAutoSet;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtImg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.TextBox txtCreationDate;
        private System.Windows.Forms.TextBox txtThemeDescription;
        private System.Windows.Forms.TextBox txtThemeID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.LinkLabel lnkBrowse;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox lstIcons;
    }
}

