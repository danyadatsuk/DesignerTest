#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeleteDataTools : MonoBehaviour
{
    [MenuItem("Snake/ClearData")]
    private static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif