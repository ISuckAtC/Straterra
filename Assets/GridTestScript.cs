using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int size = Random.Range(0, (Grid._instance.width * Grid._instance.height));

            Vector2 position = Grid._instance.GetPosition(size);
            
            

            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.transform.position = new Vector3(/*position.x*/ 1f, 1f, /*position.y*/ 1f);
            temp.GetComponent<MeshRenderer>().material.color = Color.red;
            
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            int x = Random.Range(0, Grid._instance.width);
            int y = Random.Range(0, Grid._instance.height);
            
            Debug.Log(Grid._instance.GetId(x, y));
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("XY: " + Grid._instance.GetPositionFromRaycast(hit.point));
            }
        }
    }
}
