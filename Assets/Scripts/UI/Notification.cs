using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public TMP_Text NoticationText;
    
    public void OnCloseButton ()
    {
        gameObject.SetActive(false);
    }
}
