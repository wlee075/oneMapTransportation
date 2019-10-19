using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace transportAPI
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strurltest = String.Format("https://jsonplaceholder.typicode.com/posts/1");
            WebRequest requestObjGet = WebRequest.Create(strurltest);
            requestObjGet.Method = "GET";
            HttpWebResponse responseObjGet = null;
            responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();
            string strresulttest = null;
            using (Stream stream = responseObjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                strresulttest = sr.ReadToEnd();
                //reminder: remove after prod. GET is working.
                System.Diagnostics.Debug.WriteLine(strresulttest);
                sr.Close();
            }
        }
    }
}