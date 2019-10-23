using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace transportAPI
{
    public partial class SiteMaster : MasterPage
    {
        public string destinationLat;
        public string destinationLon;
        public string startLat;
        public string startLon;
        private string destinationInput;
        private string startLocationInput;

        public class Address
        {
            public List<SearchAddress> results { get; set; }
        }

        public class SearchAddress
        {
            public string building { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //reminder: remove after prod. Retrieving input from textbox is working.
            //System.Diagnostics.Debug.WriteLine("Destination: "+destinationInput+ " Start: "+ startLocationInput);
        }

        private void GetAddress(string address)
        {
            //GET METHOD for destination search query
            string strurltest = String.Format("https://developers.onemap.sg/commonapi/search?searchVal=" + address + "&returnGeom=Y&getAddrDetails=Y&pageNum=");
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
                //System.Diagnostics.Debug.WriteLine(strresulttest);
                sr.Close();
            }

            //display search recommendations
            Address searchAddress = new JavaScriptSerializer().Deserialize<Address>(strresulttest);
            foreach (var item in searchAddress.results)
            {
                if (item.building == address.ToUpper() && address == destinationInput)
                {
  
                    destinationLat = item.latitude;
                    destinationLon = item.longitude;
                    //reminder: remove after prod. Data successfully passed to showEndPosition() JS function
                    System.Diagnostics.Debug.WriteLine("D Building name: " + item.building + "\n");
                    System.Diagnostics.Debug.WriteLine("Latitude: " + item.latitude + "\n");
                    System.Diagnostics.Debug.WriteLine("Longitude: " + item.longitude + "\n");
                }
               
                if(item.building == address.ToUpper() && address == startLocationInput)
                {
                    startLat = item.latitude;
                    startLon = item.longitude;
                    //reminder: remove after prod. Data successfully passed to showEndPosition() JS function
                    System.Diagnostics.Debug.WriteLine("S Building name: " + item.building + "\n");
                    System.Diagnostics.Debug.WriteLine("Latitude: " + item.latitude + "\n");
                    System.Diagnostics.Debug.WriteLine("Longitude: " + item.longitude + "\n");
                }
            }
        }


                                                                                                                                                                                                                                                                                                           
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            destinationInput = TextBox1.Text;
            startLocationInput = TextBox2.Text;
            GetAddress(destinationInput);
            GetAddress(startLocationInput);
        }
    }
}