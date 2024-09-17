using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{


    public Text timerText;
    private float startTime;  
    private bool finnished = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (finnished)
        {
            return;
        }
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string Seconds = (t % 60).ToString("f1");
        timerText.text = minutes +" : "+ Seconds;

    }


    public void Finnish()
    {
        finnished = true;
        timerText.color = Color.blue;
    }
}
