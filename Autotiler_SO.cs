using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

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

        RuleTile newRuleTile = CreateInstance<RuleTile>();
        EditorUtility.CopySerialized(ruleTileTemplate, newRuleTile);

        // Check number of sprites matches the number of rules.
        int numberOfRules = ruleTileTemplate.m_TilingRules.Count;

        foreach (Texture2D tilemap in tilemaps)
        {
            string spriteSheetName = AssetDatabase.GetAssetPath(tilemap);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetName).OfType<Sprite>().ToArray();
            // Debug.Log("Sprite sheet: " + tilemap.name + " | Sprites contained: " + sprites.Length);

            if (sprites.Length != numberOfRules)
            {
                Debug.LogError("Tilemap does not contain the number of sprites needed by Rule Tile template.");
                Debug.LogError("Sprite sheet: " + tilemap.name + " | Sprites contained: " + sprites.Length);
                Debug.LogError("Rule Tile template: " + ruleTileTemplate + " | Rules contained: " + numberOfRules);
                return;
            }

        }

        if (tilemaps.Count == 1)
        {
            string spriteSheetName = AssetDatabase.GetAssetPath(tilemaps[0]);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetName).OfType<Sprite>().ToArray();

            for (int i = 0; i < numberOfRules; i++)
            {
                newRuleTile.m_TilingRules[i].m_Sprites[0] = sprites[i];
            }
        } else
        {
            foreach (Texture2D tilemap in tilemaps)
            {
                int position = tilemaps.IndexOf(tilemap);
                string spriteSheetName = AssetDatabase.GetAssetPath(tilemap);
                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetName).OfType<Sprite>().ToArray();

                for (int i = 0; i < numberOfRules; i++)
                {
                    newRuleTile.m_TilingRules[i].m_Sprites[position] = sprites[i];                    
                }
            }
        }

        Debug.Log("Successful!");
        AssetDatabase.CreateAsset(newRuleTile, AssetDatabase.GetAssetPath(this));
    }
}

#endif