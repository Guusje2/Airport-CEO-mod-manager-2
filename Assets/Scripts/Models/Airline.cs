using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACEOMM2;
using UnityEngine;

namespace ACEOMM2
{
    public class Airline : Business
    {
        //[NonSerialized]
        public List<Livery> liveries;
        public string flightPrefix;
        public string invLogo;
        public List<string> fleet;
        public bool isCustom = true;
        public Airline(string _name, string _id, string _countryCode, string _description, string _CEO, string _businessclass, string _type, string imgURL, string _flightPrefix, string _author) : base(_name, _id, _countryCode, _description, _CEO, _businessclass, _type, imgURL, _author)
        {
            name = _name;
            // id = _id;
            countryCode = _countryCode;
            description = _description;
            CEOName = _CEO;
            businessClass = BusinessClass(_businessclass);
            businessType = BusinessType(_type);
            logoPath = imgURL;
            flightPrefix = _flightPrefix;
            liveries = new List<Livery>();
            Author = _author;
        }

        public new void WriteToFile (string location)
        {
            //Debug.Log("Installing " + name);
            System.IO.Directory.CreateDirectory(Path.Combine(location, name));
            try
            {
                DownloadLogo(location);
            }
            catch (Exception)
            {

                throw;
            }



            using (StreamWriter file = new StreamWriter(Path.Combine(location, name, name + ".json")))
            {
                file.Write(MakeJSONString());
            }
            //Debug.Log("Liveries count: " + liveries.Count);
            foreach (Livery livery in liveries)
            {
           
                Directory.CreateDirectory(Path.Combine(location, name, livery.Aircraft));
                DownloadFile(Path.Combine(location, name, livery.Aircraft, "liveryData.json"), livery.JsonLink);
                DownloadFile(Path.Combine(location, name, livery.Aircraft, livery.Aircraft + "_Mod.png"), livery.ImageLink);
               // Debug.Log("Installing " + livery.Airline + "|" + livery.Aircraft + " to " + Path.Combine(location, name, livery.Aircraft));
            }

        }

        public string MakeJSONString()
        {
            //Debug.Log("MakeJSONSTRING");
            foreach (Livery livery in liveries)
            {
                if(fleet == null)
                {
                    fleet = new List<string>(); 
                    fleet.Add(livery.Aircraft);
                    //Debug.Log("Added " + livery.Aircraft + " to the fleet of " + name);
                    continue;
                }
                if ( !fleet.Contains(livery.Aircraft))
                {
                    fleet.Add(livery.Aircraft);
                    //Debug.Log("Added " + livery.Aircraft + " to the fleet of " + name);
                }
            }
            return JsonUtility.ToJson(this);
        }
    }

    public class Livery
    {
        public string Aircraft;
        public string Author;
        public string Airline;
        public string ImageLink;
        public string JsonLink;

        public Livery (string _Aircraft, string _Author, string _Airline, string _Imagelink, string _JsonLink)
        {
            Aircraft = _Aircraft;
            Author = _Author;
            Airline = _Airline;
            ImageLink = _Imagelink;
            JsonLink = _JsonLink;
        }
    }
}
