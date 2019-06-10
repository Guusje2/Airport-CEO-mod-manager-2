using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACEOMM2;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class BusinessPanel : MonoBehaviour
{
    public Business Business { get { return B; } set
        {
            B = value;
            OnNewBusinessSet();
        } }
    private Business B;
    public Text NameText;
    public Text CEOText;
    public Text AuthorText;
    public Text DescriptionText;
    public Text CountryText;
    public Text TypeText;
    public Text ClassText;
    public Image LogoImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnNewBusinessSet()
    {
        NameText.text = B.name;
        CEOText.text = B.CEOName;
        DescriptionText.text = B.description;
        CountryText.text = B.countryCode;
        TypeText.text = B.businessType.ToString();
        ClassText.text = B.businessClass.ToString();
        Task<bool> t = B.DownloadLogo(Path.Combine(Application.streamingAssetsPath, B.name + "png"));
        if (t.Status != TaskStatus.Running || t.Status != TaskStatus.RanToCompletion)
        {
            t.RunSynchronously();
        }
        
        Debug.Log((string)Path.Combine(Application.streamingAssetsPath, B.name + "png"));
        LogoImage.sprite = IMG2Sprite.instance.LoadNewSprite(Path.Combine(Application.streamingAssetsPath, B.name + "png"));
    }

    public void OnClose ()
    {
        this.gameObject.SetActive(false);
    }
}
