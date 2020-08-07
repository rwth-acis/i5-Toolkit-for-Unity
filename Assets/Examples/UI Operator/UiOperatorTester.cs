using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiOperatorTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            InputField input = GameObject.Find("InputField").GetComponent<InputField>();
            input.text = "hello";
        }
    }
}
