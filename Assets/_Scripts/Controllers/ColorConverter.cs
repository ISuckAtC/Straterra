using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorConverter : MonoBehaviour
{
    public static ColorConverter I;
    
    void Start()
    {
        if (I == null)
            I = this;
        else
            Destroy(this);

        List<GameObject> cubes = new List<GameObject>();
        Color[] colors = new Color[8];

        colors[0] = IntToColor(ColorToInt(Color.cyan));
        colors[1] = IntToColor(ColorToInt(Color.blue));
        colors[2] = IntToColor(ColorToInt(Color.gray));
        colors[3] = IntToColor(ColorToInt(Color.yellow));
        colors[4] = IntToColor(ColorToInt(Color.white));
        colors[5] = IntToColor(ColorToInt(Color.green));
        colors[6] = IntToColor(ColorToInt(Color.magenta));
        colors[7] = IntToColor(ColorToInt(Color.black));
        
        for (int i = 0; i < 8; i++)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp.transform.position = new Vector3(i, 0, 0);

            temp.GetComponent<MeshRenderer>().material.color = colors[i];
            
            cubes.Add(temp);
        }
        /*
        Debug.Log(IntToColor(255255255));
        Debug.Log(ColorToInt(Color.cyan));
        Debug.Log(IntToColor(123055099));
        Debug.Log(ColorToInt(Color.yellow));
        Debug.Log(IntToColor(025025025));
        Debug.Log(ColorToInt(Color.black));
        */
    }

    public Color IntToColor(int intColor)
    {
        System.Drawing.Color convertedColor = System.Drawing.Color.FromArgb(intColor);
        
        /*
        int r = (intColor >> 16) & 0xff;
        int g = (intColor >> 8)  & 0xff;
        int b = (intColor)       & 0xff;
        */

        Color color = new Color((float)convertedColor.R/* / 255*/, (float)convertedColor.G/* / 255*/, (float)convertedColor.B/* / 255*/, (float)convertedColor.A);
        
        return color;
    }

    public int ColorToInt(Color color)
    {
        //System.Drawing.Color convertedColor = System.Drawing.Color.ToArgb(color);
     
        System.Drawing.Color convertedColor = System.Drawing.Color.FromArgb((byte)color.a * 255, (byte)color.r * 255,(byte)color.g * 255,(byte)color.b * 255);//new System.Drawing.Color());

        //System.Drawing.Color finalColor = 
        
            //.R = color.r * 255;
        
        // color.r * 255, color.g * 255, color.b * 255
        
        //int r = (int)(color.r * 0xff);
        //int g = (int)(color.g * 0xff);
        //int b = (int)(color.b * 0xff);

        int intColor = (convertedColor.A & 0xff) << 24 | (convertedColor.R & 0xff) << 16 | (convertedColor.G & 0xff) << 8 | (convertedColor.B & 0xff);

        return intColor;
    }
}
