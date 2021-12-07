using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Penguin : MonoBehaviour
{
    public List<Transform> waypoints;
    public List<Transform> holders;
    public Transform handPosition;
    public float threshold = 0.1f;
    public float waitDistance = 3f;
    public float speed = 3f;
    public bool getIceCream = false;
    [SerializeField] GameObject heightConstraintTarget;
    //[SerializeField] GameObject ikTarget;
    [SerializeField] GameObject headAimTarget;
    [SerializeField] GameObject headPositionTarget;
    [SerializeField] MultiPositionConstraint positionConstraint;
    [SerializeField] MultiAimConstraint headAimConstraint;

    private Animator _animator;
    private bool isLeaving = false;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        heightConstraintTarget.transform.position += new Vector3(0, Random.Range(0f, 3f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLeaving)
            headAimTarget.transform.position = holders[1].position;
        //wait for my ice cream
        if (!getIceCream && Vector3.Distance(transform.position, waypoints[1].position) <= threshold)
        {
            //stop and wait
            _animator.SetBool("isWalking", false);

        }
        else if (ShouldIWait())
        {
            //Wait
            _animator.SetBool("isWalking", false);
        }
        else if(Vector3.Distance(transform.position, waypoints[2].position) <= threshold)
        {
            //Penguin disappear
            DestroyMe();
        }
        else
        {
            //Moveforward;
            float ranSpped = speed + Random.Range(-1f, 1f);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward * ranSpped * Time.deltaTime, 1f);
            _animator.SetBool("isWalking", true);
        }
    }

    private bool ShouldIWait()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, waitDistance))
        {
            if (hit.transform.gameObject.CompareTag("Penguin"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                return true;
            }
        }
        return false;
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public IEnumerator PlayIceCreamAnimation(int holderIndex)
    {
        //_animator.SetBool("pickup", true);
        //TO-DO
        Transform holder = holders[holderIndex];
        Transform myIceCream = holder.GetChild(0);
        if(myIceCream == null)
        {
            Debug.LogError("ERROR: no ice cream in holder " + holderIndex);
        }
        headPositionTarget.transform.position = holder.position;
        //myIceCream.position = handPosition.position;
        positionConstraint.weight = 1;
        yield return new WaitForSeconds(0.5f);
        myIceCream.parent = transform;
        positionConstraint.weight = 0;

        headAimConstraint.weight = 0;
    }

    public void SadPenguin()
    {
        headAimTarget.transform.position = transform.position + Vector3.forward * 12f;
        isLeaving = true;
    }
}
