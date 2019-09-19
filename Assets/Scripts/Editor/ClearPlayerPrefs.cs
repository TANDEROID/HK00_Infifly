using UnityEngine;
using UnityEditor;

public class ClearPlayerPrefs {

    [MenuItem("Tools/Clear PlayerPrefs")]
	public static void ClearPP()
    {
        PlayerPrefs.DeleteAll();
    }
}