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
        private string transportType;
        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjM0MzYsInVzZXJfaWQiOjM0MzYsImVtYWlsIjoid2xlZTA3NUBlLm50dS5lZHUuc2ciLCJmb3JldmVyIjpmYWxzZSwiaXNzIjoiaHR0cDpcL1wvb20yLmRmZS5vbmVtYXAuc2dcL2FwaVwvdjJcL3VzZXJcL3Nlc3Npb24iLCJpYXQiOjE1NzE5MzE0NjksImV4cCI6MTU3MjM2MzQ2OSwibmJmIjoxNTcxOTMxNDY5LCJqdGkiOiJmMDRlMWNlMzVlZTFiMmNlOGMzNDE0N2UxNjFhYzZhMiJ9.BradZOMhi5tEdYs_1SYCdSyd5ZKswqumkjM4SlUoWIo";

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

        public class Route
        {
            public string status_message { get; set; }
            public string route_geometry { get; set; }
            public int status { get; set; }
            public List<List<object>> route_instructions { get; set; }
            public List<string> route_name { get; set; }
            public RouteSummary route_summary { get; set; }
            public string viaRoute { get; set; }
            public string subtitle { get; set; }
            public Phyroute phyroute { get; set; }
        }

        public class RouteSummary
        {
            public string start_point { get; set; }
            public string end_point { get; set; }
            public int total_time { get; set; }
            public int total_distance { get; set; }
        }

        public class Phyroute
        {
            public string status_message { get; set; }
            public string route_geometry { get; set; }
            public int status { get; set; }
            public List<List<object>> route_instructions { get; set; }
            public List<string> route_name { get; set; }
            public RouteSummary route_summary { get; set; }
            public string viaRoute { get; set; }
            public string subtitle { get; set; }
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
                    break;
                }
               
                if(item.building == address.ToUpper() && address == startLocationInput)
                {
                    startLat = item.latitude;
                    startLon = item.longitude;
                    //reminder: remove after prod. Data successfully passed to showEndPosition() JS function
                    System.Diagnostics.Debug.WriteLine("S Building name: " + item.building + "\n");
                    System.Diagnostics.Debug.WriteLine("Latitude: " + startLat + "\n");
                    System.Diagnostics.Debug.WriteLine("Longitude: " + startLon + "\n");
                    break;
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
            GetInstructions();
        }

        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TptOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = TptOptions.SelectedItem.Text;
            //System.Diagnostics.Debug.WriteLine(selectedValue);
            transportType = selectedValue;
        }

        private void GetInstructions()
        {
            //GET METHOD for route query
            string strurltest = String.Format("https://developers.onemap.sg/privateapi/routingsvc/route?start="+
                startLat+","+ startLon +"&end="+ destinationLat +","+ destinationLon+"&"+
                "routeType="+ transportType + "&token="+token);
            System.Diagnostics.Debug.WriteLine(strurltest);
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

            Route route = new JavaScriptSerializer().Deserialize<Route>(strresulttest);
            //display route instructions
            foreach (var item in route.route_instructions)
            {
                System.Diagnostics.Debug.WriteLine(item[9] + "\n");
                TextBox3.Text = TextBox3.Text + Environment.NewLine + item[9];
            }
        }

    }
}