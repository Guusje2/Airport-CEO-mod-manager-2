using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Sheets;
using Google.Apis;
using ACEOMM2;
using System;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

//this is one complete mess, needs major changes

[Serializable]
public class GoogleController : MonoBehaviour {
    readonly string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    readonly string applicationName = "ACEOMM2";
    UserCredential credential;
    public SheetsService service;
    public ModPack ACEOMM;
    //public ModPack test;

    // Use this for initialization
    void Start () {
        /*using (var stream = new FileStream("Credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Debug.Log("Credential file saved to: " + credPath);
        }
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName,
        }); */

        ACEOMM = new ModPack("1hb1i3JrN9hKFkSLe3Wpe9XPKTbWkR3PKab2cIiWwIi0", "ACEOMM");
        ACEOMM.ranges.Add("data", "Lookups!K2:Q3");
        ACEOMM.ranges.Add("banks", "Banks!A2:K500");
        ACEOMM.ranges.Add("contractors", "Contractors!A2:K500");
        ACEOMM.ranges.Add("avfuel", "AVFuelSuppliers!A2:K500");
        ACEOMM.ranges.Add("catering", "Catering!A2:K500");
        ACEOMM.ranges.Add("airlines", "Airlines!A2:S1500");
        ACEOMM.ranges.Add("liveries", "Liveries!A2:O1000");
        ACEOMM.ranges.Add("products", "Products!A2:M500");
        ACEOMM.ranges.Add("deice", "De-Ice!A2:K100");
        ACEOMM.ranges.Add("franchises", "Franchises!A2:Q1000");

        ACEOMM.GetMainData();
        ACEOMM.GetBankData();
        ACEOMM.GetContractorsData();
        ACEOMM.GetAvFuelData();
        ACEOMM.GetCateringData();
        ACEOMM.GetAirlineData();
        ACEOMM.GetLiveriesData();
        ACEOMM.GetProductsData();
        ACEOMM.GetDeicingData();
        ACEOMM.GetFranchisesData();
        GameObject.FindObjectOfType<UIController>().RefreshModUi();
        
       
        Debug.Log(ACEOMM.description);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}

[Serializable]
public class ModPack
{
    public List<Business> businesses;
    public Dictionary<string, Airline> Airlines;
    public ProductsContainer products;
    public string version;
    public string name;
    public string description;
    public string author;
    public string spreadsheetId;
    public static
    readonly string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly, DriveService.Scope.DriveReadonly };
    /// <summary>
    /// contains the ranges google spreadsheet api uses. key is a lowercase string like basic;banks;avfuel;
    /// </summary>
    public Dictionary<string, string> ranges;
    private ServiceAccountCredential credential;
    private SheetsService SheetService;


