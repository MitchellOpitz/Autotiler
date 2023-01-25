using UnityEngine;
using System.Collections.Generic;

#if (UNITY_EDITOR)

[ExecuteInEditMode]
[CreateAssetMenu(menuName = "AutoTiler_Menu_Name", fileName = "Default_Autotiler_File_Name")]

public class Autotiler_SO : ScriptableObject
{
    [SerializeField]
    List<Texture2D> tilemaps;

    [SerializeField]
    RuleTile ruleTileTemplate;

    public void CreateRuleTiles()
    {
        Debug.Log("Button worked!");
    }
}

#endif