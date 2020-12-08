<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;
using LitJson;
using web_trazabilidad.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;


public class FileUploadHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string UrlFileUpload = Cls_Config.UrlFileUpload;
        Cls_Mensaje Mensaje = new Cls_Mensaje();
        string json_resultado = "{}";
        try
        {
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;

                string nombre = context.Request.Params[0];
                string url = context.Request.Params[1];


                if (url.Contains(UrlFileUpload))
                {
                    url = url.Replace(UrlFileUpload, "");
                }


                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];

                    if (!Directory.Exists(context.Server.MapPath(url)))
                        Directory.CreateDirectory(context.Server.MapPath(url));

                    if (string.IsNullOrEmpty(Path.GetExtension(nombre)))
                    {
                        nombre = nombre + Path.GetExtension(file.FileName);
                    }

                    string fname = context.Server.MapPath(url + "/" + nombre);

                    //Si es imagen
                    string extension = System.IO.Path.GetExtension(nombre);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        System.Drawing.Image image = Image.FromStream(file.InputStream);
                        System.Drawing.Bitmap imagen = new System.Drawing.Bitmap(image);
                        imagen.Save(fname);
                    }
                    else
                    {
                        file.SaveAs(fname);
                    }
                }
                Mensaje.ID = Path.GetExtension(files[0].FileName);
                Mensaje.Titulo = nombre;
                Mensaje.Estatus = "success";
                Mensaje.Mensaje = "File(s) Uploaded Successfully!";
            }
            else
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "No se encontraron archivos";
            }
        }
        catch (Exception ex)
        {
            Mensaje.Estatus = "error";
            Mensaje.Mensaje = ex.Message;
        }
        finally
        {
            json_resultado = JsonMapper.ToJson(Mensaje);
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write(json_resultado);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}