using ACEOMM2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACEOMM2
{
    [Serializable]
    public class Database
    {
        
            public List<Business> businesses;
            public Dictionary<string, Airline> Airlines;
            public ProductsContainer products;
            public string version;
            public string name;
            public string description;
            public string author;
        public string businessSheetName;
        public string liveriesSheetName;
        public string airlinesSheetName;
            

            public Database (string _name, string _version, string _businessSheetName, string _liveriesSheetName, string _airlinesSheetName)
            {
                name = _name;
                version = _version;
            businessSheetName = _businessSheetName;
            liveriesSheetName = _liveriesSheetName;
            airlinesSheetName = _airlinesSheetName;
            businesses = new List<Business>();
            Airlines = new Dictionary<string, Airline>();
            products = new ProductsContainer();
            Debug.Log("Created a Database with the name: " + name);
            }


            /// <summary>
            /// takes in all the values from the googlecontroller, reads the data and splits it to the relevant functions
            /// </summary>
            /// <param name="businesses"></param>
            /// <param name="liveries"></param>
            /// <param name="products"></param>
            public void GetAllBusinessData(IList<IList<object>> businesses, IList<IList<object>> liveries, IList<IList<object>> products, IList<IList<object>> airlines)
            {
            Debug.Log("GetAllBusinessData for " + name + ": businesses length: " + businesses.Count.ToString() + " liveries length: " + liveries.Count());
            foreach (IList<object> row in businesses)
            {
                if ((string)row[0] == "")
                {
                    break;
                } else if ((string)row[7] != "Complete")
                {
                    continue;
                }
                switch (row[0])
                {
                    case "Bank":
                        GetBankData(row);
                        break;
                    case "FoodFranchise":
                        GetFranchisesData(row);
                        break;
                    case "ShopFranchise":
                        GetFranchisesData(row);
                        break;
                    case "AvFuelSupplier":
                        GetAvFuelData(row);
                        break;
                    case "Catering":
                        GetCateringData(row);
                        break;
                    case "Deicing":
                        GetDeicingData(row);
                        break;
                    case "Contractors":
                        GetContractorsData(row);
                        break;
                    default:
                        break;
                }
            }
            GetAirlineData(airlines);
            GetLiveriesData(liveries);
            GetProductsData(products);
            
            }


            public void GetBankData(IList<object> row)
            {
                
                 if ((string)row[1] == "")
                  {
                    return;
                  }
                 else
                  {
                    if ((string)row[7] != "Complete" || row.Count < 10)
                    {
                        return;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Bank", (string)row[9], (string)row[6]));
                  }
            }

            public void GetContractorsData(IList<object> row)
            {
                
                        if ((string)row[1] == "")
                        {
                            return;
                        }
                        else
                        {
                            if ((string)row[7] != "Complete")
                            {
                                return;
                            }
                            businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Contractor", (string)row[9], (string)row[6]));
                        }
            }

            public void GetAvFuelData(IList<object> row)
            {
                        if ((string)row[1] == "")
                        {
                          return;
                        }
                        else
                        {
                            if ((string)row[7] != "Complete")
                            {
                                return;
                            }
                            businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "AVFuelSupplier", (string)row[9], (string)row[6]));
                        }
            }

            public void GetCateringData(IList<object> row)
            {
                        if ((string)row[1] == "")
                        {
                            return;
                        }
                        else
                        {
                            if ((string)row[7] != "Complete")
                            {
                                return;
                            }
                            businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Catering", (string)row[9], (string)row[6]));
                        }
            }

            private static void SaveStream(MemoryStream stream, string saveTo)
            {
                using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
                {
                    stream.WriteTo(file);
                }
            }

          public void GetAirlineData(IList<IList<object>> values)
            {
            //Debug.Log("GetAirlineData");
            foreach (IList<object> row in values)
            {
                if (row.Count <= 2 || (string)row[1] == "")
                {
                    break;
                }
                
                if (row.Count <= 14 || (string)row[1] == "Name")
                {
                    continue;
                }
                else
                {
                    //Debug.Log((string)row[1] + ":");
                    if ((string)row[0] == "FALSE")
                    {
                        continue;

                    } //checks if the row length is long enough, and thus if it has colors defined
                    else if (row.Count >= 14)
                    {
                        Airline a;
                        a = new Airline((string)row[1], "", (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Airline", (string)row[9], (string)row[11], (string)row[6], (string)row[13], (string)row[12]);
                        businesses.Add(a);
                        Airlines.Add(a.name, a);
                    }
                    else if (row.Count >= 12)
                    {
                        Airline a;
                        a = new Airline((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Airline", (string)row[9], (string)row[11], (string)row[6], "", "");
                        businesses.Add(a);
                        Airlines.Add(a.name, a);
                    }
                }
            }
          }

            public void GetLiveriesData(IList<IList<object>>  values)
            {
                if (values != null && values.Count > 01)
                {
                foreach (var row in values){
                    
                    
                        if (row.Count < 11 || (string)row[4] != "Complete" )
                        {
                            continue;
                        }
                    
                    //Debug.Log("Creating the livery by " + (string)row[3] + " for " + (string)row[0]);

                    Livery a = new Livery((string)row[2], (string)row[3], (string)row[0], (string)row[11], (string)row[10]);
                        //Debug.Log(a.Airline);
                        try
                        {
                            Airlines[a.Airline].liveries.Add(a);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Error on GetLiveriesData: " + e + " Livery stats: (Aircraft: " + a.Aircraft + " , Airline: " + a.Airline + " )");
                            throw;
                        }
                    if ((string)row[1] == "")
                    {
                        break;
                    }

                }
                }
            }   

            /**
             * Gets all products data from the spreadsheet, checks if the products arent from apoaspis or have no image link and creatas Products instances from the data. (those get added to a list in their constructor
             * 
             */
            public void GetProductsData(IList<IList<object>> values)
            {
              
                if (values != null && values.Count > 01)
                {
                    foreach (var row in values)
                    {
                        if (row.Count <= 6)
                        {
                            continue;
                        }
                        Product a;
                        if ((string)row[5] == "Apoapsis Studios" || (string)row[1] == "" || (string)row[6] == "")
                        {
                            continue;
                        }
                        //Debug.Log(row.Count);
                        if (row.Count >= 13)
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], (string)row[10], (string)row[11], (string)row[12], (string)row[5]);
                            //Debug.Log("rowcount 13");
                        }
                        else if (row.Count >= 12)
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], (string)row[10], (string)row[11], "", (string)row[5]);
                        }
                        else if (row.Count >= 11)
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], (string)row[10], "", "", (string)row[5]);
                        }
                        else if (row.Count >= 10)
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], "", "", "", (string)row[5]);
                        }
                        else if (row.Count >= 9)
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], "", "", "", "", (string)row[5]);
                        }
                        else
                        {
                            a = new Product((string)row[3], (string)row[2], (string)row[6], "", "", "", "", "", (string)row[5]);
                            //Debug.Log("rowcount 8");
                            a.canHaveAnyColor = true;
                        }


                        products.array.Add(a);
                    }
                }
                //Debug.Log(products.array.Count + " products in list");
            }

            public void GetDeicingData(IList<object> row)
            {
                
                        if ((string)row[1] == "")
                        {
                            return;
                        }
                        else
                        {
                            if ((string)row[7] != "Complete")
                            {
                                return;
                            }
                            businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Deicing", (string)row[9], (string)row[6]));
                        }
                    
                
            }

            public void GetFranchisesData(IList<object> row)
            {
                

                        //Debug.Log("InFranchisesCheck");
                        //checking for invalid data
                        if (row.Count < 16 || (string)row[15] == "" || (string)row[14] == "" || (string)row[13] == "" || (string)row[12] == "" || (string)row[11] == "")
                        {
                return;
                        }
                        businesses.Add(new Franchise((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], (string)row[0], (string)row[9], (string)row[10], new string[] { (string)row[11], (string)row[12], (string)row[13], (string)row[14], (string)row[15] }));

            }

        public void Clear()
        {
            products = null;
            businesses = null;
            Airlines = null;
            products = new ProductsContainer();
            businesses = new List<Business>();
            Airlines = new Dictionary<string, Airline>();
        }
                
    }
}