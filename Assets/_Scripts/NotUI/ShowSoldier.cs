using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSoldier : MonoBehaviour
{
    public int soldierAmount;
    public Vector2 bounds;
    public GameObject soliderPrefab;
    public Transform soldierParent;
    public bool lookAtCamera;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < soldierAmount; ++i)
        {
            Vector3 position = soliderPrefab.transform.position + new Vector3(Random.Range(-bounds.x, bounds.x), 0f, Random.Range(-bounds.y, bounds.y));
            Vector3 direction = (Camera.main.transform.position - position).normalized;
            direction.x = 0;
            direction = direction.normalized;
            GameObject spawned = Instantiate(soliderPrefab, position, !lookAtCamera ? Quaternion.identity : Quaternion.LookRotation(direction));
            spawned.transform.parent = soldierParent;
        }
    }
}
