using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShadowMatch : MonoBehaviour
{

    public TMP_Text text1;
    public TMP_Text text2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            text2.text = "101";
        }*/

        text1.text = text2.text;

    }
}
