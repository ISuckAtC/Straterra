using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverController : MonoBehaviour
{
    private UnityEngine.UI.GraphicRaycaster gr;

    public GameObject hoverWindow;

    public TMP_Text headerText;
    public TMP_Text breadText;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
        {
            if (RaycastAll() != null)
            {
                if (!hoverWindow.activeSelf)
                {
                    Hover hover = RaycastAll();
                    
                    
                }
            }
        }
        else
        {
            
        }
    }

    private void Toggle(bool enable)
    {
        if (enable)
        {
            hoverWindow.SetActive(true);
            return;
        }
        
        hoverWindow.SetActive(false);
    }
    
    private Hover RaycastAll()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        List<RaycastResult> rrs = new List<RaycastResult>();
            
        gr.Raycast(ped, rrs);

        if (rrs.Count > 0 && rrs[0].gameObject.GetComponent<Hover>() != null)
        {
            // Canvas hover
            return rrs[0].gameObject.GetComponent<Hover>();

        }
        else if (rrs.Count == 0 && Physics.Raycast(ray, out hit))
        {
            // Worldspace hover
            if (hit.transform.GetComponent<Hover>() != null)
            {
                return hit.transform.GetComponent<Hover>();
            }
        }

        return null;
    }
    
}
