using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using System.Threading.Tasks;

namespace ACEOMM2
{
    [Serializable]
    public enum Businessclass
    {
        cheap,
        small,
        medium,
        big,
        exclusive
    }

    [Serializable]
    public enum BusinessType
    { 
        all,
        contractor,
        avFuelSupplier,
        bank,
        airline,
        franchise,
        catering,
    }

    [Serializable]
    public class Business 
    {
        public string name;
        //public string id;
        //public string countryCode;
        public string description;
        public string logoPath = "";
        public Businessclass businessClass;
        [NonSerialized]
        public string countryCode;
        public BusinessType businessType;
        public string CEOName;
        public string Author;
        [NonSerialized]
        public string imgURL;

        public Business(string _name, string _id, string _countryCode, string _description, string _CEO, string _businessclass, string _type, string _imgURL)
        {
            name = _name;
            // id = _id;
            countryCode = _countryCode;
            description = _description;
            CEOName = _CEO;
            businessClass = BusinessClass(_businessclass);
            businessType = BusinessType(_type);
            imgURL = _imgURL;
        }

        /// <summary>
        /// Gets the Businessclass based on an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Businessclass BusinessClass(string input)
        {
            switch (input)
            {
                case "Small":
                    return Businessclass.small;
                    break;
                case "Medium":
                    return Businessclass.medium;
                    break;
                case "Cheap":
                    return Businessclass.cheap;
                    break;
                case "Big":
                    return Businessclass.big;
                    break;
                case "Exclusive":
                    return Businessclass.exclusive;
                    break;
                default:
                    Debug.LogWarning("Invalid business class" + input);
                    return Businessclass.medium;
            }
        }

        /// <summary>
        /// Gets the BusinessType based on an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public BusinessType BusinessType(string input)
        {
            switch (input)
            {
                case "Bank":
                    return ACEOMM2.BusinessType.bank;
                    break;
                case "AVFuelSupplier":
                    return ACEOMM2.BusinessType.avFuelSupplier;
                    break;
                case "Contractor":
                    return ACEOMM2.BusinessType.contractor;
                    break;
                case "Catering":
                    return ACEOMM2.BusinessType.catering;
                    break;
                case "Airline":
                    return ACEOMM2.BusinessType.airline;
                    break;
                default:
                    Debug.LogWarning("Invalid Business Type " + input);
                    return ACEOMM2.BusinessType.catering;
            }
        }

        public string MakeJSONString()
        {
            return JsonUtility.ToJson(this);
        }
        
        /// <summary>
        /// Writes a business to a .json file, and installs its logo
        /// </summary>
        /// <param name="location"> install location folder ("Banks")</param>
        public async void WriteToFile(string location)
        {
            Debug.Log("Installing " + name);
            System.IO.Directory.CreateDirectory(Path.Combine(location , name ));
            try
            {
                DownloadLogo(location);
            }
            catch (Exception)
            {

                throw;
            }



            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(location, name, name + ".json")))
            {
                file.Write(MakeJSONString());
            }


        }

        /// <summary>
        /// used for downloading the logo from a web address, as specified in the spreadsheet
        /// </summary>
        /// <param name="location"> location to install the logo</param>
        public async Task<bool> DownloadLogo(string location)
        {
            Task<bool> t = DownloadFile(Path.Combine(location, name) + Path.DirectorySeparatorChar + name + ".png", imgURL);
            bool completion = await t;
            logoPath = name + ".png";
            return completion;
        }

        /// <summary>
        /// Generic image downloading function
        /// </summary>
        /// <param name="_installLocation">The COMPLETE path to install the image, including .png</param>
        /// <param name="_Url"> source url </param>
        public async Task<bool> DownloadFile(string _installLocation,string _Url )
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    Debug.Log(_Url);
                    client.DownloadFileAsync(new Uri(_Url), _installLocation);
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("image download from" + _Url + "failed.");
                    return false;
                    throw;
                }
            }
        }

    }
}