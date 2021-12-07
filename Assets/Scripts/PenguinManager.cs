using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinManager : MonoBehaviour
{
    public GameObject penguin;
    public List<Transform> waypoints;
    public List<Transform> holders;

    private Queue<GameObject> _penguins;

    private void Start()
    {
        _penguins = new Queue<GameObject>();
    }

    public void SpawnNewPenguin()
    {
        GameObject newPenguin = Instantiate(penguin, waypoints[0].position, Quaternion.identity);
        newPenguin.GetComponent<Penguin>().waypoints = waypoints;
        newPenguin.GetComponent<Penguin>().holders = holders;
        _penguins.Enqueue(newPenguin);
    }

    public void GetYourIceCream(int holderIndex)
    {
        //play get ice cream animation
        GameObject p = _penguins.Peek();
        StartCoroutine(p.GetComponent<Penguin>().PlayIceCreamAnimation(holderIndex));

        //when donw playing, leave
        p = _penguins.Dequeue();
        p.GetComponent<Penguin>().getIceCream = true;
    }

    public void PopFrontPenguin()
    {
        GameObject p = _penguins.Dequeue();
        p.GetComponent<Penguin>().getIceCream = true;
        p.GetComponent<Penguin>().SadPenguin();
    }

    public void ClearPenguin()
    {
        while(_penguins.Count > 0)
        {
            PopFrontPenguin();
        }
    }
}
