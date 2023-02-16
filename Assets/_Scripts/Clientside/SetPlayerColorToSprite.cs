using UnityEngine;
using UnityEngine.UI;

public class SetPlayerColorToSprite : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().color = GameManager.PlayerColor;
        
        Destroy(this);
    }
}
