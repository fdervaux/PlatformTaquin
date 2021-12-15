using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform _start;
    public Transform _end;
    public float _speed;

    private Vector3 _currentTarget;
    private bool isInverseWay = false;
    private Rigidbody _rigidbody;
    private Vector3 _velocity = Vector3.zero;

    public Vector3 velocity()
    {
        return _velocity;
    }


    // Start is called before the first frame update
    void Start()
    {
        _currentTarget =  _end.position;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = _start.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 oldPosition = _rigidbody.position;

        //move platform to target
        _rigidbody.position = Vector3.MoveTowards(transform.position, _currentTarget, _speed*Time.fixedDeltaTime);
        _velocity = (_rigidbody.position - oldPosition) / Time.fixedDeltaTime;


        //inverse target if reach position
        if(transform.position == _currentTarget)
        {
            if(isInverseWay)
            {
                _currentTarget = _end.position;
                isInverseWay = false;
            }
            else
            {
                _currentTarget = _start.position;
                isInverseWay = true;
            }
        }
    }
}
