using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class InstructionManager : MonoBehaviour
{
    public Transform anchor;        // get the transform of the OculusGo Controller device
    public GameObject instructPanel;
    public GameObject Timer;
    public GameObject Order;
    public GameObject Score;

    public AudioClip UIButton;
    public AudioClip Window;

    public static UnityAction onTriggerDown = null;

    private GameObject btn;
    public GameObject btn1;
    public GameObject btn2;
    private TMPro.TMP_Text btn1txt;
    private TMPro.TMP_Text btn2txt;
    private float MAX_DISTANCE = 1000;

    private TMPro.TMP_Text instruct;

    private List<string> instructions;
    private int instructind = 0;

    public Animator windowAnim;
    public Animator screenAnim;

    private TimerManager timerscript;
    private OrderGenerator orderscript;
    private ScoreManager scorescript;

    private AudioSource _as;

    [SerializeField] private PenguinManager _pm;
    private IceCreamButton[] iceCreamBtns;
    // Start is called before the first frame update
    void Start()
    {
        instructPanel.SetActive(true);


        instructions = new List<string> { "Welcome!", "You're going to make ice cream in this game. Woo-Hoo!",
            "First, take a look at the ice cream truck and get familiar with your worksatation!",
            "You will see a screen with a list of orders later.",
            "Try making ice cream with desired flavor and toppings in time!",
            "Put the ice cream on the holder behind the window to submit and earn scores.",
            "Press the start button to start taking orders. You have two minutes!"};

        Transform instructText = instructPanel.transform.Find("Instructions");
        instruct = instructText.GetComponent<TMPro.TMP_Text>();

        timerscript = Timer.GetComponent<TimerManager>();
        orderscript = Order.GetComponent<OrderGenerator>();
        scorescript = Score.GetComponent<ScoreManager>();

        //two buttons changed to quit and restart
        Transform b1 = btn1.transform.Find("TextNext");
        btn1txt = b1.GetComponent<TMPro.TMP_Text>();
        Transform b2 = btn2.transform.Find("TextPrev");
        btn2txt = b2.GetComponent<TMPro.TMP_Text>();
        //instruct.text += "Final Score:" + "\n" + scorescript.totalScore;

        iceCreamBtns = FindObjectsOfType<IceCreamButton>();

        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //instruct.text = instructions[instructind];
        Ray ray = new Ray(anchor.position, anchor.forward); // cast a ray from the controller out towards where it is pointing
        RaycastHit hit;                                     // returns the hit variable to indicate what and where the ray 
                                                            // was intersected if at all

        // check for user input: primary trigger
        // textPanel.SetActive(true);

        if (timerscript.firstGame == true)
        {
            instruct.text = instructions[instructind];
            if (instructind == 0)
            {
                instruct.fontSize = 0.2f;
                btn2txt.text = "Quit";
            }
            else
            {
                instruct.fontSize = 0.1f;
                btn2txt.text = "Prev";
            }

            //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.Touch))
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch))
            {
                if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
                {
                    _as.clip = UIButton;
                    _as.Play();
                    if (hit.transform.gameObject.CompareTag("ButtonNext"))
                    {
                        //_as.clip = UIButton;
                        //_as.Play();
                        btn = hit.transform.gameObject;
                        TriggerDownNext();
                    }
                    else if (hit.transform.gameObject.CompareTag("ButtonPrev"))
                    {
                        btn = hit.transform.gameObject;
                        TriggerDownPrev();
                    }
                }
            }
        }
        else if (timerscript.timerIsRunning == false)
        {
            //show final score when time's up
            instruct.text = "Final Score:" + "\n" + scorescript.totalScore;
            btn1txt.fontSize = 0.07f;
            btn1txt.text = "Restart";
            btn2txt.text = "Quit";

            //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.Touch))
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch))
            {
                if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
                {
                    _as.clip = UIButton;
                    _as.Play();
                    if (hit.transform.gameObject.CompareTag("ButtonNext"))
                    {
                        btn = hit.transform.gameObject;
                        //restart game
                        TriggerDownNext();
                    }
                    else if (hit.transform.gameObject.CompareTag("ButtonPrev"))
                    {
                        btn = hit.transform.gameObject;
                        //quit game
                        Application.Quit();
                    }
                }
            }
        }
    }

   
    private void TriggerDownNext()
    {
        if (timerscript.firstGame == true) {
            instructind = Mathf.Min(instructions.Count-1,instructind+1);
            if (instructind == instructions.Count - 1)
            {
                Transform nexttxt = btn.transform.Find("TextNext");
                TMPro.TMP_Text btnnexttxt = nexttxt.GetComponent<TMPro.TMP_Text>();
                if (btnnexttxt.text == "Start")
                {
                    StartCoroutine(DoStartAnim(0.5f));
                    timerscript.timerIsRunning = true;
                    timerscript.cd = true;
                    timerscript.win = true;
                    orderscript.takingOrders = true;
                    timerscript.firstGame = false;
                    for (int i = 0; i < iceCreamBtns.Length; i++)
                        iceCreamBtns[i].activate = true;
                    //btn1txt.text = "Restart";
                    //btn2txt.text = "Quit";

                }
                btnnexttxt.text = "Start";
            }   
        }
        else
        {
            //Transform nexttxt = btn.transform.Find("TextNext");
            //Transform b1 = btn1.transform.Find("TextNext");
            //TMPro.TMP_Text btnnexttxt = b1.GetComponent<TMPro.TMP_Text>();
            if (btn1txt.text == "Restart")
            {
                StartCoroutine(DoStartAnim(0.5f));
                scorescript.ClearScore();
                timerscript.timerIsRunning = true;
                timerscript.cd = true;
                timerscript.win = true;
                orderscript.takingOrders = true;
                orderscript.count = 2;
                for (int i = 0; i < iceCreamBtns.Length; i++)
                    iceCreamBtns[i].activate = true;
                //timerscript.firstGame = false;
            }
        }
    }

    public IEnumerator DoStartAnim(float t)
    {
        _as.clip = Window;
        _as.Play();
        yield return new WaitForSeconds(t);

        screenAnim.SetBool("up", false);
        windowAnim.SetBool("up", true);
        //yield return new WaitForSeconds(t);
        //timerscript.timerIsRunning = true;
        //orderscript.takingOrders = true;
        //timerscript.firstGame = false;
    }

    public IEnumerator DoEndAnim(float t)
    {
        _as.clip = Window;
        _as.Play();
        yield return new WaitForSeconds(t);

        screenAnim.SetBool("up", true);
        windowAnim.SetBool("up", false);
        orderscript.takingOrders = false;
        orderscript.orders.Clear();
        _pm.ClearPenguin();

        if (timerscript.win == true)
        {
            _as.clip = timerscript.Win;
            _as.Play();
            timerscript.win = false;
        }

        for (int i = 0; i < iceCreamBtns.Length; i++)
            iceCreamBtns[i].activate = false;
        //yield return new WaitForSeconds(t);
    }

    private void TriggerDownPrev()
    {
        if (instructind == 0 && btn2txt.text == "Quit")
        {
            Application.Quit();
        }
        instructind = Mathf.Max(0, instructind - 1);
    }
}
