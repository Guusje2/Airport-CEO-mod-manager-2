﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDownloadMods()
    {
        SceneManager.LoadScene("SheetsDataScene");
    }

    public void OnCreateABusiness()
    {
        NotificationController.SendNotification("This hasn't been implemented yet"); 
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnDiscordButton()
    {
        Application.OpenURL("https://discord.gg/NdU5eUA");
    }

    public void OnMailButton()
    {
        Application.OpenURL("https://mailto:aceommteam@gmail.com");
    }
}
