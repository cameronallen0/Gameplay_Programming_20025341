using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private PlatformPath _platformPath;
    [SerializeField] private float _speed;
    public GameObject Player;

    private int _targetPointIndex;

    private Transform _previousPoint;
    private Transform _targetPoint;

    private float _timeToPoint;
    private float _elaspedTime;

    void Start()
    {
        TargetNextPoint();
    }
    void FixedUpdate()
    {
        _elaspedTime += Time.deltaTime;

        float elapsedPercentage = _elaspedTime / _timeToPoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousPoint.position, _targetPoint.position, elapsedPercentage);

        if(elapsedPercentage >= 1)
        {
            TargetNextPoint();
        } 
    }

    private void TargetNextPoint()
    {
        _previousPoint = _platformPath.GetPoint(_targetPointIndex);
        _targetPointIndex = _platformPath.GetNextPointIndex(_targetPointIndex);
        _targetPoint = _platformPath.GetPoint(_targetPointIndex);

        _elaspedTime = 0;

        float distanceToPoint = Vector3.Distance(_previousPoint.position, _targetPoint.position);
        _timeToPoint = distanceToPoint / _speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
