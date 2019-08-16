using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusinessUI : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text country;
    public TMP_Text businessType;
    public Button moreInfoButton;
    public Toggle selectedToggle;
    public static UIController controller;
    public static bool wasLastColored = true;
    public bool isColored = false;
    // Start is called before the first frame update
    void Start()
    {
        if (controller == null)
        {
            controller = FindObjectOfType<UIController>();
        }
        if (!wasLastColored)
        {
            GetComponent<RawImage>().color = new Color(.7f, .7f, .7f,1);
            isColored = true;
            wasLastColored = true;
        } else
        {
            wasLastColored = false;
        }
    }

    public void OnToggle(bool b)
    {
        if (b)
        {
            controller.OnSelectBusiness(gameObject);
            Debug.Log("Selected " + name.text);
        }   else
        {
            controller.OnDeselectBusiness(gameObject);
            if (isColored)
            {
                GetComponent<RawImage>().color = new Color(.7f, .7f, .7f, 1);
            }
        }
    }

    public void OnMoreInfo()
    {
        controller.bp.gameObject.SetActive((true));
        controller.bp.Business = controller.GameobjectBusinessDatabase[gameObject];
    }
}
