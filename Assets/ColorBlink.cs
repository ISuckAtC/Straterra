using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlink : MonoBehaviour
{
    public float speed = 10;
    private TMPro.TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        color.a = Mathf.PingPong(Time.time * speed, 1);
        text.color = color;
    }
}