    public ModPack (string _spreadsheetId, string _name)
    {
        spreadsheetId = _spreadsheetId;
        ranges = new Dictionary<string, string>();
        businesses = new List<Business>();
        Airlines = new Dictionary<string, Airline>();
        products = new ProductsContainer();
        name = _name;
        string P12Path = Path.Combine(Application.streamingAssetsPath, "key.p12");
        var certificate = new X509Certificate2(P12Path, "notasecret", X509KeyStorageFlags.Exportable);
        string serviceAccountEmail = "aceomm2@quickstart-1554883937733.iam.gserviceaccount.com";
        ServiceAccountCredential credential = new ServiceAccountCredential(
           new ServiceAccountCredential.Initializer(serviceAccountEmail)
           {
               Scopes = scopes
           }.FromCertificate(certificate));

        SheetService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "ACEOMM2",
        });
        Controller.instance.modPacks.Add(this);
    }

    public void GetAllBusinessData()
    {
        GetAvFuelData();
        GetBankData();
        GetCateringData();
        GetContractorsData();
        GetAirlineData();
        GetLiveriesData();
        GetDeicingData();
        GetFranchisesData();
        GetProductsData();
    }

    public void GetMainData()
    {
        string range = ranges["data"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
         ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            for (int i = 0; i < 1; i++)
            {
                var row = values[i];
           
                Debug.Log("Row length" + row);
                version = (string)row[1];
                name = (string)row[3];
                description = (string)row[0];
                author = (string)row[4];
            }
        }
        else
        {
            Debug.Log("No Data Found");
            
        }
    }

    public void GetBankData ()
    {
        string range = ranges["banks"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if((string)row[1] == "")
                {
                    break;
                } else
                {
                    if ((string)row[7] != "Complete")
                    {
                        continue;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Bank", (string)row[9], (string)row[6]));
                }
            }
        }
    }

    public void GetContractorsData ()
    {
        string range = ranges["contractors"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    if ((string)row[7] != "Complete")
                    {
                        continue;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Contractor", (string)row[9], (string)row[6]));
                }
            }
        }
    }

    public void GetAvFuelData()
    {
        string range = ranges["avfuel"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    if ((string)row[7] != "Complete")
                    {
                        continue;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "AVFuelSupplier", (string)row[9], (string)row[6]));
                }
            }
        }
    }

    public void GetCateringData()
    {
        string range = ranges["catering"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    if ((string)row[7] != "Complete" )
                    {
                        continue;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Catering", (string)row[9], (string)row[6]));
                }
            }
        }
    }

    private static void SaveStream(MemoryStream stream, string saveTo)
    {
        using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
        {
            stream.WriteTo(file);
        }
    }

    public void GetAirlineData()
    {
        string range = ranges["airlines"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;   
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    //Debug.Log((string)row[1] + ":" + (string)row[18] + "!");
                    if ((string)row[7] != "Complete" || (string)row[18] == "#N/A")
                    {
                        continue;
                    }
                    Airline a;
                    a = new Airline((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Airline", (string)row[9], (string)row[11], (string)row[6], (string)row[12],(string)row[13]);
                    businesses.Add(a);
                    Airlines.Add((string)row[1], a);
                }
            }
        }
    }

    public void GetLiveriesData()
    {
        string range = ranges["liveries"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
         ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    if ((string)row[4] != "Complete" )
                    {
                        continue;
                    }
                    //Debug.Log("Creating the livery by " + (string)row[2] + " for " + (string)row[0]);
                    
                    Livery a = new Livery((string)row[2], (string)row[3], (string)row[0], (string)row[12],(string)row[11]);
                    //Debug.Log(a.Airline);
                    Airlines[(string)row[0]].liveries.Add(a);
                }
            }
        }
    }

    /**
     * Gets all products data from the spreadsheet, checks if the products arent from apoaspis or have no image link and creatas Products instances from the data. (those get added to a list in their constructor
     * 
     */
    public void GetProductsData()
    {
        string range = ranges["products"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
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
                } else if (row.Count >= 12)
                {
                    a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], (string)row[10], (string)row[11], "", (string)row[5]);
                } else if (row.Count >= 11)
                {
                    a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], (string)row[10], "", "", (string)row[5]);
                } else if (row.Count >= 10)
                {
                    a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], (string)row[9], "", "", "", (string)row[5]);
                } else if (row.Count >= 9)
                {
                    a = new Product((string)row[3], (string)row[2], (string)row[6], (string)row[8], "", "", "", "", (string)row[5]);
                } else
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

    public void GetDeicingData()
    {
        string range = ranges["deice"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                if ((string)row[1] == "")
                {
                    break;
                }
                else
                {
                    if ((string)row[7] != "Complete")
                    {
                        continue;
                    }
                    businesses.Add(new Business((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Deicing", (string)row[9], (string)row[6]));
                }
            }
        }
    }

    public void GetFranchisesData()
    {
        Debug.Log("GetFranchisesData");
        string range = ranges["franchises"];
        SpreadsheetsResource.ValuesResource.GetRequest request =
                SheetService.Spreadsheets.Values.Get(spreadsheetId, range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        //Debug.Log(values.Count);
        if (values != null && values.Count > 01)
        {
            foreach (var row in values)
            {
                
                //Debug.Log("InFranchisesCheck");
                //checking for invalid data
                if (row.Count < 16 || (string)row[15] == "" || (string)row[14] == "" || (string)row[13] == "" || (string)row[12] == "" || (string)row[11] == "")
                {
                    continue;
                }
                businesses.Add(new Franchise((string)row[1], (string)row[0], (string)row[2], (string)row[3], (string)row[4], (string)row[5], "Franchise", (string)row[9], (string)row[10], new string[] { (string)row[11], (string)row[12], (string)row[13], (string)row[14], (string)row[15]}));
                
            }
        }
    }
}
