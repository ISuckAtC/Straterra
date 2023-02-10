using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BalancingUI : MonoBehaviour
{
    private BattleSim bs = new BattleSim();

    private bool winner;
    
    // Group : 3 ints, 1 bool

    public List<Group> green = new List<Group>();
    public List<Group> red = new List<Group>();

    [Header("Green Team")]
    [Range(0, 200)] public int swordsmenA;
    [Range(0, 200)] public int archersA;
    [Range(0, 200)] public int spearmenA;
    [Range(0, 200)] public int cavalryA;
    
    [Space(15)]
    [Header("Red Team")]
    [Range(0, 200)] public int swordsmenB;
    [Range(0, 200)] public int archersB;
    [Range(0, 200)] public int spearmenB;
    [Range(0, 200)] public int cavalryB;
    
    
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FillGroup();
            
            winner = bs.Fight(green, red);

            if (winner)
                Debug.LogWarning("Left Wins.");
            else
                Debug.LogWarning("Right Wins.");
        }
    }
    
    private void FillGroup()
    {
        // Archer id    : 0
        // Cavalry id   : 1
        // Swordman id  : 2
        // Spearman id  : 3
        
        green.Clear();
        red.Clear();
        
        // public Group(int _count, int _unitId, int _position, bool _right)
        if (archersA > 0)
            green.Add(new Group(archersA, 0, -10, false));
        
        if (cavalryA > 0)
            green.Add(new Group(cavalryA, 1, -10, false));
        
        if (swordsmenA > 0)
            green.Add(new Group(swordsmenA, 2, -10, false));
        
        if (spearmenA > 0)
            green.Add(new Group(spearmenA, 3, -10, false));
        
        
        if (archersB > 0)
            red.Add(new Group(archersB, 0, -10, false));
        
        if (cavalryB > 0)
            red.Add(new Group(cavalryB, 1, -10, false));
        
        if (swordsmenB > 0)
            red.Add(new Group(swordsmenB, 2, -10, false));
        
        if (spearmenB > 0)
            red.Add(new Group(spearmenB, 3, -10, false));
    }
}
