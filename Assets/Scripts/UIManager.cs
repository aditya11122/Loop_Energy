using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerializableClasses;
using System.IO;
using UnityEditor;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

public class UIManager : MonoBehaviour
{
        

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI LevelText;

    public GameUIInfo GameUIObj;

    private void Awake()
    {
        GameUIObj = new GameUIInfo();

        LevelInfoFromJson();

        UpdateUI();
    }

    
    //Load the game stats relatd data when the game launches
    public void LevelInfoFromJson()
    {

        if (File.Exists(Application.persistentDataPath
                   + "/GameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/GameData.dat", FileMode.Open);
            GameUIInfo data = (GameUIInfo)bf.Deserialize(file);
            file.Close();

            GameUIObj = data;
            Debug.Log("Game data loaded!");
        }

      





    }

    //Update the UI right after parsing the GameInfo class
    public void UpdateUI()
    {
       
        scoreText.text = "Score: " + GameUIObj.score.ToString();
        LevelText.text = "Level: " + GameUIObj.level.ToString();
    }

    //Save the JSon game data when the game quits
    public void SendUIDataToJson()
    {
        string destination = "Assets/Resources/GameUI/GameData.txt";


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/GameData.dat");

        bf.Serialize(file, GameUIObj);

        file.Close();
      
    }

    
}


