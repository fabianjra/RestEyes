using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace RestEyes
{
  public partial class frmPrincipal : Form
  {
    private int intTiempo = 0;

    public frmPrincipal()
    {
      InitializeComponent();
      CargarMenu();
    }

    #region Eventos
    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
      try
      {
        IniciarContador();
        this.Opacity = 0.0;

        Archivo(true, Recursos.ModoDiscreto, Recursos.ModoOnWindows); //Inicia en modo discreto y en modo de inciar con windows
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void TmTiempo_Tick(object sender, EventArgs e)
    {
      try
      {
        intTiempo += 1;
        string tiempoFormatoCultura = ObtenerFormatoTiempo(intTiempo);
        lblContador.Text = tiempoFormatoCultura;

        //Cuando se cumplen los 20 minutos, se mostrara la alerta de ver hacia otro lado (tiempo en segundos 1200)
        if (intTiempo == 1200)
        {
          tmTiempo.Stop();
          lblContador.Text = "0";
          intTiempo = 0;

          if (((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[0]).Checked) //Modo discreto
            notificacionIcono.ShowBalloonTip(5, "Tiempo Finalizado", "Refresque su vista por 20 segundos", ToolTipIcon.Warning);

          else if (((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[1]).Checked) //Modo alerta
            MessageBox.Show("Refresque su vista por 20 segundos", "Tiempo Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

          IniciarContador();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void NotificacionIcono_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        if (e.Button == MouseButtons.Left) //Click izquierdo al icono en el tray
        {
          string tiempoFormatoCultura = ObtenerFormatoTiempo(intTiempo);

          notificacionIcono.ShowBalloonTip(1, "Tiempo transcurrido", tiempoFormatoCultura, ToolTipIcon.Info);
        }
        else if (e.Button == MouseButtons.Right) //Click derecho al icono en el tray
        {

        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void NotificacionIcon_Cerrar(object sender, EventArgs e)
    {
      try
      {
        Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void NotificacionIcon_ModoDiscreto(object sender, EventArgs e)
    {
      try
      {
        Archivo(false, Recursos.ModoDiscreto, Recursos.ModoOnWindows);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void NotificacionIcon_ModoAlerta(object sender, EventArgs e)
    {
      try
      {
        Archivo(false, Recursos.ModoAlerta, Recursos.ModoOnWindows);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    #endregion FIN: Eventos


    #region Metodos
    private void IniciarContador()
    {
      tmTiempo = new Timer();
      tmTiempo.Tick += new EventHandler(TmTiempo_Tick);
      tmTiempo.Interval = 1000; // en miliseconds
      tmTiempo.Start();
    }

    private string ObtenerFormatoTiempo(int intTiempo)
    {
      TimeSpan spanTiempo = TimeSpan.FromSeconds(intTiempo);
      DateTime tiempoFormato = DateTime.Today.Add(spanTiempo);
      string tiempoFormatoCultura = tiempoFormato.ToString("mm:ss", CultureInfo.InvariantCulture);

      return tiempoFormatoCultura;
    }

    private void CargarMenu()
    {
      notificacionIcono.ContextMenuStrip = new ContextMenuStrip();

      notificacionIcono.ContextMenuStrip.Items.Add("Modo discreto", null, NotificacionIcon_ModoDiscreto);
      notificacionIcono.ContextMenuStrip.Items.Add("Modo alerta", null, NotificacionIcon_ModoAlerta);
      notificacionIcono.ContextMenuStrip.Items.Add("Cerrar", null, NotificacionIcon_Cerrar);

      ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[0]).Checked = true;
    }

    /// <summary>
    /// Crea el archivo o lo lee, para ajustar la configuracion seleccionada en las opciones
    /// </summary>
    /// <param name="pInicio">Carga desde el load o no: True = carga desde load, False = carga desde un metodo</param>
    /// <param name="pModo">Modo de alerta: 1 = discreto, 2 = alerta)</param>
    /// <param name="pWindows">Modo de inicio de windows: OnWindows, OffWindows</param>
    /// <returns></returns>
    private void Archivo(bool pInicio, string pModo, string pWindows)
    {
      try
      {
        string variables = string.Empty;

        string localPath = System.Environment.GetEnvironmentVariable("USERPROFILE"); //Obtiene el directorio del usuario actual
        string[] archivo = Directory.GetFiles(localPath, Recursos.NombreFile); //Busca el archivo de configuracion, en el directorio

        //Si se ejecuta este metodo, desde un click de opcion, se debe borrar el archivo y volver a crear
        if (pInicio == false)
          archivo = Utilitarios.BorrarArchivo(localPath, archivo);


        if (archivo.Length == 0) //No existe el archivo, se debe crear
        {
          variables = Utilitarios.CrearArchivo(localPath, pModo, pWindows);
        }
        else//Leer archivo existente
        {
          variables = Utilitarios.LeerArchivo(localPath);
        }

        //Ajustar parametros de la aplicacion, en dependencia de la seleccion del archivo de texto
        if (string.IsNullOrEmpty(variables?.Trim()) == false)
        {
          //Primer campo: tipo de notificacion: 1 = modo discreto, 2 = modo alerta
          //Segundo campo: modo de inicio automatico en windows: OnWindows = incia automaticamente con windows, OffWindows = no inicia automaticamente con windows
          string[] propiedadesAjuste = variables.Split(';');

          if (propiedadesAjuste[0] == Recursos.ModoDiscreto) //Modo discreto
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[0]).Checked = true;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[1]).Checked = false;
          }
          else if (propiedadesAjuste[0] == Recursos.ModoAlerta)//Modo alerta
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[1]).Checked = true;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[0]).Checked = false;
          }
           
          if (propiedadesAjuste[1] == Recursos.ModoOnWindows)
          {
            //Metodo para iniciar con windows
          }
          else if (propiedadesAjuste[1] == Recursos.ModoOffWindows)
          {
            //Metodo para NO iniciar con windows
          }

        }//FIN if: leer variables del archivo para ajustarlos al sistema

        Utilitarios.OcultarArchivo(localPath);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }//FIN: Clase Archivo

    #endregion FIN: Metodos


  }//FIN: Clase
}//FIN: NameSpace
