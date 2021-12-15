using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public float _speed;

    private Vector3 _currentTarget;
    private bool isInverseWay = false;
    private Rigidbody _rigidbody;
    private Vector3 _velocity = Vector3.zero;

    public float _acc = 1f;

    private bool reachedTarget = true;

    public bool isReachedTarget()
    {
        return reachedTarget;
    }


    public void setTarget(Vector3 target)
    {
        reachedTarget = false;
        _currentTarget = target;
    }

    public Vector3 velocity()
    {
        return _velocity;
    }


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!reachedTarget)
        {
            Debug.Log(_velocity);

            Vector3 oldPosition = _rigidbody.position;

            Vector3 velocityDirection = (_currentTarget-transform.position).normalized;

            float currentSpeed = _velocity.magnitude;

            currentSpeed = Mathf.Min( currentSpeed + _acc * Time.deltaTime , _speed);

            Vector3 velocityMax = Vector3.MoveTowards(_velocity, velocityDirection * _speed, _acc*Time.fixedDeltaTime);

            _rigidbody.position = Vector3.MoveTowards(transform.position, _currentTarget, velocityMax.magnitude * Time.fixedDeltaTime);

            _velocity = (_rigidbody.position - oldPosition) / Time.fixedDeltaTime;

            if(oldPosition == _rigidbody.position)
            {
                reachedTarget = true;
                _velocity = Vector3.zero;
            }
        }
    }

    
}
