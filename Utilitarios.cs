using System;
using System.IO;
using System.Text;

namespace RestEyes
{
  public class Utilitarios
  {

    /// <summary>
    /// Oculta el archivo de texto que se guarda en la carpeta personal del usuario
    /// </summary>
    /// <param name="pLocalPath">Ruta de la carpeta personal del usuario, para buscar el archivo de texto plano</param>
    public static void OcultarArchivo(string pLocalPath)
    {
      try
      {
        bool atributoOculto = (File.GetAttributes(pLocalPath + "\\" + Recursos.NombreFile) & FileAttributes.Hidden) == FileAttributes.Hidden;
        if (atributoOculto == false)
          File.SetAttributes(pLocalPath + "\\" + Recursos.NombreFile, FileAttributes.Hidden);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Crea el archivo de texto plano en la carpeta personal del usuario, con las variables que se reciben, para los ajustes de la aplicacion
    /// </summary>
    /// <param name="pLocalPath">Ruta de la carpeta personal del usuario, para buscar el archivo de texto plano</param>
    /// <param name="pModo">Modo de ajuste para mostrar la notificacion de 20 minutos: 1 = modo discreto, 2 = modo alerta</param>
    /// <param name="pWindows">Modo de inicio de windows: OnWindows = inicia con windows, OffWindows = no inicia con windows</param>
    /// <returns></returns>
    public static string CrearArchivo(string pLocalPath, string pModo, string pWindows)
    {
      try
      {
        string variables = string.Empty;

        using (FileStream fs = File.Create(pLocalPath + "\\" + Recursos.NombreFile))
        {
          //Se crean los parametros a guardar en el archivo, para ser utilizados en la app y saber que configuracio usar
          //Primer campo: tipo de notificacion: 1 = modo discreto, 2 = modo alerta
          //Segundo campo: modo de inicio automatico en windows: OnWindows = incia automaticamente con windows, OffWindows = no inicia automaticamente con windows

          byte[] textoArchivo = new UTF8Encoding(true).GetBytes(pModo + ";" + pWindows);
          fs.Write(textoArchivo, 0, textoArchivo.Length);

          variables = Encoding.Default.GetString(textoArchivo);
        }

        return variables;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Lee todas las lineas del archivo de texto que se encuentra en la carpeta personal del usuario.
    /// </summary>
    /// <param name="pLocalPath">Ruta de la carpeta personal del usuario, para buscar el archivo de texto plano</param>
    /// <returns>Devuelve todo el texto del archivo leido, en una sola hilera</returns>
    public static string LeerArchivo(string pLocalPath)
    {
      try
      {
        string variables = string.Empty;

        using (StreamReader sr = File.OpenText(pLocalPath + "\\" + Recursos.NombreFile))
        {
          string textoArchivo = string.Empty;

          while ((textoArchivo = sr.ReadLine()) != null)
          {
            variables += textoArchivo;
          }
        }

        return variables;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Borra el archivo de la carpeta personal del usuario. Si el archivo existe (tiene contenido) lo borra, si el arreglo de bytes viene vacio,
    /// quiere decir que no se encontro ningun archivo, entonces no es necesario borrarlo. Siempre se retorna un string vacio, para que
    /// el archivo vuelva a crearse.
    /// </summary>
    /// <param name="pLocalPath">Ruta de la carpeta personal del usuario, para buscar el archivo de texto plano</param>
    /// <param name="pArchivo">Arreglo de bytes del contenido de archivo de texto plano</param>
    /// <returns></returns>
    public static string[] BorrarArchivo(string pLocalPath, string[] pArchivo)
    {
      try
      {
         if (pArchivo.Length != 0)
            File.Delete(pLocalPath + "\\" + Recursos.NombreFile);

        return new string[0];
      }
      catch (Exception)
      {
        throw;
      }
    } 

  }//FIN: Clase
}//FIN: nameSpace
