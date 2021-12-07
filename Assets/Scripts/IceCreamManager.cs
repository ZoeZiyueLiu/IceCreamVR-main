using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamManager : MonoBehaviour
{
    //Public
    public GameObject spawnObj;
    public float drippingSpeed = 1f;
    public string flavor;
    public bool isPressing = false;
    [SerializeField] IceCreamButton otherBtn1;
    [SerializeField] IceCreamButton otherBtn2;

    //Private variables
    private Transform spawnPoint;
    List<GameObject> strokes = new List<GameObject>();

    private GameObject m_currParent;
    private GameObject m_currChild;
    private float fullLength;

    public void Start()
    {
        spawnPoint = transform;
        if (flavor == "")
            Debug.LogError("Error: flavor is not set!");
    }
    private void Update()
    {
        //if (isPressing)
        //{
        //    Debug.Log("is pressing, diactiviate");
        //    otherBtn1.activate = false;
        //    otherBtn2.activate = false;
        //}
        //else
        //{
        //    otherBtn1.activate = true;
        //    otherBtn2.activate = true;
        //}

        //Editor tests
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down!");
            ButtonOnEnter();
        }
        else if(Input.GetMouseButton(0))
        {
            Debug.Log("Mouse hold!");
            ButtonOnStay();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse up!");
            ButtonOnExit();
        }
    }

    public void ButtonOnEnter()
    {
        isPressing = true;
        GameObject group = new GameObject("Ice Cream Stroke");
        strokes.Add(group);
        //add the first ice cream
        GameObject child = SpawnStrokeAt(spawnPoint);

        child.GetComponent<IceCreamStroke>().StartFreezeCoroutine();
        fullLength = child.transform.GetChild(0).position.y - transform.position.y;
        Debug.Log(fullLength);

        //TO-DO
        Material mat = m_currChild.transform.GetChild(19).GetComponent<Renderer>().material;
        //float grow = 1f - (m_currChild.transform.GetChild(0).transform.position.y - transform.position.y) / fullLength;
        mat.SetFloat("_Grow", 0);

        StartCoroutine(AddIceCream());
    }

    public void ButtonOnStay()
    {
        Move();
    }

    public void ButtonOnExit()
    {
        Release();
        isPressing = false;
    }

    private IEnumerator AddIceCream()
    {
        while (isPressing)
        {
            GameObject stroke = strokes[strokes.Count - 1];
            Transform lastChild = stroke.transform.GetChild(stroke.transform.childCount - 1);

            //joint0 position
            if (lastChild.GetChild(0).transform.position.y <= spawnPoint.position.y)
            {
                //if last ice cream stroke ends, add another one
                GameObject newChild = SpawnStrokeAt(lastChild.GetChild(0).transform);

                //connect two strokes
                lastChild.GetChild(1).GetComponent<CharacterJoint>().connectedBody = newChild.transform.GetChild(18).GetComponent<Rigidbody>();
                lastChild.GetChild(0).position = newChild.transform.GetChild(18).position;
                FixedJoint fj = lastChild.GetChild(0).gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = newChild.transform.GetChild(18).GetComponent<Rigidbody>();
                lastChild.GetChild(0).parent = newChild.transform.GetChild(18);
            }
            lastChild.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForFixedUpdate();
        }
    }

    private GameObject SpawnStrokeAt(Transform spawnPosition)
    {
        m_currParent = strokes[strokes.Count - 1];
        GameObject newChild = Instantiate(spawnObj, spawnPosition.position, Quaternion.identity, m_currParent.transform);
        IceCreamStroke ics = newChild.AddComponent<IceCreamStroke>();
        ics.flavor = flavor;
        m_currChild = newChild;
        newChild.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;

        return newChild;
    }

    // move ice cream down when player still pressing
    private void Move()
    {
        Material mat = m_currChild.transform.Find("pCylinder2").GetComponent<Renderer>().material;
        float grow = 1f - (m_currChild.transform.GetChild(0).transform.position.y - transform.position.y) / fullLength;
        mat.SetFloat("_Grow", grow);
    }

    // Unfreeze all ice cream part
    private void Release()
    {
        m_currParent = strokes[strokes.Count - 1];
        for (int i = 0; i < m_currParent.transform.childCount-1; i++)
        {
            GameObject child = m_currParent.transform.GetChild(i).gameObject;
            if(child.transform.GetChild(0).GetComponent<Rigidbody>() != null)
                child.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        }
        //Last child
        Transform lastchild = m_currParent.transform.GetChild(m_currParent.transform.childCount - 1);
        lastchild.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;

        //Update material to cull
        Material mat = lastchild.Find("pCylinder2").GetComponent<Renderer>().material;
        float grow = 1f - (lastchild.transform.GetChild(0).transform.position.y - transform.position.y) / fullLength;
        mat.SetFloat("_Grow", grow);

        //disable collision
        int disableNum = (int) Mathf.Floor((1f - grow) * 19);
        //Debug.Log(disableNum);
        if (disableNum < 2) return;
        //lastchild.GetChild(disableNum).GetComponent<CharacterJoint>().connectedBody = null;
        for (int i = 0; i < disableNum-2; i++)
        {
            Destroy(lastchild.GetChild(i).GetComponent<CharacterJoint>());
            Destroy(lastchild.GetChild(i).GetComponent<Rigidbody>());
            Destroy(lastchild.GetChild(i).GetComponent<Collider>());
        }
        if(lastchild.GetComponent<IceCreamStroke>().cone != null)
            lastchild.parent = lastchild.GetComponent<IceCreamStroke>().cone.transform;
    }

    //destroy the stroke with cone connected
    public void DestroyStroke(GameObject cone)
    {

    }

    //private IEnumerator CreateOverTime()
    //{
    //    while(isPressing)
    //    {
    //        CreateOne();
    //        yield return new WaitForSeconds(spawnInterval);
    //    }
    //}

    //private void CreateOne()
    //{
    //    m_currChildIndex++;
    //    m_currParent = strokes[strokes.Count - 1];
    //    // instantiate child under the current ice cream stroke
    //    GameObject child = Instantiate(spawnObj, spawnPoint.position, Quaternion.identity, m_currParent.transform);

    //    child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    //    //child.AddComponent<IceCreamPart>();
    //    // remove joint if its the first
    //    if (m_currParent.transform.childCount == 1)
    //    {
    //        Destroy(child.GetComponent<HingeJoint>());
    //    }
    //    else
    //    {
    //        // get prev child and connect to character joint
    //        GameObject prevChild = m_currParent.transform.GetChild(m_currChildIndex-1).gameObject;
    //        HingeJoint cj = child.GetComponent<HingeJoint>();
    //        cj.connectedBody = prevChild.GetComponent<Rigidbody>();

    //        prevChild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;       //unfreeze previous one
    //    }
    //}

    // push ice cream downwards
}
