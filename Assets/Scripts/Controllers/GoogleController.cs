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


[Serializable]
public class GoogleController : MonoBehaviour {
    readonly string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly, DriveService.Scope.DriveReadonly };
    readonly string applicationName = "ACEOMM2";
    UserCredential credential;
    public SheetsService service;

    // Use this for initialization
    void Start () {
        string P12Path = Path.Combine(Application.streamingAssetsPath, "key.p12");
        var certificate = new X509Certificate2(P12Path, "notasecret", X509KeyStorageFlags.Exportable);
        string serviceAccountEmail = "aceomm2@quickstart-1554883937733.iam.gserviceaccount.com";
        ServiceAccountCredential credential = new ServiceAccountCredential(
           new ServiceAccountCredential.Initializer(serviceAccountEmail)
           {
               Scopes = scopes
           }.FromCertificate(certificate));

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "ACEOMM2",
        });

        GetDataGoogle();
	}

    public void GetDataGoogle()
    {
        Debug.Log("GetDataGoogle");
        string range = "Main!A2:E20";
        SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get("1eFn71SVZsVvH7qOLv99ZR3Kl4ozn2b6V5s__4JQgZMk", range);

        //loops through all the data got from the sheet
        ValueRange response = request.Execute();
        IList<IList<System.Object>> values = response.Values;
        SpreadsheetsResource.ValuesResource.GetRequest requestProducts =
                service.Spreadsheets.Values.Get("1eFn71SVZsVvH7qOLv99ZR3Kl4ozn2b6V5s__4JQgZMk", "MASTERS_Products!A2:M250");

        //loops through all the data got from the sheet
        ValueRange responseProducts = requestProducts.Execute();
        IList<IList<System.Object>> productValues = response.Values;
        foreach (var row in values)
        {
            if ((string)row[0] == "")
            {
                break;
            }
            else
            {
                Debug.Log(row[0]);
                Database a = new Database((string)row[0], (string)row[4], (string)row[1], (string)row[2], (string)row[3]);
                Controller.instance.databases.Add(a);

                //businesses
                SpreadsheetsResource.ValuesResource.GetRequest requestBusinesses =
                service.Spreadsheets.Values.Get("1eFn71SVZsVvH7qOLv99ZR3Kl4ozn2b6V5s__4JQgZMk", a.businessSheetName);

                //loops through all the data got from the sheet
                ValueRange responseBusinesses = requestBusinesses.Execute();
                IList<IList<System.Object>> valuesBusinesses = responseBusinesses.Values;

                SpreadsheetsResource.ValuesResource.GetRequest requestLiveries =
                service.Spreadsheets.Values.Get("1eFn71SVZsVvH7qOLv99ZR3Kl4ozn2b6V5s__4JQgZMk", a.liveriesSheetName);

                //loops through all the data got from the sheet
                ValueRange responseLiveries = requestLiveries.Execute();
                IList<IList<System.Object>> valuesLiveries = responseLiveries.Values;

                SpreadsheetsResource.ValuesResource.GetRequest requestAirlines =
                service.Spreadsheets.Values.Get("1eFn71SVZsVvH7qOLv99ZR3Kl4ozn2b6V5s__4JQgZMk", a.airlinesSheetName);

                //loops through all the data got from the sheet
                ValueRange responseAirlines = requestLiveries.Execute();
                IList<IList<System.Object>> valuesAirlines = responseLiveries.Values;

                a.GetAllBusinessData(valuesBusinesses, valuesLiveries, productValues, valuesAirlines);
            }
        }
        GameObject.FindObjectOfType<UIController>().RefreshModUi();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
}


