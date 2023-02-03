using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController _instance;

    public Image SelectedBuildingImage;
    
    public Sprite[] overlayTiles;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetBuildingImage(byte buildingIndex)
    {
        SelectedBuildingImage.sprite = overlayTiles[buildingIndex];
    }

}
