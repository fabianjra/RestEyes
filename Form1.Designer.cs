namespace RestEyes
{
  partial class frmPrincipal
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
      this.tmTiempo = new System.Windows.Forms.Timer(this.components);
      this.lblContador = new System.Windows.Forms.Label();
      this.notificacionIcono = new System.Windows.Forms.NotifyIcon(this.components);
      this.SuspendLayout();
      // 
      // tmTiempo
      // 
      this.tmTiempo.Tick += new System.EventHandler(this.TmTiempo_Tick);
      // 
      // lblContador
      // 
      this.lblContador.AutoSize = true;
      this.lblContador.Location = new System.Drawing.Point(98, 96);
      this.lblContador.Name = "lblContador";
      this.lblContador.Size = new System.Drawing.Size(0, 13);
      this.lblContador.TabIndex = 0;
      // 
      // notificacionIcono
      // 
      this.notificacionIcono.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
      this.notificacionIcono.Icon = ((System.Drawing.Icon)(resources.GetObject("notificacionIcono.Icon")));
      this.notificacionIcono.Text = "Notificacion";
      this.notificacionIcono.Visible = true;
      this.notificacionIcono.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotificacionIcono_MouseClick);
      // 
      // frmPrincipal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(298, 264);
      this.Controls.Add(this.lblContador);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "frmPrincipal";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "RestEyes";
      this.Load += new System.EventHandler(this.FrmPrincipal_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer tmTiempo;
    private System.Windows.Forms.Label lblContador;
    private System.Windows.Forms.NotifyIcon notificacionIcono;
  }
}

