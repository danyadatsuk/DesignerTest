using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBase", menuName = "Snake/LevelBase")]
public class LevelsBase : ScriptableObject
{   
    public LevelConfiguration[] levels;
}
