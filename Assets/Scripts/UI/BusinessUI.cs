using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUI : MonoBehaviour
{
    public new Text name;
    public Text country;
    public Text businessType;
    public Button moreInfoButton;
    public Toggle selectedToggle;
    public static UIController controller;
    // Start is called before the first frame update
    void Start()
    {
        if (controller == null)
        {
            controller = GameObject.FindObjectOfType<UIController>();
        }
    }

    public void OnToggle(bool b)
    {
        if (b)
        {
            controller.OnSelectBusiness(this.gameObject);
            Debug.Log("Selected " + name);
        }   else
        {
            controller.OnDeselectBusiness(this.gameObject);
        }
    }

    public void OnMoreInfo()
    {
        controller.bp.gameObject.SetActive((true));
        controller.bp.Business = controller.GameobjectBusinessDatabase[this.gameObject];
    }
}
