using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNum : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public uint lvlCounter = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = string.Format("LEVEL {0}", lvlCounter.ToString());

    }
}
