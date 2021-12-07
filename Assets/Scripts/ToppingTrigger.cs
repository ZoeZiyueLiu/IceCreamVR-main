using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ToppingTrigger : MonoBehaviour
{
    public int toppingIndex;
    public GameObject toppingPrefab;
    public int spawnNum = 10;
    //public bool randomColor = false;
    public List<Material> randomMaterials = new List<Material>(); 

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        ToppingsManager tm = other.gameObject.GetComponent<ToppingsManager>();
        if(tm!=null)
            tm.SpawnToppings(toppingPrefab, spawnNum, randomMaterials);
    }
}
