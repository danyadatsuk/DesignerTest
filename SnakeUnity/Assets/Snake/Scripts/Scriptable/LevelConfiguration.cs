using UnityEngine;

[CreateAssetMenu(fileName = "ConfigLevel_", menuName = "Snake/ConfigLevel")]
public class LevelConfiguration : ScriptableObject
{
    public int countPartToWin;
    public GameObject prefab;

    public ColorScheme colorScheme;
}
