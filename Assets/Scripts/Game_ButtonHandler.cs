using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_ButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelNum levelNum = GameObject.Find("LevelNumber").GetComponent<LevelNum>();
        if (levelNum.lvlCounter <= 30)
        {
            Debug.Log("Level is less than 30");
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
