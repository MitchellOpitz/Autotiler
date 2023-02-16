using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine.Tilemaps;

#if (UNITY_EDITOR)

[ExecuteInEditMode]
[CreateAssetMenu(menuName = "AutoTiler_Menu_Name", fileName = "Default_Autotiler_File_Name")]

public class Autotiler_SO : ScriptableObject
{

    [SerializeField]
    List<Texture2D> tilemaps;

    [SerializeField]
    RuleTile ruleTileTemplate;

    private RuleTile newRuleTile;
    private int numberOfRules;

    public void CreateRuleTiles()
    {

        if (CheckRuleTemplate()  && CompareTilemapsToRuleTile())
        {
            CopyRuleTile();
            if (tilemaps.Count == 1)
            {
                SingleTilemap();
            } else
            {
                RandomTilemaps();
            }
            CreateAsset();
        }
    }

    private bool CheckRuleTemplate()
    {
        // Error handling if no rule tile provided.
        if (ruleTileTemplate == null)
        {
            Debug.LogError("Rule tile not assigned.");
            return false;
        } else
        {
            // Debug.Log("Rule tile is assigned.");
            return true;
        }
    }

    private bool CompareTilemapsToRuleTile()
    {
        // Check number of sprites matches the number of rules.
        numberOfRules = ruleTileTemplate.m_TilingRules.Count;

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
                return false;
            }

        }
        // Debug.Log("All tilemaps match provided Rule Tile.");
        return true;
    }

    private void CopyRuleTile()
    {
        newRuleTile = CreateInstance<RuleTile>();
        EditorUtility.CopySerialized(ruleTileTemplate, newRuleTile);
        // Debug.Log("Rule Tile duplicated.");
    }

    private void SingleTilemap()
    {

        // Convert Tiling Rule Output to "Single".
        foreach (RuleTile.TilingRule rule in newRuleTile.m_TilingRules)
        {
            rule.m_Output = RuleTile.TilingRule.OutputSprite.Single;
            rule.m_Sprites = new Sprite[] { rule.m_Sprites[0] };
            // Debug.Log("Converted Output rule to 'Single'.");
        }

        // Set all sprites to the Rule Tile.
        string spriteSheetName = AssetDatabase.GetAssetPath(tilemaps[0]);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetName).OfType<Sprite>().OrderBy(s => s.name).ToArray();

        for (int i = 0; i < numberOfRules; i++)
        {
            // Debug.Log("Sprite at position: " + i + " = " + sprites[i] + ".");
            newRuleTile.m_TilingRules[i].m_Sprites[0] = sprites[i];
        }

        // Debug.Log("Single tilemap created.");
    }

    private void RandomTilemaps()
    {
        // Debug.Log("Random Tilemaps started.");

        // Convert Tiling Rule Output to "Single".
        foreach (RuleTile.TilingRule rule in newRuleTile.m_TilingRules)
        {
            rule.m_Output = RuleTile.TilingRule.OutputSprite.Random;
            rule.m_Sprites = new Sprite[tilemaps.Count];
            // Debug.Log("Converted Output rule to 'Random' with size of " + tilemaps.Count + ".");
        }

        for (int i = 0; i < tilemaps.Count; i++)
        {
            Texture2D tilemap = tilemaps[i];
            string spriteSheetName = AssetDatabase.GetAssetPath(tilemap);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetName).OfType<Sprite>().OrderBy(s => s.name).ToArray();
            for (int j = 0; j < numberOfRules; j++)
            {
                newRuleTile.m_TilingRules[j].m_Sprites[i] = sprites[j];
            }

        }
    }

    private void CreateAsset()
    {
        AssetDatabase.CreateAsset(newRuleTile, AssetDatabase.GetAssetPath(this));
        // Debug.Log("New Rule Tile asset created successfully.");
    }
}

#endif