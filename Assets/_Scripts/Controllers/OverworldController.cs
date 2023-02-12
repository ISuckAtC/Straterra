using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

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

    
    
    void Start()
    {
        Grid.onReady += OnGridReady;


    }

    private void OnGridReady()
    {
        cam = GetComponent<Camera>();

        holdCounter = holdDelay;

        cam.orthographicSize = zoom;

        int startingposition = FindStartingPosition.FirstVillage();
        
        PlaceBuilding(1, startingposition);

        Vector2 cameraposition = Grid._instance.GetPosition(startingposition);
        
        transform.position = new Vector3(cameraposition.x, 98f, cameraposition.y);

        Player selfPlayer = LocalData.SelfPlayer;
        
        selfPlayer.cityLocation = startingposition;

        LocalData.SelfPlayer = selfPlayer;
        
        
        int enemyposition = FindStartingPosition.FirstVillage();
        PlaceBuilding(1, enemyposition);

        List<Group> enemyArmy = new List<Group>();

        enemyArmy.Add(new Group(5, 1));
        enemyArmy.Add(new Group(5, 2));
        enemyArmy.Add(new Group(5, 3));

        Grid._instance.tiles[enemyposition].army = enemyArmy;


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
            building = 1;
            buildingIndex = 1;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // House
        {
            building = 2;
            buildingIndex = 100;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Castle
        {
            building = 3;
            buildingIndex = 110;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
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

        // Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            holding = true;
            transform.position -= new Vector3(cam.orthographicSize * moveSpeedTap, 0f, 0f);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            holding = false;

        // Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            holding = true;
            transform.position += new Vector3(cam.orthographicSize * moveSpeedTap, 0f, 0f);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            holding = false;

        // Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            holding = true;
            transform.position += new Vector3(0f, 0f, cam.orthographicSize * moveSpeedTap);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            holding = false;

        // Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            holding = true;
            transform.position -= new Vector3(0f, 0f, cam.orthographicSize * moveSpeedTap);
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
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(movementAmount, 0f, 0f) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0f, 0f, movementAmount) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0f, 0f, movementAmount) * cam.orthographicSize * moveSpeedHold * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            transform.position -= new Vector3(Input.GetAxisRaw("Mouse X"), 0f, Input.GetAxisRaw("Mouse Y")) * 2f;
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

                cam.orthographicSize = zoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (zoom > 12f)
                zoom -= 10f;

            if (zoom <= 2.1f)
                zoom = 2.1f;

            cam.orthographicSize = zoom;
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (zoom < 108f)
                zoom += 10f;

            if (zoom >= 119.9f)
                zoom = 119.9f;

            cam.orthographicSize = zoom;
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Place village.
        {
            if (Physics.Raycast(ray, out hit))
            {
                int position = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));

                PlaceBuilding(buildingIndex, position);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                int id = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));
                InfoScreen._instance.ToggleInfoScreen(false);
                InfoScreen._instance.ToggleInfoScreenResource(false);
                CheckTile(id);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoScreen._instance.ToggleInfoScreen(false);
            InfoScreen._instance.ToggleInfoScreenResource(false);
            //InfoScreen._instance.ToggleInfoScreen(false);
        }
    }

    private void CheckTile(int id)
    {
        byte buildingType = Grid._instance.tiles[id].building;

        if (buildingType > 1)
        {
            // Random building
            InfoScreen._instance.ToggleInfoScreenResource(true);
            InfoScreen._instance.ToggleInfoScreen(true);
            InfoScreen._instance.UpdateInfoScreenResource(id);
            
        }
        else if (buildingType == 1)
        {
            // Village building

            InfoScreen._instance.UpdateInfoScreenVillage(id);
            InfoScreen._instance.ToggleInfoScreen(true);
        }
        else
        {
            // No building -> Show tile resources
            InfoScreen._instance.UpdateInfoScreen(id);
            InfoScreen._instance.ToggleInfoScreen(true);
        }
    }
    public void PlaceBuilding(byte buildingId, int position)
    {
        if (buildingId == 0) throw new Exception("A building id of 0 means no building. This method should not be called if building id is 0.");

        MapBuilding mapBuilding = MapBuildingDefinition.I[buildingId];

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
        ScheduledEvent scheduleBuilding = new ScheduledMapBuildEvent(MapBuildingDefinition.I[buildingId].buildingTime / 10, buildingId, position);
        
        GameManager.PlayerFood -= mapBuilding.foodCost;
        GameManager.PlayerWood -= mapBuilding.woodCost;
        GameManager.PlayerMetal -= mapBuilding.metalCost;
        GameManager.PlayerOrder -= mapBuilding.orderCost;
        
        Debug.Log("" + mapBuilding.name + " was placed in location " + position);
    }
}