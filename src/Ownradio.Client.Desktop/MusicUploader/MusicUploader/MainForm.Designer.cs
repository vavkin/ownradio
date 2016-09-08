namespace OwnRadio.DesktopPlayer
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.fileID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PathFromFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderBrowserDialogMain = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.playButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSelect,
            this.toolStripButtonUpload,
            this.toolStripSeparator1,
            this.toolStripButtonSettings});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(810, 39);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonSelect
            // 
            this.toolStripButtonSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelect.Image")));
            this.toolStripButtonSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelect.Name = "toolStripButtonSelect";
            this.toolStripButtonSelect.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSelect.Text = "Выбрать";
            this.toolStripButtonSelect.Click += new System.EventHandler(this.toolStripButtonSelect_Click);
            // 
            // toolStripButtonUpload
            // 
            this.toolStripButtonUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUpload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpload.Image")));
            this.toolStripButtonUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpload.Name = "toolStripButtonUpload";
            this.toolStripButtonUpload.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonUpload.Text = "toolStripButtonUpload";
            this.toolStripButtonUpload.ToolTipText = "Загрузить";
            this.toolStripButtonUpload.Click += new System.EventHandler(this.toolStripButtonUpload_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonSettings.Text = "toolStripButtonSettings";
            this.toolStripButtonSettings.ToolTipText = "Настройки";
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripButtonSettings_Click);
            // 
            // listViewFiles
            // 
            this.listViewFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileID,
            this.FileName,
            this.PathFromFolder});
            this.listViewFiles.Location = new System.Drawing.Point(16, 52);
            this.listViewFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(777, 245);
            this.listViewFiles.TabIndex = 1;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            // 
            // fileID
            // 
            this.fileID.Text = "GUID";
            this.fileID.Width = 180;
            // 
            // FileName
            // 
            this.FileName.Text = "Имя файла";
            this.FileName.Width = 180;
            // 
            // PathFromFolder
            // 
            this.PathFromFolder.Text = "Путь";
            this.PathFromFolder.Width = 180;
            // 
            // textBoxLog
            // 
            this.textBoxLog.AcceptsReturn = true;
            this.textBoxLog.AcceptsTab = true;
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(16, 305);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(776, 105);
            this.textBoxLog.TabIndex = 2;
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.playButton.Location = new System.Drawing.Point(16, 417);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(55, 38);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "⏸";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nextButton.Location = new System.Drawing.Point(77, 417);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(31, 38);
            this.nextButton.TabIndex = 4;
            this.nextButton.Text = "⏭";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 467);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.toolStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Загрузчик файлов";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStripMain;
		private System.Windows.Forms.ToolStripButton toolStripButtonSelect;
		private System.Windows.Forms.ToolStripButton toolStripButtonUpload;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
		private System.Windows.Forms.ListView listViewFiles;
		private System.Windows.Forms.ColumnHeader PathFromFolder;
		private System.Windows.Forms.ColumnHeader FileName;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogMain;
		private System.Windows.Forms.ColumnHeader fileID;
		private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button nextButton;
    }
}

