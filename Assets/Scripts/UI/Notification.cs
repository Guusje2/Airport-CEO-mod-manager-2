﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public Text NoticationText;
    
    public void OnCloseButton ()
    {
        gameObject.SetActive(false);
    }
}
