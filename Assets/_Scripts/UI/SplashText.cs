using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashText : MonoBehaviour
{
    static public SplashText I;
    public float splashDuration;
    public TMPro.TMP_Text text;
    public float fadeTime;
    private float currentFade;

    public static void Splash(string message)
    {
        I.text.text = message;
        I.currentFade = I.fadeTime;
    }

    void Start()
    {
        if (I) Destroy(this);
        I = this;
    }
    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        color.a = ((1f / fadeTime) * currentFade);
        text.color = color;
        currentFade -= Time.deltaTime;
    }
}
