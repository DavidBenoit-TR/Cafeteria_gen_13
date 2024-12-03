using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

namespace Cafetería_gen_13
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        //declaro una variable global
        public string tipomenu = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "Hola desde el CodeBehind";

            //Recupero una variable que viene desde la URL, para ello valido si existe una variable llamda "Id" dentro de la URL, si no existe, pasamos un 0 como argumento, de lo contrario tomo su valor
            if (Request.QueryString["Id"] == null)
            {
                tipomenu = "0";
            }
            else
            {
                tipomenu = Request.QueryString["Id"];
            }

            TransformarXML();
        }

        //método para cargar y transformar un XML usando XSLT
        private void TransformarXML()
        {
            //recuperamos las rutas de nuestros XML y XSLT
            string xmlPath = ConfigurationManager.AppSettings["FileServer"].ToString() + "xml/menu.xml";
            string xsltPath = ConfigurationManager.AppSettings["FileServer"].ToString() + "xslt/xsltFile.xslt";

            //leer el archivo XML (importamos "using System.Xml;")
            XmlTextReader xmlTextReader = new XmlTextReader(xmlPath);

            //Configuramos las credenciales para resolver recursos externos como el XSLT
            XmlUrlResolver xmlUrlResolver = new XmlUrlResolver();
            xmlUrlResolver.Credentials = CredentialCache.DefaultCredentials;

            //crear las configuraciones para poder acceder al XSLT
            //los parámetros "true" sirven para
            //1. tratar el XSLT como documento que puede ser transformado
            //2. Permitir la ejecución de bloques de código JS como resultado de la trasnformación
            XsltSettings settings = new XsltSettings(true, true);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltPath, settings, xmlUrlResolver);

            //creamos un StringBuilder para almacenar el resultado de la transformación
            StringBuilder stringBuilder = new StringBuilder();
            TextWriter textWriter = new StringWriter(stringBuilder);

            //configuramos los argumentos para la transformación
            XsltArgumentList xsltArgumentList = new XsltArgumentList();
            xsltArgumentList.AddParam("TipoMenu", "", tipomenu);

            //transformamos el XML => HTML usando XSLT
            xslt.Transform(xmlTextReader, xsltArgumentList, textWriter);

            //obtenemos el resultado de la transformación como 1 sola cadena
            string resultado = stringBuilder.ToString();

            //escribimos el resultado como una respuesta HTTP
            Response.Write(resultado);
        }
    }
}