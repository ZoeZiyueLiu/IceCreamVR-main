using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timelimit = 30;
    //set game time in inspector
    public float timeRemaining;
    public bool timerIsRunning = false;
    public bool firstGame = true;

    public AudioClip Countdown;
    public AudioClip Win;

    public GameObject timePanel;

    private TMPro.TMP_Text timeLeft;

    public GameObject Instruct;
    private InstructionManager instructscript;

    private AudioSource _as;

    //flags for detection audio played or not
    public bool cd = true;
    public bool win = true;

    // Start is called before the first frame update
    void Start()
    {
        //timer set to false before player press start
        //timerIsRunning = true;
        timeRemaining = timelimit;
        Transform timeText = timePanel.transform.Find("TimeLeft");
        timeLeft = timeText.GetComponent<TMPro.TMP_Text>();

        instructscript = Instruct.GetComponent<InstructionManager>();

        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <=4 && cd==true)
                {
                    _as.clip = Countdown;
                    _as.Play();
                    cd = false;
                }
            }
            else 
            {
                timeRemaining = 0;
                timerIsRunning = false;
                StartCoroutine(instructscript.DoEndAnim(0.1f));

            }
        }
        else
        {
            timeRemaining = timelimit;
        }

        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeLeft.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }    
}
