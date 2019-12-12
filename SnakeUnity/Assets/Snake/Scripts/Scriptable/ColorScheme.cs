using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorScheme_", menuName = "Snake/ColorScheme")]
public class ColorScheme : ScriptableObject
{
    [Header("Background color")]
    public Color cameraColor;
    public Color ssaoColor;

    [Header("Level Color")]
    public Color textColor;
    public Color textLineColor;
    public Color floorColor;
    public Color borderColor;

    [Header("Object Color")]
    public Color objectNetralColor1;
    public Color objectNetralColor2;
    public Color objectNetralColor3;
    public Color objectAssaultColor;

    [Header("Snake Color")]
    public Color snakeColor;
    public Color feedColor;
}
