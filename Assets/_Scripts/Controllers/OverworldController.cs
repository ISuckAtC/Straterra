using System;
using System.Collections;
using System.Collections.Generic;
using NetworkStructs;
using UnityEditorInternal.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OverworldController : MonoBehaviour
{
    public float movementAmount;

    private float zoom = 50f;
    public float zoomAmount;


    private Camera cam;

    public float moveSpeedHold;
    public float moveSpeedTap;

    public float holdDelay;
    private float holdCounter;
    private bool holding;

    private byte building;
    private byte buildingIndex;

    public Transform tileHighlight;
    public Transform selectedTileHighlight;
    public Transform buildMenu;
    private int previousTile = 1;
    public UnityEngine.UI.GraphicRaycaster gr;
    
    private Vector2 playerVillagePosition;
    public UnityEngine.Tilemaps.TileBase flag;
    
    void Start()
    {
        Grid.onReady += OnGridReady;
    }

    private void OnGridReady()
    {
        
        selectedTileHighlight.transform.position = new Vector3Int(Grid._instance.width, 1, Grid._instance.height);

        cam = GetComponent<Camera>();

        holdCounter = holdDelay;

        cam.orthographicSize = zoom;
        
        for (int i = 0; i < Network.allUsers.Count; ++i)
        {
            User user = Network.allUsers[i];

            Grid._instance.tiles[user.cityLocation].owner = user.userId;
            Grid._instance.tiles[user.cityLocation].building = 1;

            Vector2Int pos = Grid._instance.GetPosition(user.cityLocation);

            PlaceTiles._instance.overlayMap.SetTile(new Vector3Int(pos.x, pos.y, 1), PlaceTiles._instance.buildingTiles[1]);
            //PlaceTiles._instance.DiplomacyMap.SetColor(new Color();

            if (user.userId == LocalData.SelfUser.userId)
            {
                PlaceTiles._instance.DiplomacyMap.SetTile(new Vector3Int(pos.x, pos.y, 1), flag);                
            }

            
            

        }
        
        /*
        DarkShrine ds = new DarkShrine(startingposition / 2, 0.05f, 1f, 0.5f);
        SplashText.Splash("WELCOME");
        
        Vector2 cameraposition = Grid._instance.GetPosition(startingposition);
        
        transform.position = new Vector3(cameraposition.x, 98f, cameraposition.y);

        
        int startingposition = FindStartingPosition.FirstVillage();
        Vector2Int vPos = Grid._instance.GetPosition(startingposition);
        PlaceTiles._instance.DiplomacyMap.SetTile(new Vector3Int( vPos.x, vPos.y, 0), flag);
        //PlaceBuildingOnSelectedTile(1/*, startingposition*//*);

        PlaceOtherBuilding(1, 1, startingposition);


        
        Player selfPlayer = LocalData.SelfUser;
        
        selfPlayer.cityLocation = startingposition;

        LocalData.SelfUser = selfPlayer;
        */
        /*
        int enemyposition = FindStartingPosition.FirstVillage();
        PlaceOtherBuilding(1, 5, enemyposition);
        
        enemyposition = FindStartingPosition.FirstVillage();
        PlaceOtherBuilding(1, 6, enemyposition);
*/
        //List<List<Group>> armies = new List<List<Group>>();
        
        //PlaceTestVillage(5);
        //PlaceTestVillage(6);
        //PlaceTestVillage(7);
        //PlaceTestVillage(8);
        
        
        /*
        for (int i = 10; i < Grid._instance.width / 4; i++)
        {
            int enemyposition = FindStartingPosition.FirstVillage();
            
            PlaceOtherBuilding(1, i, enemyposition);

            Grid._instance.tiles[enemyposition].army = RandomEnemy();
        }
        */
        
        building = 1;
        buildingIndex = 10;

    }
    
    private List<Group> RandomEnemy()
    {
        float bias1 = Random.Range(0.001f, 3f);
        float bias2 = Random.Range(0f, 1f);
        float str = bias1 * bias2;
        
        List<Group> army = new List<Group>();
        
        for (int i = 0; i < Random.Range(1, 5); i++)
        {
            int amount = Random.Range(0, 10000);
            amount = (int)(amount * str);
            
            army.Add(new Group(amount, i));
        }


        return army;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            tileHighlight.transform.position = new Vector3Int((int)(hit.point.x + PlaceTiles.tilePivot.x), 1, (int)(hit.point.z + PlaceTiles.tilePivot.y));
        }


        // Building Selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Village
        {
            building = 4;
            buildingIndex = 10;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // House
        {
            building = 5;
            buildingIndex = 20;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Castle
        {
            building = 6;
            buildingIndex = 30;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Farm
        {
            building = 4;
            buildingIndex = 10;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Logging Camp
        {
            building = 5;
            buildingIndex = 20;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) // Mine
        {
            building = 6;
            buildingIndex = 30;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) // Darkshrine
        {
            building = 7;
            buildingIndex = 250;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        */

        // Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            holding = true;
            transform.position -= new Vector3(cam.orthographicSize * moveSpeedTap, 0f, 0f);
            
            if (transform.position.x < 0f)
                transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            holding = false;

        // Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            holding = true;
            transform.position += new Vector3(cam.orthographicSize * moveSpeedTap, 0f, 0f);
            
            if (transform.position.x > Grid._instance.width)
                transform.position = new Vector3(Grid._instance.width, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            holding = false;

        // Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            holding = true;
            transform.position += new Vector3(0f, 0f, cam.orthographicSize * moveSpeedTap);
            
            if (transform.position.z > Grid._instance.height)
                transform.position = new Vector3(transform.position.x, transform.position.y, Grid._instance.height);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            holding = false;

        // Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            holding = true;
            transform.position -= new Vector3(0f, 0f, cam.orthographicSize * moveSpeedTap);
            
            if (transform.position.z < 0f)
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            holding = false;

        if (holding)
            holdCounter -= Time.deltaTime;
        else
            holdCounter = holdDelay;

        if (holdCounter < 0f)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(movementAmount, 0f, 0f) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;

                if (transform.position.x < 0f)
                    transform.position = new Vector3(0f, transform.position.y, transform.position.z);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(movementAmount, 0f, 0f) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;

                if (transform.position.x > Grid._instance.width)
                    transform.position = new Vector3(Grid._instance.width, transform.position.y, transform.position.z);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0f, 0f, movementAmount) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;
                
                if (transform.position.z > Grid._instance.height)
                    transform.position = new Vector3(transform.position.x, transform.position.y, Grid._instance.height);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0f, 0f, movementAmount) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;
                
                if (transform.position.z < 0f)
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
        }

        
        
        
        if (Input.GetKey(KeyCode.Mouse2))
        {
            transform.position -= new Vector3(Input.GetAxisRaw("Mouse X"), 0f, Input.GetAxisRaw("Mouse Y")) * 2f;
            
            if (transform.position.x < 0f)
                transform.position = new Vector3(0f, transform.position.y, transform.position.z);
            if (transform.position.x > Grid._instance.width)
                transform.position = new Vector3(Grid._instance.width, transform.position.y, transform.position.z);
            if (transform.position.z > Grid._instance.height)
                transform.position = new Vector3(transform.position.x, transform.position.y, Grid._instance.height);
            if (transform.position.z < 0f)
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        if (Input.mouseScrollDelta.y != 0f)
        {
            if (zoom > 2f && zoom < 120f)
            {
                zoom += -Input.mouseScrollDelta.y * zoomAmount * Time.deltaTime;

                if (zoom <= 2.1f)
                    zoom = 2.1f;
                else if (zoom > 119.9f)
                    zoom = 119.9f;

                float buildScale = zoom * 0.025f;
            
                buildMenu.localScale = new Vector3(buildScale, buildScale, buildScale);
                
                cam.orthographicSize = zoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (zoom > 12f)
                zoom -= 10f;

            if (zoom <= 2.1f)
                zoom = 2.1f;

            float buildScale = zoom * 0.025f;
            
            buildMenu.localScale = new Vector3(buildScale, buildScale, buildScale);
            
            cam.orthographicSize = zoom;
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (zoom < 108f)
                zoom += 10f;

            if (zoom >= 119.9f)
                zoom = 119.9f;

            float buildScale = zoom * 0.025f;
            
            buildMenu.localScale = new Vector3(buildScale, buildScale, buildScale);
            
            cam.orthographicSize = zoom;
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Place village.
        {
            if (Physics.Raycast(ray, out hit))
            {
                int position = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));

                PlaceBuildingOnSelectedTile(buildingIndex/*, position*/);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Physics.Raycast(ray, out hit))
            {
                int id = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));
                AttackWithAll(id);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //if (!UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents)

            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = Input.mousePosition;

            List<RaycastResult> rrs = new List<RaycastResult>();
            
            gr.Raycast(ped, rrs);
            
            if (rrs.Count == 0 && Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    int id = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));
                    //InfoScreen._instance.ToggleInfoScreen(false);
                    //InfoScreen._instance.ToggleInfoScreenResource(false);
                    if (id == previousTile)
                    {
                        buildMenu.gameObject.SetActive(true);
                        buildMenu.transform.position = new Vector3Int((int)(hit.point.x + PlaceTiles.tilePivot.x), (int)1f, (int)(hit.point.z + PlaceTiles.tilePivot.y));
                    
                    }
                    else
                    {
                        //buildMenu.gameObject.SetActive(false);
                        previousTile = id;
                    }
                
                    selectedTileHighlight.gameObject.SetActive(true);
                    selectedTileHighlight.position = new Vector3Int((int)(hit.point.x + PlaceTiles.tilePivot.x), (int)1f, (int)(hit.point.z + PlaceTiles.tilePivot.y));
                
                    InfoScreen._instance.CloseInfoScreen();
                    InfoScreen._instance.CloseResourceInfoScreen();
                    InfoScreen._instance.CloseVillageInfoScreen();
                
                    CheckTile(id);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            selectedTileHighlight.gameObject.SetActive(false);
           
            buildMenu.gameObject.SetActive(false);
            
            InfoScreen._instance.CloseInfoScreen();
            InfoScreen._instance.CloseResourceInfoScreen();
            InfoScreen._instance.CloseVillageInfoScreen();
            //InfoScreen._instance.ToggleInfoScreen(false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            FocusOnVillage();
        }
    }

    public static void AttackWithAll(int position)
    {
        Debug.Log("Attacked tile has building type " + Grid._instance.tiles[position].building);
        if (Grid._instance.tiles[position].building != 1) return;
        List<Group> army = new List<Group>();
        for (int i = 0; i < GameManager.I.playerResources.unitAmounts.Length; ++i)
        {
            if (GameManager.I.playerResources.unitAmounts[i] > 0) 
            {
                army.Add(new Group(GameManager.I.playerResources.unitAmounts[i], i));
                GameManager.I.playerResources.unitAmounts[i] = 0;
            }
        }
        if (army.Count > 0)
        {
            Debug.Log("Scheduling attack");
            ScheduledAttackEvent attackEvent = new ScheduledAttackEvent(5, army, position, LocalData.SelfUser.cityLocation, LocalData.SelfUser.userId);
        }
    }

    public void FocusOnVillage()
    {
        transform.position = new Vector3(playerVillagePosition.x, transform.position.y, playerVillagePosition.y);

        zoom = 10f;
        
        float buildScale = zoom * 0.025f;
            
        buildMenu.localScale = new Vector3(buildScale, buildScale, buildScale);
            
        cam.orthographicSize = zoom;
        
    }
    
    private void CheckTile(int id)
    {
        byte buildingType = Grid._instance.tiles[id].building;

        InfoScreen._instance.UpdateInfoScreen(id);
        InfoScreen._instance.OpenInfoScreen();
        if (buildingType > 1)
        {
            // Random building
            
            InfoScreen._instance.OpenResourceInfoScreen();
            InfoScreen._instance.OpenInfoScreen();
            InfoScreen._instance.UpdateInfoScreenResource(id);
            
        }
        else if (buildingType == 1)
        {
            // Village building

            InfoScreen._instance.UpdateInfoScreenVillage(id);
            InfoScreen._instance.OpenInfoScreen();
            InfoScreen._instance.OpenVillageInfoScreen(id);
            //InfoScreen._instance.ToggleInfoScreen(true);
        }
        
    }
    public void PlaceBuildingOnSelectedTile(int buildingId/*, int selectedPosition = 0*/)
    {
        /*
        Vector2Int asdf = new Vector2Int((int)selectedTileHighlight.position.x, (int)selectedTileHighlight.position.z);
        int selectedPosition = Grid._instance.GetIdByVec(asdf);
        */
        int selectedPosition = Grid._instance.GetIdByVec(new Vector2Int((int)selectedTileHighlight.transform.position.x, (int)selectedTileHighlight.transform.position.z));
        Vector2Int villagePos = Grid._instance.GetPosition(LocalData.SelfUser.cityLocation);
        
        
        if (buildingId == 0) throw new Exception("A building id of 0 means no building. This method should not be called if building id is 0.");
        if (Grid._instance.tiles[selectedPosition].tileType == 1)
        {
            Debug.LogWarning("Tiletype 1 is water. No buildings can be built on water.");
            
            SplashText.Splash("You cannot build on water.");
            
            return;
        }
        if (Grid._instance.tiles[selectedPosition].tileType == 255)
        {
            Debug.LogWarning("Tried to construct building on construction");
            
            SplashText.Splash("A building is already here.");
            return;
        }
        if (Mathf.Abs(villagePos.x - selectedTileHighlight.transform.position.x) > 8 || Mathf.Abs(villagePos.y - selectedTileHighlight.transform.position.z) > 8)
        {
            Debug.LogWarning("Building too far away from Village!");
            
            SplashText.Splash("Too far from your Village.");
            return;
        }
        
        MapBuilding mapBuilding = MapBuildingDefinition.I[buildingId];

        int foodCost = mapBuilding.foodCost;
        int woodCost = mapBuilding.woodCost;
        int metalCost = mapBuilding.metalCost;
        int orderCost = mapBuilding.orderCost;

        if (foodCost >  GameManager.PlayerFood ||
            woodCost >  GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            if (foodCost > GameManager.PlayerFood)
            {
                GameManager.I.LackingResources("Food");
            }
            if (woodCost > GameManager.PlayerWood)
            {
                GameManager.I.LackingResources("Wood");
            }
            if (metalCost > GameManager.PlayerMetal)
            {
                GameManager.I.LackingResources("Metal");
            }
            if (orderCost > GameManager.PlayerOrder)
            {
                GameManager.I.LackingResources("Order");
            }
            return;
        }
                                                                                                        // BUG Remove division later
        ScheduledEvent scheduleBuilding = new ScheduledMapBuildEvent(MapBuildingDefinition.I[buildingId].buildingTime / 10, (byte)buildingId, selectedPosition, LocalData.SelfUser.userId);
        
        GameManager.PlayerFood -= mapBuilding.foodCost;
        GameManager.PlayerWood -= mapBuilding.woodCost;
        GameManager.PlayerMetal -= mapBuilding.metalCost;
        GameManager.PlayerOrder -= mapBuilding.orderCost;
        
        
        
        Debug.Log("" + mapBuilding.name + " was placed in location " + selectedPosition);
        
        
    }
    
    public void PlaceOtherBuilding(byte buildingId, int owner, int position)
    {
        //Vector2Int selectedPosition = new Vector2Int(UnityEngine.Random.Range(1, Grid._instance.width -1), UnityEngine.Random.Range(1, Grid._instance.height -1));
        //int position = Grid._instance.GetIdByVec(selectedPosition);
        
        if (buildingId == 0) throw new Exception("A building id of 0 means no building. This method should not be called if building id is 0.");
        if (Grid._instance.tiles[position].tileType == 1) throw new Exception("Tiletype 1 is water. No buildings can be built on water.");
        if (Grid._instance.tiles[position].tileType == 255)
        {
            Debug.Log("Tried to construct building on construction");
            return;
        }
        MapBuilding mapBuilding = MapBuildingDefinition.I[buildingId];

        if (owner == 1)
        {
            playerVillagePosition = Grid._instance.GetPosition(position);
        }
        
        
        
        
        int foodCost = mapBuilding.foodCost;
        int woodCost = mapBuilding.woodCost;
        int metalCost = mapBuilding.metalCost;
        int orderCost = mapBuilding.orderCost;

        if (foodCost > GameManager.PlayerFood ||
            woodCost > GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            Debug.LogWarning("Not enough resources");
            return;
        }
        // BUG Remove division later
        ScheduledEvent scheduleBuilding = new ScheduledMapBuildEvent(MapBuildingDefinition.I[buildingId].buildingTime / 10, buildingId, position, owner);
        
        GameManager.PlayerFood -= mapBuilding.foodCost;
        GameManager.PlayerWood -= mapBuilding.woodCost;
        GameManager.PlayerMetal -= mapBuilding.metalCost;
        GameManager.PlayerOrder -= mapBuilding.orderCost;
        
        
        
        Debug.Log("" + mapBuilding.name + " was placed in location " + position);
        
        
    }
    
     public void PlaceTestVillage(int owner)
    {
        //Vector2Int selectedPosition = new Vector2Int(UnityEngine.Random.Range(1, Grid._instance.width -1), UnityEngine.Random.Range(1, Grid._instance.height -1));
        //int position = Grid._instance.GetIdByVec(selectedPosition);
        
        MapBuilding mapBuilding = MapBuildingDefinition.I[1];
        
        int position;// = Grid._instance.GetIdByVec(vecPos);

        if (owner == 5)
        {
            position = Grid._instance.GetIdByVec(new Vector2Int(57,77));
        }    
        else if (owner == 6)
        {
            position = Grid._instance.GetIdByVec(new Vector2Int(64,71));
        }
        else if (owner == 7)
        {
            position = Grid._instance.GetIdByVec(new Vector2Int(50,66));
        }
        else if (owner == 8)
        {
            position = Grid._instance.GetIdByVec(new Vector2Int(75, 62));
        }
        else
            position = 12876;
        
        Debug.Log("Village " + owner + " was created at position " + position);
        
        Grid._instance.tiles[position].army = RandomEnemy();
        
        // BUG Remove division later
        ScheduledEvent scheduleBuilding = new ScheduledMapBuildEvent(MapBuildingDefinition.I[1].buildingTime / 10, 1, position, owner);
        
        
    }
}