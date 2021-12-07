using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topping : MonoBehaviour
{
    public string toppingName;
    private bool isOnIceCream = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMe());
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObj = collision.gameObject;
        if (otherObj.CompareTag("IceCream"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = otherObj.transform.parent;
            IceCream ic = otherObj.transform.parent.GetComponent<IceCream>();
            if (ic != null && !ic.Ingredients.Contains(toppingName))
            {
                ic.Ingredients.Add(toppingName);
            }
            isOnIceCream = true;
        }
    }

    private IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(15f);
        if(!isOnIceCream)
            Destroy(gameObject);
    }
}
