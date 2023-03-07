using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverController : MonoBehaviour
{
    public UnityEngine.UI.GraphicRaycaster gr;

    public GameObject hoverWindow;

    public TMP_Text headerText;
    public TMP_Text bodyText;

    public Vector3 mousePos;

    [Range (0, 1)] public float buttonAlpha;

    private int hoverSizeX;
    private int hoverSizeY;
    private int xOffset;
    private int yOffset;

    void Start()
    {
        hoverWindow.SetActive(false);

        hoverSizeX = (int)(hoverWindow.GetComponent<RectTransform>().sizeDelta.x / 2) + 10;
        hoverSizeY = (int)(hoverWindow.GetComponent<RectTransform>().sizeDelta.y / 2) + 10;


        xOffset = hoverSizeX;
        yOffset = hoverSizeY;
    }

    void FixedUpdate()
    {
        mousePos = Input.mousePosition;
        RaycastAll();
    }


    private void RaycastAll()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        List<RaycastResult> rrs = new List<RaycastResult>();

        gr.Raycast(ped, rrs);


        if (rrs.Count > 0)
        {
            for (int i = 0; i < rrs.Count; i++)
            {
                if (rrs[i].gameObject.layer == 8)
                {
                    rrs[i].gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = buttonAlpha;
                    
                    // Canvas hover
                    FoundHit(rrs[i].gameObject.GetComponent<Hover>());
                    break;
                }
                else if (i == rrs.Count - 1)
                {
                    Toggle(false);

                    Debug.Log("Disabled here.");
                }
            }
        }
        else if (rrs.Count == 0 && Physics.Raycast(ray, out hit))
        {
            // Worldspace hover
            if (hit.transform.gameObject.layer == 8) //GetComponent<Hover>() != null)
            {
                FoundHit(hit.transform.GetComponent<Hover>());
            }
            else
            {
                Toggle(false);
            }
        }
        else
        {
            Toggle(false);
        }

        if (hoverWindow.activeSelf)
        {
            SetOffset();
            ((RectTransform)hoverWindow.transform).position = new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, 0);
        }
    }

    private void SetOffset()
    {
        if (Input.mousePosition.x > Screen.width - (hoverSizeX * 2))
            xOffset = -hoverSizeX;
        else
            xOffset = hoverSizeX;

        if (Input.mousePosition.y < hoverSizeY * 2)
            yOffset = hoverSizeY;
        else
            yOffset = -hoverSizeY;
    }

    private void FoundHit(Hover hover)
    {
        ((RectTransform)hoverWindow.transform).position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        headerText.text = hover.headerText;
        bodyText.text = hover.bodyText;

        Toggle(true);
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
}