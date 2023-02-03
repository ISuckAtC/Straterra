using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    void Start()
    {
        cam = GetComponent<Camera>();

        holdCounter = holdDelay;
        
        cam.orthographicSize = zoom;
    }

    void Update()
    {
        // Building Selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Village
        {
            building = 1;
            buildingIndex = 1;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))   // House
        {
            building = 2;
            buildingIndex = 50;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))   // Castle
        {
            building = 3;
            buildingIndex = 60;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))   // Farm
        {
            building = 4;
            buildingIndex = 10;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))   // Logging Camp
        {
            building = 5;
            buildingIndex = 20;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))   // Mine
        {
            building = 6;
            buildingIndex = 30;
            UIController._instance.SetBuildingImage(buildingIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))   // Darkshrine
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
        
        if (Input.GetKeyDown(KeyCode.Q))    // Place village.
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                PlaceTiles._instance.PlaceBuilding(buildingIndex, building, new Vector2Int((int)hit.point.x, (int)hit.point.z));
                
                Debug.Log("" + hit.point + PlaceTiles.tilePivot);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                int id = Grid._instance.GetIdByVec(new Vector2(hit.point.x + PlaceTiles.tilePivot.x, hit.point.z + PlaceTiles.tilePivot.y));
                CheckTile(id);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            InfoScreen._instance.ToggleInfoScreen(false);
        }
    }

    private void CheckTile(int id)
    {
        byte buildingType = Grid._instance.tiles[id].building;
        
        if (buildingType > 1)
        {   // Random building
            InfoScreen._instance.UpdateInfoScreenBuilding(id);
            InfoScreen._instance.ToggleInfoScreen(true);
            
        }
        else if (buildingType == 1)
        {   // Village building
            
            InfoScreen._instance.UpdateInfoScreenVillage(id);
            InfoScreen._instance.ToggleInfoScreen(true);
            
        }
        else
        {   // No building -> Show tile resources
            InfoScreen._instance.UpdateInfoScreen(id);
            InfoScreen._instance.ToggleInfoScreen(true);
        }

    }
    
}