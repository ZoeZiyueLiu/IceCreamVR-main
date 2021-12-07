using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrashBin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if(parent != null && parent.CompareTag("Cone"))
            StartCoroutine(TrashIceCream(parent.gameObject));
        else if(other.gameObject.CompareTag("Cone"))
            StartCoroutine(TrashIceCream(other.gameObject));
    }

    private IEnumerator TrashIceCream(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }
}
