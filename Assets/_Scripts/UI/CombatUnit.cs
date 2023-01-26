using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    #region cost
    public int unitFoodCost;
    public int unitWoodCost;
    public int unitMetalCost;
    public int unitOrderCost;
    public int unitChaosCost;
    #endregion
    #region stats
    public int unitHealth;
    public int unitOffense;
    public int unitDefense;
    public int unitFoodConsumption;
    #endregion
    public string unitName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract void hireTroop();
}
