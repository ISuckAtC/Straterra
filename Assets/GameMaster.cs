using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameMaster : MonoBehaviour
{

    int food;
    int wood;
    int metal;
    int order;
    int chaos;

    public TMPro.TMP_Text foodText;
    public TMPro.TMP_Text woodText;
    public TMPro.TMP_Text metalText;
    public TMPro.TMP_Text orderText;
    public TMPro.TMP_Text chaosText;
    GameObject soldier;

    public GameObject resourcesMenu;
    public GameObject hireTroopsMenu;
    public GameObject upgradeTroopsMenu;
    public List<string> windowNameList;



    public void GetResource(string name)
    {
        switch (name)
        {
            case "food": //Food
                int foodAmount;
                foodAmount = Random.Range(5, 80);
                food += foodAmount;
                Debug.Log("food is " + food);
                UpdateResources();
                break;

            case "wood": //Wood
                int woodAmount;
                woodAmount = Random.Range(5, 80);
                wood += woodAmount;
                Debug.Log("wood is " + wood);
                UpdateResources();
                break;

            case "metal": //Metal
                int metalAmount;
                metalAmount = Random.Range(5, 80);
                metal += metalAmount;
                Debug.Log("metal is " + metal);
                UpdateResources();
                break;

            case "order": //Order
                int orderAmount;
                orderAmount = Random.Range(5, 80);
                order += orderAmount;
                Debug.Log("order is " + order);
                UpdateResources();
                break;

            case "chaos": //chaos
                int chaosAmount;
                chaosAmount = Random.Range(5, 80);
                chaos += chaosAmount;
                Debug.Log("chaos is " + chaos);
                UpdateResources();
                break;
        }

    }
    void LoseResource(int amount, string resource)
    {

    }
    void UpdateResources()
    {
        foodText.SetText(food.ToString());
        woodText.SetText(wood.ToString());
        metalText.SetText(metal.ToString());
        orderText.SetText(order.ToString());
        chaosText.SetText(chaos.ToString());
    }


    // Update is called once per frame
    void Update()
    {
        #region toggleOpenWindow
        /*
        LIST OF INPUTS
        R  resources
        P  options
        T  troops

        */

        if (Input.GetKeyDown(KeyCode.R))
        {
            resourcesMenu.gameObject.SetActive(!resourcesMenu.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            MenuHireTroops();
        }
        #endregion
    }


}
