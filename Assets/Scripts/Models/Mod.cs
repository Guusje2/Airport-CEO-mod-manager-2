using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ACEOMM2
{
    [Serializable]
    public class Mod
    {
        private string BusinessInstallFolder;
        public string id = "0";
        public string publishedFileID = "0";
        public string name;
        public string description;
        public string author;
        public string version;
        public string iconFileName = "Icon.png";
        public string modType = "";
        public string[] tags = { "" };
        private List<Business> businesses;
        

        public Mod(string _name, string _description, string _author, string _version)
        {
            name = _name;
            description = _description;
            author = _author;
            version = _version;
            businesses = new List<Business>();
        }

        /// <summary>
        /// returns the mod main folder
        /// </summary>
        /// <param name="location">ends with a slash</param>
        /// <returns>mod main folder</returns>
        public string CreateModDataJSON(string location)
        {
            System.IO.Directory.CreateDirectory(Path.Combine(location ,name));
            System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(location, name) , "Companies"));
            System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(location, name) , "Companies" , "Banks"));
            System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(location, name), "Companies", "AVFuelSuppliers"));
            System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(location, name), "Companies", "Contractors"));
            System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(location, name), "Companies", "Catering"));
            BusinessInstallFolder = Path.Combine(Path.Combine(location, name) , "Companies");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(location, name) + @"\modData.json"))
            {
                file.Write(JsonUtility.ToJson(this));
            }
            InstallBusinessByType();
            return (location + name);
        }

        public void AddBusiness (Business b)
        {
            businesses.Add(b);
        }

        public void RemoveBusiness(Business b)
        {
            businesses.Remove(b);
        }

        public void InstallBusinessByType()
        {
            Debug.Log("InstallBusinessByType");
            foreach (Business business in businesses)
            {
                
                switch (business.businessType)
                {
                    case BusinessType.all:
                        break;
                    case BusinessType.contractor:
                        business.WriteToFile(Path.Combine(BusinessInstallFolder, "Contractors"));
                        break;
                    case BusinessType.avFuelSupplier:
                        business.WriteToFile(Path.Combine(BusinessInstallFolder, "AVFuelSuppliers"));
                        break;
                    case BusinessType.bank:
                        business.WriteToFile(Path.Combine(BusinessInstallFolder , "Banks"));
                        break;
                    case BusinessType.airline:
                        Airline a = (Airline)business;
                        a.WriteToFile(Path.Combine(BusinessInstallFolder, "Airlines"));
                        break;
                    case BusinessType.franchise:
                        break;
                    case BusinessType.catering:
                        business.WriteToFile(Path.Combine(BusinessInstallFolder, "Catering"));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}