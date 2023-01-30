using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoScreen : MonoBehaviour
{
    public static InfoScreen _instance;
    
    public GameObject infoScreen;
    
    public TMP_Text coordinateText;
    
    public TMP_Text tileTypeText;
    public Image tileImage;
    
    public TMP_Text foodAmountText;
    public Image foodImage;
    
    public TMP_Text woodAmountText;
    public Image woodImage;
    
    public TMP_Text metalAmountText;
    public Image metalImage;
    
    public TMP_Text chaosAmountText;
    public Image chaosImage;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        infoScreen.SetActive(false);
    }

    public void ToggleInfoScreen(bool enable)
    {
        if (enable)
        {
            infoScreen.SetActive(true);
            return;
        }
        
        infoScreen.SetActive(false);
    }

    public void UpdateInfoScreen(Vector2 pos)
    {
        int tile = Grid._instance.GetIdByVec(pos);

        int tileType = Grid._instance.tiles[tile].tileType;


        switch (tileType)
        {
            // 0 - no tile | 1 - water | 2 - grassland | 3 - forest | 4 - hill | 5 - mountain
            case 0 :
                tileTypeText.text = "Barrier";
                break;
            
            case 1 :
                tileTypeText.text = "Lake";
                break;
            
            case 2 :
                tileTypeText.text = "Grassland";
                break;
            
            case 3 :
                tileTypeText.text = "Forest";
                break;
            
            case 4 :
                tileTypeText.text = "Hill";
                break;
            
            case 5 :
                tileTypeText.text = "Mountain";
                break;
        }
        
        
        int roundedFoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[tile].foodAmount) * 100));
        foodAmountText.text = "Food: " + roundedFoodAmt + "%";

        float roundedWoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[tile].woodAmount) * 100));
        woodAmountText.text = "Wood: " + roundedWoodAmt + "%";

        float roundedMetalAmt = (Mathf.FloorToInt((Grid._instance.tiles[tile].metalAmount) * 100));
        metalAmountText.text = "Metal: " + roundedMetalAmt + "%";

        float roundedChaosAmt = (Mathf.FloorToInt((Grid._instance.tiles[tile].chaosAmount) * 100));
        chaosAmountText.text = "Chaos: " + + roundedChaosAmt + "%";

        Vector2 idSplit = Grid._instance.GetPosition(tile);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y/* + "  Byte: " + PlaceTiles._instance.FindAdjacentWater(Grid._instance.tiles[tile].id)*/; //PlaceTiles._instance.;


    }
    
}
