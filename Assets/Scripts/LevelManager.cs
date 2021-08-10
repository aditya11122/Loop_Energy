using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using SerializableClasses;
using UnityEngine.SceneManagement;

//This script is related to the level generation


#if UNITY_EDITOR
[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
    public GameObject zone;

    public bool trigger;
    public string levelName;

    public List<Vector3> positions_zone;

    public Level LevelTileInfo;

    List<Vector3> position_list = new List<Vector3>();

    
    void PositionsStoring()
    {
        
        for (int i = 0; i < zone.transform.childCount; i++)
        {
            position_list.Add(zone.transform.GetChild(i).position);

        }
        
        PositionConversions();
    }

  

    void PositionConversions()
    {

        List<PositionsVector3> posList = new List<PositionsVector3>();

            
        for (int r=0; r< position_list.Count; r++)
        {
            PositionsVector3 pos = new PositionsVector3();

            pos.puzzleNumber = zone.transform.GetChild(r).name;

            pos.PositionX = position_list[r].x;
            pos.PositionY = position_list[r].y;
            pos.PositionZ = position_list[r].z;
            
            posList.Add(pos);
        }

        LevelTileInfo.Positions = posList.ToList();
       
        

        SendToJson();
    }

    void SendToJson()
    {
        var json = JsonUtility.ToJson(LevelTileInfo);
        string destination = "Assets/Resources/TileInfo/TileInfo-"+ levelName +".txt";
        StreamWriter writer = new StreamWriter(destination, true);
        writer.Write(json);
        writer.Close();
        AssetDatabase.ImportAsset(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger)
        {
            trigger = false;
            LevelTileInfo = new Level();
            positions_zone = new List<Vector3>();
            PositionsStoring();
        }
    }
}
#endif
