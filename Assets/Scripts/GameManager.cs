using SerializableClasses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Prefab references
    public GameObject PlacerObject;
    public GameObject HolderObject;

    //Which level to load
    Levels LevelToLoad;

    //Materials list for holder and placer objects
    List<Material> HolderMaterials;
    List<Material> PlacerMaterials;

    //variables related to touch input and drag drop functionality
    Vector3 mousediff;
    Vector3 prevMousePos;
    Vector3 newMousePos;

    //list of all the placer and holder objects
    List<GameObject> PlacerList;
    List<GameObject> HolderList;

    //For every case we determine the target and pawn which will be the main interaction with
    public GameObject Target;
    public GameObject pawnHandler;

    bool objectSelected;
    Vector3 defaultposition;

    public int levelToSpawn;

    public List<PositionsVector3> spawningPos = new List<PositionsVector3>();
    private Level levelTileInfo = new Level();
    
    //UI data
    public int Score;
    public int level;

    //scriptable object referencing to levels
    public Levels LevelsScriptableObject;

    int counter;

    UIManager UImanagerref;
    
    //Initialisations
    void Start()
    {
        UImanagerref = GetComponent<UIManager>();

        HolderMaterials = new List<Material>();
        PlacerMaterials = new List<Material>();

        PlacerList = new List<GameObject>();
        HolderList = new List<GameObject>();

        LevelUpdate(UImanagerref.GameUIObj.level);

         counter = 0;

        Score = UImanagerref.GameUIObj.score;
        

        //GetComponent<GameManager>().Score = GameUIObj.score;
        //GetComponent<GameManager>().levelToSpawn = GameUIObj.level;

        defaultposition = PlacerObject.transform.position;

        MaterialLoader();

        GetTileInfoFromJson(UImanagerref.GameUIObj.level);
    }

    //Assigning the level update throughout the relevant variables
    public void LevelUpdate(int levelGiven)
    {
        levelToSpawn = levelGiven;
        level = levelToSpawn;
        UImanagerref.LevelText.text = "Level: " + levelToSpawn.ToString();
    }

    //Loading materials and storing them
    void MaterialLoader()
    {
        for (int q = 1; q < 41; q++)
        {
            Material mat = Resources.Load<Material>("CriticalMaterials/HolderMaterial/" + q.ToString());
            HolderMaterials.Add(mat);

            Material mat2 = Resources.Load<Material>("CriticalMaterials/PlacerMaterial/" + q.ToString());
            PlacerMaterials.Add(mat2);
        }
        
    }


    public void GetTileInfoFromJson(int levelToSpawn)
    {
        TextAsset info = LevelsScriptableObject.returnLevelData(levelToSpawn);

        levelTileInfo = JsonUtility.FromJson<Level>(info.text);
        spawningPos = levelTileInfo.Positions;

        SpawnLevel(levelToSpawn);

    }

    
    //Spawn the level based on the level selected
    void SpawnLevel(int levelToSpawn)
    {
        Debug.Log("Tiles count: " + levelTileInfo.Positions.Count);
        for (int r = 0; r < levelTileInfo.Positions.Count; r++)
        {
            Vector3 positionSpawn = new Vector3(levelTileInfo.Positions[r].PositionX, levelTileInfo.Positions[r].PositionY, levelTileInfo.Positions[r].PositionZ);
            GameObject TileObj = Instantiate(HolderObject,positionSpawn, Quaternion.identity);
            string materialName = levelTileInfo.Positions[r].puzzleNumber;
            Debug.Log("Puzzle number: " + materialName);
            TileObj.GetComponent<Renderer>().material = HolderMaterials.Find(x => x.name == materialName);

            HolderList.Add(TileObj);
        }

        PawnCreator(counter);

    }

    //This is where we create the pawn to be placed on the pattern
    void PawnCreator(int counter)
    {
        //Initial thing
        Target = HolderList[counter];
        pawnHandler = Instantiate(PlacerObject);
        pawnHandler.transform.tag = "draggable";
        pawnHandler.GetComponent<Renderer>().material = PlacerMaterials.Find(x => x.name == levelTileInfo.Positions[counter].puzzleNumber);

        PlacerList.Add(pawnHandler);
       
    }

    void Update()
    {
        //****Drag and drop functionality*******

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.tag == "draggable")
                {
                    Debug.Log("Object selected");
                    objectSelected = true;

                }
            }

        }

        if(Input.GetMouseButton(0))
        {
            
            mousediff = Input.mousePosition - prevMousePos;
                
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo,45))
            {
                Debug.Log("Ray running on the plane");
                Debug.DrawRay(Camera.main.transform.position, (hitInfo.point - Camera.main.transform.position)*30, Color.black);
               
                Debug.Log("base has been selected");
                if (objectSelected)
                {
                    Debug.Log("Object should move..!!");
                    Vector3 hitPosition = hitInfo.point;
                    pawnHandler.transform.position = new Vector3(hitPosition.x, 0.16f, hitPosition.z);

                }
                
            }
            
        }

        //When the object has been placed and the tap is relieved
        if (Input.GetMouseButtonUp(0))
        {
            objectSelected = false;

            if(Vector3.Distance(Target.transform.position, pawnHandler.transform.position)< 0.5f)
            {
                pawnHandler.transform.position = new Vector3(Target.transform.position.x, 0.06f, Target.transform.position.z);

                pawnHandler.transform.tag = "placed";

                if (counter < (levelTileInfo.Positions.Count-1))
                {
                    counter += 1;
                    PawnCreator(counter);

                    //Score Update
                    Score += 5;
                    UImanagerref.scoreText.text = "Score: "+Score.ToString() ;
                    
                }
                else//This is the situation for when the level has been completed
                {
                    counter = 0;

                    LevelComplete();
                }
                
            }
            else
            {
                pawnHandler.transform.position = defaultposition;

            }
            
        }
    }

    //Functionality when level completes
    void LevelComplete()
    {
        
        if(levelToSpawn < 5)
        {
            levelToSpawn += 1;

            UImanagerref.LevelText.text = "LEVEL :" + levelToSpawn.ToString();
            level = levelToSpawn;
            ClearLevel();

            GetTileInfoFromJson(levelToSpawn);

            //SpawnLevel(levelToSpawn);
        }

        UImanagerref.SendUIDataToJson();
        
    }

    //Function used to respawn the level when entering another level
    public void ClearLevel()
    {
        for(int i=0; i< HolderList.Count; i++)
        {
            Destroy(HolderList[i]);
        }
        HolderList.Clear();

        for(int t=0; t< PlacerList.Count; t++)
        {
            Destroy(PlacerList[t]);
        }
        PlacerList.Clear();
    }

    //This is to rightly send the data 
    void SerialiseTheUIData()
    {
        UImanagerref.GameUIObj.score = Score;
        UImanagerref.GameUIObj.level = level;

        UImanagerref.SendUIDataToJson();
    }

    //Serialise the data when the application quits
    private void OnApplicationQuit()
    {
        SerialiseTheUIData();
    }
}
