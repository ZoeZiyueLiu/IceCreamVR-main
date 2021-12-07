using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //public GameObject holder;

    public GameObject orderManager;
    public GameObject scorePanel;

    public List<OrderGenerator.Order> currOrders;
    public List<GameObject> iceCreamHolder;

    public AudioClip GoodOrder;
    public AudioClip BadOrder;

    private bool putOnHolder;

    public int currScore = 0;
    public int totalScore = 0;

    private string SelectedFlavor;
    private List<string> SelectedToppings;

    private List<string>[] iceCreamRack = new List<string>[3];

    private TMPro.TMP_Text cscore;
    private TMPro.TMP_Text tscore;

    private float timeGap = 10;     //time to add a new order
    private float lastCheck = 0;

    private AudioSource _as;
    [SerializeField] private PenguinManager _pm;

    // Start is called before the first frame update
    void Start()
    {
        Transform totalScore = scorePanel.transform.Find("TotalScore");
        Transform currScore = scorePanel.transform.Find("CurrentOrder");
        tscore=totalScore.GetComponent<TMPro.TMP_Text>();
        cscore=currScore.GetComponent<TMPro.TMP_Text>();

        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OrderGenerator orderScript = orderManager.GetComponent<OrderGenerator>();
        currOrders = orderScript.orders;
    }

    public void AddCheckIceCream(List<string> iceCream, int index)
    {
        if (index >= iceCreamRack.Length & index < 0)
        {
            Debug.LogError("ERROR: holder index out of range!");
            return;
        }
        iceCreamRack[index] = iceCream;
        NotifyCheck(index);
    }

    //called when a ice cream is put on holder
    public void NotifyCheck(int index)
    {
        if (Time.time - timeGap > lastCheck)
        {
            if (currOrders.Count > 0)
            {
                CheckIceCream(index);
                totalScore += currScore;
                //tscore.text = "Total Score:" + "\n" + totalScore;
                tscore.text = "Total Score:" + totalScore;
                if (currScore > 0)
                {
                    cscore.text = "+" + currScore.ToString();
                }
                else
                {
                    cscore.text = currScore.ToString();
                }
                currOrders.RemoveAt(0);
                lastCheck = Time.time;
            }
        }

    }

    void CheckIceCream(int index)
    {
        Debug.Log("Checking ice cream " + index);
        currScore = 0;
        OrderGenerator.Order firstOrder = currOrders[0];
        //check flavor
        if (iceCreamRack[index].Count>0 && iceCreamRack[index][0] == firstOrder.SelectedFlavor)
        {
            currScore += 30;
        }
        else
        {
            currScore -= 10;
        }

        List<string> currToppings = firstOrder.SelectedToppings;
        //check toppings
        for (int i = 1; i < iceCreamRack[index].Count-1; i++)
        {
            for(int j = 0; j < currToppings.Count; j++) {
                if (iceCreamRack[index][i] == currToppings[j])
                {
                    currScore += 10;
                    break;
                }
                else
                {
                    currScore -= 5;
                }
            }
        }
        
        if(currScore>0)
        {
            _as.clip = GoodOrder;
            _as.Play();
        }
        else
        {
            _as.clip = BadOrder;
            _as.Play();
        }

        _pm.GetYourIceCream(index);
    }

    //called when restart the game
    public void ClearScore()
    {
        currScore = 0;
        totalScore = 0;
        cscore.text = "+" + currScore.ToString();
        tscore.text = "Total Score:" + totalScore;
    }

}
