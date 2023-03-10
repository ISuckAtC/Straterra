using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text foodText;
    [SerializeField]
    private TMPro.TextMeshProUGUI woodText;
    [SerializeField]
    private TMPro.TextMeshProUGUI metalText;
    [SerializeField]
    private TMPro.TextMeshProUGUI orderText;
    [SerializeField]
    private TMPro.TextMeshProUGUI chaosText;

    private bool foodDirty;
    private bool woodDirty;
    private bool metalDirty;
    private bool orderDirty;

    private int food;
    private int wood;
    private int metal;
    private int order;
    
    public static TopBar I;

    public int Food 
    {
        set
        {
            foodDirty = true;
            food = value;
            //foodText.text = NumConverter.GetConvertedAmount(value);
        }
    }
    public int Wood 
    {
        set
        {
            woodDirty = true;
            wood = value;
            //woodText.text = NumConverter.GetConvertedAmount(value);
        }
    }
    public int Metal 
    {
        set
        {
            metalDirty = true;
            metal = value;
            //metalText.text = NumConverter.GetConvertedAmount(value);
        }
    }
    public int Order 
    {
        set
        {
            orderDirty = true;
            order = value;
            //orderText.text = NumConverter.GetConvertedAmount(value);
        }
    }
    public int Chaos 
    {
        set
        {
            chaosText.text = NumConverter.GetConvertedAmount(value);
        }
    }

    public void Awake()
    {
        if (I != null) throw new System.Exception("Another topbar detected");
        I = this;
    }

    public void Update()
    {
        if (foodDirty)
        {
            foodDirty = false;
            foodText.text = NumConverter.GetConvertedAmount(food);
        }
        if (woodDirty)
        {
            woodDirty = false;
            woodText.text = NumConverter.GetConvertedAmount(wood);
        }
        if (metalDirty)
        {
            metalDirty = false;
            metalText.text = NumConverter.GetConvertedAmount(metal);
        }
        if (orderDirty)
        {
            orderDirty = false;
            orderText.text = NumConverter.GetConvertedAmount(order);
        }
    }
}
