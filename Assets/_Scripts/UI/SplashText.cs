using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashText : MonoBehaviour
{
    static public SplashText I;
    private TMPro.TMP_Text text;
    public float fadeTime;
    private float currentFade;

    private float eVal;
    
    public AnimationCurve fadeCurve;
    
    public static void Splash(string message)
    {
        I.text.text = message;
        I.currentFade = I.fadeTime;
    }

    void Start()
    {
        if (I) Destroy(this);
        I = this;
        text = GetComponent<TMPro.TMP_Text>();
        //fadeCurve = gameObject.AddComponent<AnimationCurve>();
            //GetComponent<AnimationCurve>();
    }
    
    // Update is called once per frame
    void Update()
    {
        eVal = currentFade / fadeTime;

        Color color = text.color;
        color.a = (fadeCurve.Evaluate(eVal));
        text.color = color;
        if (currentFade > 0.01f)
            currentFade -= Time.deltaTime;
        else
            currentFade = 0.0001f;
    }
}
