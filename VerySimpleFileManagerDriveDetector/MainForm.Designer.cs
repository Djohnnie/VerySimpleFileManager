namespace VerySimpleFileManagerDriveDetector;

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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        notificationTrayIcon = new NotifyIcon(components);
        pollingTimer = new System.Windows.Forms.Timer(components);
        SuspendLayout();
        // 
        // notificationTrayIcon
        // 
        notificationTrayIcon.Icon = (Icon)resources.GetObject("notificationTrayIcon.Icon");
        notificationTrayIcon.Text = "notifyIcon1";
        notificationTrayIcon.Visible = true;
        // 
        // pollingTimer
        // 
        pollingTimer.Enabled = true;
        pollingTimer.Interval = 10000;
        pollingTimer.Tick += pollingTimer_Tick;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(563, 0);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainForm";
        ShowInTaskbar = false;
        Text = "Very Simple File Manager - Drive Detector";
        WindowState = FormWindowState.Minimized;
        ResumeLayout(false);
    }

    #endregion

    private NotifyIcon notificationTrayIcon;
    private System.Windows.Forms.Timer pollingTimer;
}