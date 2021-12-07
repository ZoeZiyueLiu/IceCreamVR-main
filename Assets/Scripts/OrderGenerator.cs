using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderGenerator : MonoBehaviour
{
    //lists of flavors and toppings
    private List<string> flavors = new List<string> { "Vanilla", "Strawberry", "Chocolate" };
    //private List<string> flavors = new List<string> { "Vanilla"};            //  TMP
    //private List<string> toppings = new List<string> { "Sprinkles", "Oreos", "Cookie Dough", "Chocolate Chips", "M&M's",
    //    "Peanuts", "Roasted Almonds", "Pecans", "Bluberries", "Strawberries", "Gummy Bears"};
    private List<string> toppings = new List<string> { "Sprinkles", "Cookie Dough", "Chocolate Chips", "M&M's", "Oreos", "Bluberries", "Strawberries", "Gummy Bears"};

    public List<Order> orders = new List<Order>();

    public GameObject panel;
    //public TMPro.TMP_Text title;
    //public TMPro.TMP_Text currentOrders;

    public AudioClip lostOrder;
    public AudioClip NewOrder;

    private string outputString;
    public bool takingOrders=false;
    private int nop=4;  //number of panels

    private List<TMPro.TMP_Text> fs = new List<TMPro.TMP_Text>();
    private List<TMPro.TMP_Text> ts = new List<TMPro.TMP_Text>();
    private List<TMPro.TMP_Text> times = new List<TMPro.TMP_Text>();

    private float timeremove = 30;  //time to finish the order
    private float timeadd = 15;     //time to add a new order
    private float lastorder = -14;

    public int count = 2; //number of orders generated
    //private int num = 2; //number used for generating simpler orders at the beginning

    System.Random random1 = new System.Random();
    System.Random random2 = new System.Random();
    System.Random random3 = new System.Random();

    private AudioSource _as;
    [SerializeField] private PenguinManager _pm;
    [SerializeField] private TimerManager _tm;

    // Start is called before the first frame update
    void Start()
    {
        //takingOrders = true;

        for(int i = 0; i < nop; i++)
        {
            //access texts for flavor,toppings and time
            Transform thistxtf = panel.transform.Find("Panel"+i.ToString()+"/Flavor");
            Transform thistxtt = panel.transform.Find("Panel" + i.ToString() + "/Toppings");
            Transform thistxttime = panel.transform.Find("Panel" + i.ToString() + "/Time");
            fs.Add(thistxtf.GetComponent<TMPro.TMP_Text>());
            ts.Add(thistxtt.GetComponent<TMPro.TMP_Text>());
            times.Add(thistxttime.GetComponent<TMPro.TMP_Text>());
        }
        //generate a new order after certain time
        //StartCoroutine(GenerateNewOrder(10f));

        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // add some noise to the time add
        float currTimeadd = timeadd * ((_tm.timeRemaining + 2f * _tm.timelimit) / 3f) / _tm.timelimit + Random.Range(0f, 2f);

        if (takingOrders == true && orders.Count<=5)
        {
            panel.SetActive(true);
            
        }
        if (takingOrders)
        {
            //check if number of orders < 4
            if (orders.Count < nop)
            {
                

                if ((Time.time - lastorder) > currTimeadd)
                {
                    _pm.SpawnNewPenguin();
                    StartCoroutine(GenerateNewOrder(5f));
                    _as.clip = NewOrder;
                    _as.Play();
                }
            }

            if (orders.Count > 0)
            {
                if ((Time.time - orders[0].starttime) > orders[0].totaltime)
                {
                    _pm.PopFrontPenguin();
                    orders.RemoveAt(0);
                    _as.clip = lostOrder;
                    _as.Play();
                }
            }

            for (int i = 0; i < nop; i++)
            {
                if (i < orders.Count)
                {
                    fs[i].text = orders[i].SelectedFlavor;

                    outputString = "";      // reset str

                    for (int j = 0; j < orders[i].SelectedToppings.Count; j++)
                    {
                        outputString += orders[i].SelectedToppings[j];

                        outputString += "\n";

                    }
                    ts[i].text = outputString;
                    times[i].text = "Time: " + ((int)((orders[i].starttime + orders[i].totaltime - Time.time) % 60)).ToString();
                }
                else
                {
                    fs[i].text = "";
                    ts[i].text = "";
                    times[i].text = "";
                }
            }
        }


    }

    private IEnumerator GenerateNewOrder(float waitTime)
    {

        Order newOrder = new Order();
        newOrder.SelectedFlavor = null;
        newOrder.SelectedToppings = new List<string>();

        //randomly generate flavor
        int f = random1.Next(flavors.Count);
        newOrder.SelectedFlavor = flavors[f];

        //randomely generate number of toppings, then generate topping and add to the topping list
        int toppingNum = random1.Next(3)+1;
        for(int i = 0; i < toppingNum; i++)
        {
            int t = random3.Next(toppings.Count);

            if (newOrder.SelectedToppings.Contains(toppings[t])) { continue; }
            else
            {
                newOrder.SelectedToppings.Add(toppings[t]);
            }
            if (count > 1)
            {
                if (newOrder.SelectedToppings.Count == 1)
                {
                    count -= 1;
                    break;
                }
            }
            else if (count>=0)
            {
                if (newOrder.SelectedToppings.Count == 2)
                {
                    count -= 1;
                    break;
                }
            }
        }
        newOrder.starttime = Time.time;
        newOrder.totaltime = timeremove * ((_tm.timeRemaining + 2f * _tm.timelimit) / 3f) / _tm.timelimit + Random.Range(0f, 2f);
        
        //add the new order to the order list
        orders.Add(newOrder);
        lastorder = Time.time;

        yield return new WaitForSecondsRealtime(waitTime);
        
    }

    public struct Order
    {
        public string SelectedFlavor;
        public List<string> SelectedToppings;
        public float starttime;
        public float totaltime;


        public Order(string selectedFlavor, List<string> selectedToppings,float st, float tt)
        {
            this.SelectedFlavor = selectedFlavor;
            this.SelectedToppings = selectedToppings;
            this.starttime = st;
            this.totaltime = tt;
        }

    }
}
