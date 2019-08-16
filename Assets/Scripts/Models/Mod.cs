using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public string installMod(string location)
        {
            //Create all necessary folders
            Directory.CreateDirectory(Path.Combine(location ,name));
            Directory.CreateDirectory(Path.Combine(location, name , "Companies"));
            Directory.CreateDirectory(Path.Combine(location, name , "Companies" , "Banks"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "AVFuelSuppliers"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "Contractors"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "Catering"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "Deicing"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "ShopFranchises"));
            Directory.CreateDirectory(Path.Combine(location, name, "Companies", "FoodFranchises"));
            Directory.CreateDirectory(Path.Combine(location, name, "Products"));
            BusinessInstallFolder = Path.Combine(location, name , "Companies");
            Controller.instance.modPacks[0].products.serializeProducts(Path.Combine(Controller.instance.productsFolder, name));
            using (StreamWriter file = new StreamWriter(Path.Combine(location, name, "modData.json")))
            {
                file.Write(JsonUtility.ToJson(this));
            }
           
            InstallBusinessByType();
            //try to reset the selection of businesses; this is very messy and needs refactoring
            foreach (Business business in businesses)
            {
                var keys = from entry in GameObject.FindObjectOfType<UIController>().GameobjectBusinessDatabase
                           where entry.Value == business
                           select entry.Key;
                GameObject gm = keys.First<GameObject>();
                gm.GetComponent<BusinessUI>().selectedToggle.enabled = false;
                gm.GetComponent<BusinessUI>().OnToggle(false);

            }
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
                        Franchise b = (Franchise)business;
                        if (b.franchiseType == "Shop")
                        {
                            b.WriteToFile(Path.Combine(BusinessInstallFolder, "ShopFranchises"));
                        }
                        else
                        {
                            b.WriteToFile(Path.Combine(BusinessInstallFolder, "FoodFranchises"));
                        }
                        break;
                    case BusinessType.deicing:
                        business.WriteToFile(Path.Combine(BusinessInstallFolder, "Deicing"));
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