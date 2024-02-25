using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public bool active = false;
    public void setInactive()
    {
        transform.localScale = new Vector3(0,0,0); 
    }

    public void setActive()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
