using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


public class CreateLevel : MonoBehaviour
{
    // Text Files
    public TextAsset level1to30;
    // Prefabs
    public GameObject plusPrefab;
    public GameObject minusPrefab;
    public GameObject numberPrefab;
    //Game Object
    [HideInInspector] public GameObject newObject;
    public GameObject levelComplete_ui;
    public bool hasrun = false;
    string getLevelString(uint level)
    {
        string fs = level1to30.text;
        string[] fLines = fs.Split('\n', '\0', '\r').Where(x=> !string.IsNullOrEmpty(x)).ToArray();
        return fLines[level-1]; 
    }

    public void generateLevel()
    {
        
        Debug.Log("Generating equation");
        LevelNum levelNum = transform.parent.GetComponentInChildren<LevelNum>();
        uint level = levelNum.lvlCounter;
        string equation = getLevelString(level);
        //foreach (char c in equation)
        for (int c = 0; c < equation.Length; c++) 
        {
            if (equation[c] == '-')
            {
                newObject = Instantiate(minusPrefab, new Vector2(0, 0), Quaternion.identity);
                newObject.transform.SetParent(GameObject.Find("GRID").transform);
                newObject.transform.SetAsLastSibling();
            }
            else if (equation[c] == '+')
            {
                newObject = Instantiate(plusPrefab, new Vector2(0, 0), Quaternion.identity);
                newObject.transform.SetParent(GameObject.Find("GRID").transform);
                newObject.transform.SetAsLastSibling();
            }
            else if (equation[c]== '=')
            {
                newObject = Instantiate(plusPrefab, new Vector2(0, 0), Quaternion.identity);
                newObject.GetComponent<TextMeshProUGUI>().text = "=";
                newObject.transform.SetParent(GameObject.Find("GRID").transform);
                newObject.transform.SetAsLastSibling();
                newObject.name = "EQUALS";
            }
            else
            {
                newObject = Instantiate(numberPrefab, new Vector2(0, 0), Quaternion.identity);
                newObject.transform.SetParent(GameObject.Find("GRID").transform);
                newObject.transform.SetAsLastSibling();
                if (c+1 == equation.Length)
                {
                    newObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = equation[c].ToString();
                    newObject.name = equation[c].ToString();
                    break;
                }
                if (equation[c+1] != '-' && equation[c + 1] != '+' && equation[c + 1] != '=' && equation[c + 1] != 'x')
                {
                    newObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = equation[c].ToString() + equation[c + 1].ToString();
                    newObject.name = equation[c].ToString() + equation[c + 1].ToString();
                    c += 1;
                }
                else
                {
                    newObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = equation[c].ToString();
                    newObject.name = equation[c].ToString();
                }
                
            }
            
        }
        levelComplete_ui.SetActive(false);
        EquationManager equationManager = transform.parent.GetComponentInChildren<EquationManager>();
        equationManager.levelComplete_run = false;
        //ERROR: CANT FIND BECAUSE INACTIVE

        UI_Handler handleUI = transform.parent.GetComponentInChildren<UI_Handler>();
        handleUI.setActive();
        hasrun = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelComplete_ui.SetActive(false);
        generateLevel();

        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
