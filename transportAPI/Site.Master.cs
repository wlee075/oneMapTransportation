﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using static transportAPI.SiteMaster;

namespace transportAPI
{
    public partial class SiteMaster : MasterPage
    {
        //route coordinates
        public string[] routeCoo;
        public string destinationLat;
        public string destinationLon;
        public string startLat;
        public string startLon;
        private string destinationInput;
        private string startLocationInput;
        private string transportType;
        //reminder: token expires after 3 days. dont forget to request new one.
        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjM0MzYsInVzZXJfaWQiOjM0MzYsImVtYWlsIjoid2xlZTA3NUBlLm50dS5lZHUuc2ciLCJmb3JldmVyIjpmYWxzZSwiaXNzIjoiaHR0cDpcL1wvb20yLmRmZS5vbmVtYXAuc2dcL2FwaVwvdjJcL3VzZXJcL3Nlc3Npb24iLCJpYXQiOjE1NzE5MzE0NjksImV4cCI6MTU3MjM2MzQ2OSwibmJmIjoxNTcxOTMxNDY5LCJqdGkiOiJmMDRlMWNlMzVlZTFiMmNlOGMzNDE0N2UxNjFhYzZhMiJ9.BradZOMhi5tEdYs_1SYCdSyd5ZKswqumkjM4SlUoWIo";
        public string route_geometry;

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


        public class PublicTransport
        {
            public Requestparameters requestParameters { get; set; }
            public Plan plan { get; set; }
            public Debugoutput debugOutput { get; set; }
            public Elevationmetadata elevationMetadata { get; set; }
        }

        public class Requestparameters
        {
            public string date { get; set; }
            public string preferredRoutes { get; set; }
            public string walkReluctance { get; set; }
            public string fromPlace { get; set; }
            public string transferPenalty { get; set; }
            public string maxWalkDistance { get; set; }
            public string maxTransfers { get; set; }
            public string otherThanPreferredRoutesPenalty { get; set; }
            public string numItineraries { get; set; }
            public string waitAtBeginningFactor { get; set; }
            public string mode { get; set; }
            public string arriveBy { get; set; }
            public string showIntermediateStops { get; set; }
            public string toPlace { get; set; }
            public string time { get; set; }
        }

        public class Plan
        {
            public long date { get; set; }
            public From from { get; set; }
            public To to { get; set; }
            public Itinerary[] itineraries { get; set; }
        }

        public class From
        {
            public string name { get; set; }
            public float lon { get; set; }
            public float lat { get; set; }
            public string orig { get; set; }
            public string vertexType { get; set; }
        }

        public class To
        {
            public string name { get; set; }
            public float lon { get; set; }
            public float lat { get; set; }
            public string orig { get; set; }
            public string vertexType { get; set; }
        }

        public class Itinerary
        {
            public int duration { get; set; }
            public long startTime { get; set; }
            public long endTime { get; set; }
            public int walkTime { get; set; }
            public int transitTime { get; set; }
            public int waitingTime { get; set; }
            public float walkDistance { get; set; }
            public bool walkLimitExceeded { get; set; }
            public int elevationLost { get; set; }
            public int elevationGained { get; set; }
            public int transfers { get; set; }
            public Leg[] legs { get; set; }
            public bool tooSloped { get; set; }
            public string fare { get; set; }
        }

        public class Leg
        {
            public long startTime { get; set; }
            public long endTime { get; set; }
            public int departureDelay { get; set; }
            public int arrivalDelay { get; set; }
            public bool realTime { get; set; }
            public float distance { get; set; }
            public bool pathway { get; set; }
            public string mode { get; set; }
            public string route { get; set; }
            public int agencyTimeZoneOffset { get; set; }
            public bool interlineWithPreviousLeg { get; set; }
            public From1 from { get; set; }
            public To1 to { get; set; }
            public Leggeometry legGeometry { get; set; }
            public bool rentedBike { get; set; }
            public bool transitLeg { get; set; }
            public int duration { get; set; }
            public object[] intermediateStops { get; set; }
            public Step[] steps { get; set; }
            public int numIntermediateStops { get; set; }
            public string agencyName { get; set; }
            public string agencyUrl { get; set; }
            public int routeType { get; set; }
            public string routeId { get; set; }
            public string agencyId { get; set; }
            public string tripId { get; set; }
            public string serviceDate { get; set; }
            public string routeShortName { get; set; }
            public string routeLongName { get; set; }
        }

