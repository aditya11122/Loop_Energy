using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class Levels : ScriptableObject
{
    
    public List<TextAsset> LevelBasedTextAssets;
    
       
    

    //Returns the Text asset based on the level selected.
    public TextAsset returnLevelData(int level)
    {
        TextAsset levelInfo = LevelBasedTextAssets[0];
        if (level!= 0 || level > 5)
        {
            levelInfo = LevelBasedTextAssets[level - 1];
            
        }
        return levelInfo;
    }

    
}
