using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class IceCreamButton : MonoBehaviour
{
    public IceCreamManager icm;
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private float deadZone = 0.1f;
    public bool activate = true;
    public AudioClip _machineSFXFront;
    public AudioClip _machineSFXMiddle;
    public AudioClip _machineSFXEnd;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    private Collider _collider;
    private Rigidbody _rb;
    //public UnityEvent onPressed, onStay, onReleased;

    private AudioSource _as;
    private bool isPlaying = false;

    private void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Debug.Log(GetValue());
        if (activate)
        {
            _collider.enabled = true;
            _rb.detectCollisions = true;
            if (!_isPressed && GetValue() + threshold >= 1)
                Pressed();
            if (_isPressed && GetValue() + threshold >= 1)
                Stay();
            if (_isPressed && GetValue() - threshold <= 0)
                Released();
        }
        else
        {
            _collider.enabled = false;
            _rb.detectCollisions = false;
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _as.clip = _machineSFXFront;
        _as.Play();
        _isPressed = true;
        icm.ButtonOnEnter();
    }
    private void Stay()
    {
        _as.clip = _machineSFXMiddle;
        _as.loop = true;
        if(!isPlaying)
        {
            _as.Play();
            isPlaying = true;
        }
        icm.ButtonOnStay();
    }

    private void Released()
    {
        _as.clip = _machineSFXEnd;
        _as.loop = false;
        _as.Play();
        isPlaying = false;
        icm.ButtonOnExit();
        _isPressed = false;
    }
}
