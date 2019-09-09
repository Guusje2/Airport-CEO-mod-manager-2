using UnityEngine;
using System.Collections;
using ACEOMM2;
using Crosstales;  
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.FB;

[Serializable]
public class UIController : MonoBehaviour
{
    public Controller controller;
    public GameObject content;
    public BusinessUI Businessprefab;
    public BusinessPanel bp;
    public ModSettingsPanel MSP;
    
    public Dictionary<GameObject, Business> GameobjectBusinessDatabase;
    // Use this for initialization
    void Start()
    {
        GameobjectBusinessDatabase = new Dictionary<GameObject, Business>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCreateModData()
    {
        controller.currentmod.author = MSP.AuthorInput.text;
        controller.currentmod.description = MSP.DescriptionInput.text;
        controller.currentmod.name = MSP.NameInput.text;
        controller.InstallMod();
        NotificationController.SendNotification("Installed the mod succesfully");
    }

    public void OnInstallFolder ()
    {
        controller.defaultInstallFolder = FileBrowser.OpenSingleFolder("Select the install folder", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Apoapsis Studios//Airport CEO//Mods");
        NotificationController.SendNotification("Mod folder set to " + controller.defaultInstallFolder);
    }

    public void OnInstallMod()
    {
        MSP.gameObject.SetActive(true);
    }
    public void RefreshMods ()
    {
        foreach (BusinessUI gm in content.GetComponentsInChildren<BusinessUI>())
        {
            Destroy(gm.gameObject);
        }
        
        foreach (Database database in Controller.instance.databases)
        {
            Debug.Log("Clearing for " + database.name);
            database.Clear();
        }
        GameObject.FindObjectOfType<GoogleController>().GetDataGoogle();
        RefreshModUi();
        NotificationController.SendNotification("Refreshing data complete");
    }

    public void RefreshModUi()
    {
        foreach (Database mp in controller.databases)
        {
            foreach (Business business in mp.businesses)
            {
                BusinessProcessing(business);
            }
        }
    }

    public GameObject BusinessProcessing(Business b)
    {
        BusinessUI gm = Instantiate<BusinessUI>(Businessprefab, content.transform);
        gm.name.text = b.name;
        gm.businessType.text = b.businessType.ToString();
        gm.country.text = b.countryCode;
        GameobjectBusinessDatabase.Add(gm.gameObject, b);
        return gm.gameObject;
    }

    public void OnSelectionValueChanged (bool b)
    {
        GameObject gm = EventSystem.current.currentSelectedGameObject;
        if (b)
        {
            OnSelectBusiness(gm);
        } else
        {
            OnDeselectBusiness(gm);
        }
    }

    public void OnSelectBusiness (GameObject gm)
    {
        Business b = GameobjectBusinessDatabase[gm];
        gm.GetComponent<RawImage>().color = Color.grey;
        controller.currentmod.AddBusiness(b);
    }

    public void OnDeselectBusiness (GameObject gm)
    {
        Business b = GameobjectBusinessDatabase[gm];
        gm.GetComponent<RawImage>().color = Color.white;
        controller.currentmod.RemoveBusiness(b);
    }

    public void OnQuitButton ()
    {
        Application.Quit();
    }


}
    