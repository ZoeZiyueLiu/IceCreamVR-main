using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class IceCreamHolderToScore : MonoBehaviour
{
    public ScoreManager scoreManager;
    public int index = -1;

    [SerializeField] private GameObject _iceCream;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        if (index < 0)
            Debug.LogError("Holder Index < 0!");
    }

    private void OnTriggerEnter(Collider other)
    {
        FindIceCream();
        if (_iceCream)
        {
            //player has to release btn then snap on here
            StartCoroutine(HandleDropOnHolder());
        }
    }

    //find ice cream object in a raycast all
    private void FindIceCream()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.up, 10f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //Renderer rend = hit.transform.GetComponent<Renderer>();
            IceCream iceCream = hit.transform.GetComponent<IceCream>();

            if (iceCream)
            {
                _iceCream = hit.transform.gameObject;
                break;
            }
        }
    }

    private IEnumerator HandleDropOnHolder()
    {
        //wait until player released
        yield return new WaitUntil(() => _iceCream.GetComponent<IceCream>().grabber.isGrabbing == false);
        MoveToHolder();
        //play put on to rack sound
        GetComponent<AudioSource>().Play();
        
        //wait
        yield return new WaitForSeconds(1f);
        ScoreIt();
        yield return new WaitForSeconds(0.5f);
        //destroy
        DestroyThisIceCream();
    }

    public void MoveToHolder()
    {
        _iceCream.GetComponent<Rigidbody>().isKinematic = true;
        _iceCream.transform.position = transform.position;
        _iceCream.transform.parent = transform;
        IceCream ic = _iceCream.GetComponent<IceCream>();
        if (ic != null)
        {
            ic.onHolder = true;
            Collider c = _iceCream.GetComponent<Collider>();
            c.enabled = false;
            _iceCream.transform.GetChild(0).GetComponent<Collider>().enabled = false;
            _iceCream.transform.rotation = Quaternion.Euler(0, _iceCream.transform.rotation.y, 0);
        }
    }

    private void ScoreIt()
    {
        IceCream ic = _iceCream.GetComponent<IceCream>();
        if (ic != null)
        {
            scoreManager.AddCheckIceCream(ic.Ingredients, index);
        }
    }

    public void DestroyThisIceCream()
    {
        if (!_iceCream) return;
        IceCream ic = _iceCream.GetComponent<IceCream>();
        if (ic != null)
        {
            foreach (GameObject stroke in ic.strokes)
                Destroy(stroke);
        }
        Destroy(_iceCream);
    }
}
