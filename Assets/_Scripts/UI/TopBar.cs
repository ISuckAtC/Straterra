using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI foodText;
    [SerializeField]
    private TMPro.TextMeshProUGUI woodText;
    [SerializeField]
    private TMPro.TextMeshProUGUI metalText;
    [SerializeField]
    private TMPro.TextMeshProUGUI orderText;
    [SerializeField]
    private TMPro.TextMeshProUGUI chaosText;

    public static TopBar I;

    public int Food 
    {
        set
        {
            foodText.text = value.ToString();
        }
    }
    public int Wood 
    {
        set
        {
            woodText.text = value.ToString();
        }
    }
    public int Metal 
    {
        set
        {
            metalText.text = value.ToString();
        }
    }
    public int Order 
    {
        set
        {
            orderText.text = value.ToString();
        }
    }
    public int Chaos 
    {
        set
        {
            chaosText.text = value.ToString();
        }
    }

    public void Awake()
    {
        if (I != null) throw new System.Exception("Another topbar detected");
        I = this;
    }
}
