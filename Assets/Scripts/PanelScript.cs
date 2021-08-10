using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{

    GameObject Manager_obj;
    public GameObject panelObject;

    private void Start()
    {
        Manager_obj = GameObject.Find("GameManager");
    }

    //Level button functionality
    public void levelButton(int level)
    {
        Manager_obj.GetComponent<GameManager>().ClearLevel();
        Manager_obj.GetComponent<GameManager>().GetTileInfoFromJson(level);
        Manager_obj.GetComponent<GameManager>().LevelUpdate(level);

        panelObject.gameObject.SetActive(false);
    }

    public void HomeButtonFunctionality()
    {
        panelObject.gameObject.SetActive(true);
    }
}
