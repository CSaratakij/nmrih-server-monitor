namespace NMRIH
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstServerStat = new System.Windows.Forms.ListView();
            this.colServerNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerOnlineStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerProcessStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colServerUptime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lstServerStat
            // 
            this.lstServerStat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colServerNumber,
            this.colServerName,
            this.colServerPort,
            this.colServerOnlineStatus,
            this.colServerProcessStatus,
            this.colServerUptime});
            this.lstServerStat.Location = new System.Drawing.Point(12, 12);
            this.lstServerStat.Name = "lstServerStat";
            this.lstServerStat.Size = new System.Drawing.Size(660, 284);
            this.lstServerStat.TabIndex = 0;
            this.lstServerStat.UseCompatibleStateImageBehavior = false;
            this.lstServerStat.View = System.Windows.Forms.View.Details;
            // 
            // colServerNumber
            // 
            this.colServerNumber.Text = "No.";
            this.colServerNumber.Width = 30;
            // 
            // colServerName
            // 
            this.colServerName.Text = "Name";
            this.colServerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colServerName.Width = 200;
            // 
            // colServerPort
            // 
            this.colServerPort.Text = "Port";
            this.colServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colServerPort.Width = 100;
            // 
            // colServerOnlineStatus
            // 
            this.colServerOnlineStatus.Text = "Status";
            this.colServerOnlineStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colServerOnlineStatus.Width = 100;
            // 
            // colServerProcessStatus
            // 
            this.colServerProcessStatus.Text = "Process";
            this.colServerProcessStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colServerProcessStatus.Width = 100;
            // 
            // colServerUptime
            // 
            this.colServerUptime.Text = "Uptime";
            this.colServerUptime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colServerUptime.Width = 100;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 330);
            this.Controls.Add(this.lstServerStat);
            this.Name = "MainForm";
            this.Text = "NMRIH : Server Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstServerStat;
        private System.Windows.Forms.ColumnHeader colServerNumber;
        private System.Windows.Forms.ColumnHeader colServerName;
        private System.Windows.Forms.ColumnHeader colServerPort;
        private System.Windows.Forms.ColumnHeader colServerOnlineStatus;
        private System.Windows.Forms.ColumnHeader colServerProcessStatus;
        private System.Windows.Forms.ColumnHeader colServerUptime;
    }
}

