using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationController : MonoBehaviour
{

    public static Notification NotificationPrefab;
    // Start is called before the first frame update
    void Start()
    {
        NotificationPrefab = (Notification)Resources.Load<Notification>("Notification");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SendNotification (string text)
    {
        Notification a = GameObject.Instantiate(NotificationPrefab);
        a.NoticationText.text = text;
        a.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;
        a.transform.position = new Vector3(Screen.width / 2 - 100, Screen.height / 2, 0);
    }
}
