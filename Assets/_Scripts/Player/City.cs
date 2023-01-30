using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuStage
{
    city,
    townhall,
    barracks
}
public class City : MonoBehaviour
{
    public TopBar topBar;
    public GameObject townhall;
    public GameObject barracks;
    
    // embassy, residence, smithy, academy, rally point, marketplace, granary, warehouse, temple, grain mill, cranny(cellar), stonemason, 
    // waterworks (oasis), 
    // military: hero mansion, stable, workshop, hospital, torment square (basically just a gym)
    // resources: sawmill, bakery, brickyard, forge 
    
    // town hall: upgrades allow you to upgrade buildings and build more buildings. No buildings can be upgraded past the level of town hall.
    // 
    
    public MenuStage stage;
    public void ClickTownHall()
    {
        if (stage == MenuStage.city)
        {
            stage = MenuStage.townhall;
            townhall.SetActive(true);
        }
    }
    public void ClickBarracks()
    {
        if (stage == MenuStage.city)
        {
            stage = MenuStage.barracks;
            barracks.SetActive(true);
        }
    }
    public void ExitMenu()
    {
        townhall.SetActive(false);
        barracks.SetActive(false);
        stage = MenuStage.city;
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
