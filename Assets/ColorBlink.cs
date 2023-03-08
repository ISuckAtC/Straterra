using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlink : MonoBehaviour
{
    public float speed = 10;
    private TMPro.TMP_Text text;
    private Image exclamationPoint;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
        exclamationPoint = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = exclamationPoint.color;
        color.a = Mathf.PingPong(Time.time * speed, 1);
        exclamationPoint.color = color;
    }
}
