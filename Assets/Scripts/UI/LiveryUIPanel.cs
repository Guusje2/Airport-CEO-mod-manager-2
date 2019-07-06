using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LiveryUIPanel : MonoBehaviour
{
    public TextMeshProUGUI Aircraft;
    public TextMeshProUGUI Author;
    
    public void SetNewLivery (string _Aircraft, string _Author)
    {
        Aircraft.text = _Aircraft;
        Author.text = _Author;
    }
}
