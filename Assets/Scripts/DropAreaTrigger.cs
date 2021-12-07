using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropAreaTrigger : MonoBehaviour
{
    public float time = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("IceCream"))
        {
            Transform stroke = other.gameObject.transform.parent.transform;
            string flavor;
            IceCreamStroke ics = stroke.GetComponent<IceCreamStroke>();
            if (ics != null)
            {
                flavor = stroke.GetComponent<IceCreamStroke>().flavor;
                StartCoroutine(StopAfterTime(other.gameObject));
                //add ice cream flavor
                IceCream ic = transform.parent.GetComponent<IceCream>();
                if (ic != null && !ic.Ingredients.Contains(flavor))
                    ic.Ingredients.Add(flavor);

                ics.cone = transform.parent.gameObject;

                Debug.Log(stroke.name);
                //stroke.transform.parent = transform.parent;

                if(!ic.strokes.Contains(stroke.gameObject))
                    ic.AddStroke(stroke.gameObject);
            }
        }
    }

    private IEnumerator StopAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.parent = transform.parent;
    }
}
