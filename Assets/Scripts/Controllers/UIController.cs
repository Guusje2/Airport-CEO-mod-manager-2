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
        controller.InstallMod();
    }

    public void OnInstallFolder ()
    {
        controller.InstallFolder = FileBrowser.OpenSingleFolder("Select the install folder", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Apoapsis Studios//Airport CEO//Mods");
    }

    public void RefreshMods ()
    {
        foreach (BusinessUI gm in content.GetComponentsInChildren<BusinessUI>())
        {
            Destroy(gm.gameObject);
        }
        Debug.Log("BusinessProcessing");
        controller.modPacks[0].businesses.Clear();
        controller.modPacks[0].Airlines.Clear();
        controller.modPacks[0].GetAllBusinessData();
        RefreshModUi();
    }

    public void RefreshModUi()
    {
        foreach (ModPack mp in controller.modPacks)
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
        return null;
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
    