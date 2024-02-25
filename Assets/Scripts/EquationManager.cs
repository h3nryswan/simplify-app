using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EquationManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public bool levelComplete_run = false;
    void checkOperatorLogic()
    {
        Transform operator1 = null;
        Transform operator2 = null;
        bool prev = false;
        bool current = false;
        bool checkFailed = false;
        foreach (Transform child in transform)
        {
            // If not an operator
            if (child.childCount > 0 || !child.TryGetComponent(out TextMeshProUGUI t))
            {
                prev = false;
                continue;
            }
            if (t.text == "=")
            {
                prev = false;
                continue;
            }
            if (t.text == "-" || t.text == "+")
            {
                operator2 = child;
                current = true;
            }
            if (current && prev)
            {
                checkFailed = true;
                break;
            }
            prev = true;
            operator1 = child;
            current = false;

        }

        if (checkFailed)
        {
            if (operator1.GetComponent<TextMeshProUGUI>().text == "-" && operator2.GetComponent<TextMeshProUGUI>().text == "-")
            {
                operator2.SetParent(null);
                Destroy(operator2.gameObject);
            }
            else if (operator1.GetComponent<TextMeshProUGUI>().text == "-" && operator2.GetComponent<TextMeshProUGUI>().text == "+")
            {
                operator2.SetParent(null);
                Destroy(operator2.gameObject);
            }
            else if (operator1.GetComponent<TextMeshProUGUI>().text == "+" && operator2.GetComponent<TextMeshProUGUI>().text == "-")
            {
                operator1.SetParent(null);
                Destroy(operator1.gameObject);
            }
            else if (operator1.GetComponent<TextMeshProUGUI>().text == "+" && operator2.GetComponent<TextMeshProUGUI>().text == "+")
            {
                operator1.SetParent(null);
                Destroy(operator1.gameObject);
            }
            else
            {
                Debug.Log("Checkfailed !!!!failed!!!!");
            }
        }
    }

    bool levelComplete()
    {
        if (transform.childCount == 3)
        {
            bool number1 = false;
            bool equals = false;
            bool number2 = false;
            bool x1 = false;
            bool x2 = false;
            TextMeshProUGUI t1 = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI eq = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI t2 = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            if (eq != null)
            {
                if (eq.text == "=")
                {
                    equals = true;
                }
            }
            if (t1 != null && t2 != null)
            {
                number1 = int.TryParse(t1.text, out _);
                number2 = int.TryParse(t2.text, out _);
                if (t1.text == "x")
                {
                    x1 = true;
                }
                if (t2.text == "x")
                {
                    x2 = true;
                }
            }
            if (x1 && equals && number2)
            {
                return true;
            }
            else if (number1 && equals && x2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (transform.childCount == 4)
        {
            // Case: first term is a minus, second term is num, third term is =, 4th term is x
            bool operator1 = transform.GetChild(0).TryGetComponent(out TextMeshProUGUI t);
            if (operator1)
            {
                if (t.text == "-")
                {
                    bool equals = false;
                    bool number1 = false;
                    bool x = false;
                    TextMeshProUGUI t1 = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
                    TextMeshProUGUI eq = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI x1 = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
                    if (eq != null)
                    {
                        if (eq.text == "=")
                        {
                            equals = true;
                        }
                    }
                    if (t1 != null)
                    {
                        number1 = int.TryParse(t1.text, out _);
                    }
                    if (x1 != null)
                    {
                        if (x1.text == "x")
                        {
                            x = true;
                        }
                    }
                    if (equals && x && number1)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                // Case: first term is a x, second term is equal third term is -, 4th term is num
                bool operator2 = transform.GetChild(2).TryGetComponent(out TextMeshProUGUI text);
                if(operator2)
                {
                    if (text.text == "-")
                    {
                        TextMeshProUGUI t1 = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
                        TextMeshProUGUI eq = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI x1 = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
                        bool equals = false;
                        bool number1 = false;
                        bool x = false;
                        if (eq != null)
                        {
                            if (eq.text == "=")
                            {
                                equals = true;
                            }
                        }
                        if (x1 != null)
                        {
                            if (x1.text == "x")
                            {
                                x = true;
                            }
                        }
                        if (t1 != null)
                        {
                            number1 = int.TryParse(t1.text, out _);
                        }
                        if (equals && x && number1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return false;

            }

        }
        else
        {
            return false;
        }
        
    }

    void waitForNextLevel()
    {
        Debug.Log("Level Complete");
        // Remove Equation
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // Display success UI

        // Update level counter
        LevelNum levelNum = transform.parent.GetComponentInChildren<LevelNum>();
        levelNum.lvlCounter += 1;

        // Wait for next -> button from user
        CreateLevel createLevel = transform.parent.GetComponentInChildren<CreateLevel>();
        createLevel.levelComplete_ui.SetActive(true);
        // Generate next equation
    }

    // Update is called once per frame
    void Update()
    {
        checkOperatorLogic();
        if (levelComplete())
        {
            if (!levelComplete_run)
            {
                UI_Handler handleUI = transform.parent.GetComponentInChildren<UI_Handler>();
                handleUI.Invoke("setInactive", (float)0.5);
                Invoke("waitForNextLevel", (float)0.5);
                levelComplete_run = true;
            }
            
        }
    }
}
