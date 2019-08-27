using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace RestEyes
{
  public partial class frmPrincipal : Form
  {
    //Ruta para la llave que usa windows para iniciar la aplicacion en el inicio de windows
    RegistryKey directorioAppRegistro = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

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

        if (ConsultaIniciaWindows())
          Archivo(true, Recursos.ModoDiscreto, Recursos.ModoOnWindows); //Inicia en modo discreto y en modo de inciar con windows True
        else
          Archivo(true, Recursos.ModoDiscreto, Recursos.ModoOffWindows); //Inicia en modo discreto y en modo de inciar con windows False
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
          //Por defecto, la accion es mostrar el menu
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
        if (ConsultaIniciaWindows())
          Archivo(false, Recursos.ModoDiscreto, Recursos.ModoOnWindows);
        else
          Archivo(false, Recursos.ModoDiscreto, Recursos.ModoOffWindows);

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
        if (ConsultaIniciaWindows())
          Archivo(false, Recursos.ModoAlerta, Recursos.ModoOnWindows);
        else
          Archivo(false, Recursos.ModoAlerta, Recursos.ModoOffWindows);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void IniciarConWindows_Si(object sender, EventArgs e)
    {
      try
      {
        IniciarConWindows(true);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void IniciarConWindows_No(object sender, EventArgs e)
    {
      try
      {
        IniciarConWindows(false);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    #endregion FIN: Eventos


    #region Metodos

    /// <summary>
    /// Inicial el contador con intervales de 1000 milisegundos (1 segundo), para aumentar
    /// en valor "1" cada segundo al contador global.
    /// </summary>
    private void IniciarContador()
    {
      try
      {
        tmTiempo = new Timer();
        tmTiempo.Tick += new EventHandler(TmTiempo_Tick);
        tmTiempo.Interval = 1000; //en miliseconds
        tmTiempo.Start();
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Asigna el formato para mostrar los minutos con segundos en el globo de notificacion.
    /// Convierte la cantidad total de segundos recibidos, a minutos totales con segundos, separados por
    /// dos puntos en formato hora.
    /// </summary>
    /// <param name="intTiempo">Cantidad de segundos</param>
    /// <returns>Formato minutos:segundos, de los segundos totales recibidos</returns>
    private string ObtenerFormatoTiempo(int intTiempo)
    {
      try
      {
        TimeSpan spanTiempo = TimeSpan.FromSeconds(intTiempo);
        DateTime tiempoFormato = DateTime.Today.Add(spanTiempo);
        string tiempoFormatoCultura = tiempoFormato.ToString("mm:ss", CultureInfo.InvariantCulture);

        return tiempoFormatoCultura;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Carga las opciones principales del menu del tray icon y selecciona el primer item por defecto: Modo discreto.
    /// Se agregan separadores para dividir las secciones de las opciones.
    /// </summary>
    private void CargarMenu()
    {
      try
      {
        notificacionIcono.ContextMenuStrip = new ContextMenuStrip();

        notificacionIcono.ContextMenuStrip.Items.Add("Modo discreto", null, NotificacionIcon_ModoDiscreto);
        notificacionIcono.ContextMenuStrip.Items.Add("Modo alerta", null, NotificacionIcon_ModoAlerta);

        notificacionIcono.ContextMenuStrip.Items.Add(new ToolStripSeparator());

        notificacionIcono.ContextMenuStrip.Items.Add("Iniciar con windows", null, IniciarConWindows_Si);
        notificacionIcono.ContextMenuStrip.Items.Add("No iniciar con windows", null, IniciarConWindows_No);

        notificacionIcono.ContextMenuStrip.Items.Add(new ToolStripSeparator());

        notificacionIcono.ContextMenuStrip.Items.Add("Cerrar", null, NotificacionIcon_Cerrar);

        ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[0]).Checked = true;
        ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[3]).Checked = true;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Busca en la carpeta local de usuario el archivo con los ajustes de la configuracion, si no lo encuentra
    /// lo crea y pone la configuracion por defecto. Si ya existe, toma la configuracion del archivo para ajustarlo 
    /// a la aplicacion.
    /// </summary>
    /// <param name="pInicio">Carga desde el load o no: True = carga desde load, False = carga desde un metodo</param>
    /// <param name="pModo">Modo de alerta: 1 = discreto, 2 = alerta)</param>
    /// <param name="pWindows">Modo de inicio de windows: OnWindows, OffWindows</param>
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

          if (propiedadesAjuste[(int)Propiedad.TipoNotificacion] == Recursos.ModoDiscreto) //Modo discreto
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.ModoDiscreto]).Checked = true;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.ModoAlerta]).Checked = false;
          }
          else if (propiedadesAjuste[(int)Propiedad.TipoNotificacion] == Recursos.ModoAlerta)//Modo alerta
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.ModoAlerta]).Checked = true;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.ModoDiscreto]).Checked = false;
          }

          if (propiedadesAjuste[(int)Propiedad.InicioWindows] == Recursos.ModoOnWindows)
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.SiIniciarConWindows]).Checked = true;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.NoIniciarConWindows]).Checked = false;
          }
          else if (propiedadesAjuste[(int)Propiedad.InicioWindows] == Recursos.ModoOffWindows)
          {
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.NoIniciarConWindows]).Checked = false;
            ((ToolStripMenuItem)notificacionIcono.ContextMenuStrip.Items[(int)Opcion.SiIniciarConWindows]).Checked = true;
          }

        }//FIN if: leer variables del archivo para ajustarlos al sistema

        Utilitarios.OcultarArchivo(localPath);
      }
      catch (Exception)
      {
        throw;
      }
    }//FIN: Clase Archivo

    /// <summary>
    /// Consulta si la aplicacion esta actualmente iniciando con windows o no.
    /// </summary>
    /// <returns>Si inicia con windows retorna true, sino, retorna false</returns>
    private bool ConsultaIniciaWindows()
    {
      try
      {
        bool inicia = false;

        //Consulta si la app actualmente inicia con windows
        if (directorioAppRegistro.GetValue(Recursos.NombreApp) != null)
          inicia = true;

        return inicia;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Agrega el nombre de la app a la llave en el registro de windows o lo remueve, para iniciar con windows
    /// o no inciar con windows, dependiendo del parametro que recibe el metodo.
    /// </summary>
    /// <param name="activador">Si recibe true: crea la llave para iniciar con windows, si recibe false: elimina la llave de inicio de windows</param>
    private void IniciarConWindows(bool activador)
    {
      try
      {
        if (activador)
        {
          if (ConsultaIniciaWindows() == false)
            //Agrega el valor al registro de windows, para iniciar la app con windows
            directorioAppRegistro.SetValue(Recursos.NombreApp, Application.ExecutablePath);
        }
        else
          //Agrega el valor al registro de windows, para iniciar la app con windows
          directorioAppRegistro.DeleteValue(Recursos.NombreApp, false);
      }
      catch (Exception)
      {
        throw;
      }
    }

    #endregion FIN: Metodos

    #region Enums
    enum Opcion
    {
      ModoDiscreto = 0,
      ModoAlerta = 1,
      SiIniciarConWindows = 3,
      NoIniciarConWindows = 4,
      Cerrar = 6
    }

    enum Propiedad
    {
      TipoNotificacion = 0,
      InicioWindows = 1
    }
    #endregion FIN: Enums

  }//FIN: Clase
}//FIN: NameSpace
