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

    private string food;
    private string wood;
    private string metal;
    private string order;
    private string chaos;
    public string Food 
    {
        get 
        {
            return food;
        }
        set
        {
            foodText.text = food = value;
        }
    }
    public string Wood 
    {
        get 
        {
            return wood;
        }
        set
        {
            woodText.text = wood = value;
        }
    }
    public string Metal 
    {
        get 
        {
            return metal;
        }
        set
        {
            metalText.text = metal = value;
        }
    }
    public string Order 
    {
        get 
        {
            return order;
        }
        set
        {
            orderText.text = order = value;
        }
    }
    public string Chaos 
    {
        get 
        {
            return chaos;
        }
        set
        {
            chaosText.text = chaos = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
