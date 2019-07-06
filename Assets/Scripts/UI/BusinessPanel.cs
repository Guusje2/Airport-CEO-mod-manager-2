using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACEOMM2;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using TMPro;

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
    public ScrollRect FleetView;
    public RawImage LogoImage;
    public LiveryUIPanel fleetprefab;
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
        AuthorText.text = B.Author;
        if (B.businessType == BusinessType.airline)
        {
            foreach (Transform gm in FleetView.content.GetComponentsInChildren<Transform>())
            {
                if (gm.name == "Content")
                {
                    continue;
                }
                Destroy(gm.gameObject);
            }
            FleetView.gameObject.SetActive(true);
            Airline a = (Airline)B;
            foreach (Livery livery in a.liveries)
            {
                Debug.Log("Found livery for: " + livery.Aircraft + " By: " + livery.Author);
                LiveryUIPanel c = Instantiate<LiveryUIPanel>(fleetprefab, FleetView.content.transform);
                c.SetNewLivery(livery.Aircraft, livery.Author);
            }
        }
        else
        {
            FleetView.gameObject.SetActive(false);
        }
       /* B.DownloadLogo(Path.Combine(Application.temporaryCachePath, B.name + "png"));
      
        
        Debug.Log((string)Path.Combine(Application.temporaryCachePath, B.name + "png"));
        byte[] bytes = File.ReadAllBytes(Path.Combine(Application.temporaryCachePath, B.name + "png"));
        Texture2D a = new Texture2D(256, 256);
        a.filterMode = FilterMode.Trilinear;
        a.LoadImage(bytes);
        LogoImage.texture = a; */
    }

    public void OnClose ()
    {
        this.gameObject.SetActive(false);
    }
}
