using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationsSolved : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public uint eqnCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eqnCounter == 1)
        {
            counterText.text = string.Format("{0} equation solved!", eqnCounter.ToString());
        }
        else
        {
            counterText.text = string.Format("{0} equations solved!", eqnCounter.ToString());
        }
        
    }
}