        public class From1
        {
            public string name { get; set; }
            public float lon { get; set; }
            public float lat { get; set; }
            public long departure { get; set; }
            public string orig { get; set; }
            public string vertexType { get; set; }
            public long arrival { get; set; }
            public string stopId { get; set; }
            public int stopIndex { get; set; }
            public int stopSequence { get; set; }
        }

        public class To1
        {
            public string name { get; set; }
            public float lon { get; set; }
            public float lat { get; set; }
            public long arrival { get; set; }
            public long departure { get; set; }
            public string vertexType { get; set; }
            public string stopId { get; set; }
            public int stopIndex { get; set; }
            public int stopSequence { get; set; }
            public string orig { get; set; }
        }

        public class Leggeometry
        {
            public string points { get; set; }
            public int length { get; set; }
        }

        public class Step
        {
            public float distance { get; set; }
            public string relativeDirection { get; set; }
            public string streetName { get; set; }
            public string absoluteDirection { get; set; }
            public bool stayOn { get; set; }
            public bool area { get; set; }
            public bool bogusName { get; set; }
            public float lon { get; set; }
            public float lat { get; set; }
            public object[] elevation { get; set; }
        }

        public class Debugoutput
        {
            public int precalculationTime { get; set; }
            public int pathCalculationTime { get; set; }
            public int[] pathTimes { get; set; }
            public int renderingTime { get; set; }
            public int totalTime { get; set; }
            public bool timedOut { get; set; }
        }

        public class Elevationmetadata
        {
            public float ellipsoidToGeoidDifference { get; set; }
            public bool geoidElevation { get; set; }
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

                if (item.building == address.ToUpper() && address == startLocationInput)
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
            transportType = selectedValue;
        }

        //GET METHOD for route query
        private void GetInstructions()
        {
            if (transportType == "pt")
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                var date = DateTime.Today.ToString("yyyy-MM-dd");
                string strurltest = String.Format("https://developers.onemap.sg/privateapi/routingsvc/route?start=" +
                         startLat + "," + startLon + "&end=" + destinationLat + "," + destinationLon + "&" +
                         "routeType=" + transportType + "&token=" + token + "&date=" + date + "&time=" + time + "&mode=TRANSIT&maxWalkDistance=1000&numItineraries=1");
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
                PublicTransport route = new JavaScriptSerializer().Deserialize<PublicTransport>(strresulttest);
                //display route instructions

                foreach (var item in route.plan.itineraries)
                {
                    int i = 0;
                    routeCoo = new string[item.legs.Count()*2];
                    
                    foreach (var leg in item.legs) //ITINERARY
                    {                        
                        routeCoo[i] = leg.from.lat.ToString() + "," + leg.from.lon.ToString();
                        ++i;
                        routeCoo[i] = leg.to.lat.ToString() + "," + leg.to.lon.ToString();
                        i++;
                        foreach (var steps in leg.steps)
                        {
                            TextBox3.Text = TextBox3.Text + Environment.NewLine + leg.mode + " " + steps.absoluteDirection + " FOR "+ leg.distance+"METRES";
                        }
                        //System.Diagnostics.Debug.WriteLine("FROM: Lat: "+leg.from.lat + " Lon:" + leg.from.lon + " Name:"+ leg.from.name +"\n");
                        //System.Diagnostics.Debug.WriteLine("TO: " + leg.to.lat + " " + leg.to.lon + "\n");
                    }
                }
            }
            else if (transportType == "drive" || transportType == "walk")
            {
                string strurltest = String.Format("https://developers.onemap.sg/privateapi/routingsvc/route?start=" +
                            startLat + "," + startLon + "&end=" + destinationLat + "," + destinationLon + "&" +
                            "routeType=" + transportType + "&token=" + token);
                WebRequest requestObjGet = WebRequest.Create(strurltest);
                requestObjGet.Method = "GET";
                HttpWebResponse responseObjGet = null;
                responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();
                string strresulttest = null;
                using (Stream stream = responseObjGet.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    strresulttest = sr.ReadToEnd();
                    sr.Close();
                }

                Route route = new JavaScriptSerializer().Deserialize<Route>(strresulttest);
                route_geometry = route.route_geometry;
                //display route instructions
                routeCoo = new string[route.route_instructions.Count];
                int i = 0;
                foreach (var item in route.route_instructions)
                {
                    routeCoo[i] = item[3].ToString();
                    i++;
                    TextBox3.Text = TextBox3.Text + Environment.NewLine + item[9];

                }
            }
        }

    }

}