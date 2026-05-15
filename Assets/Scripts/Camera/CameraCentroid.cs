using DG.Tweening;
using System;
using UnityEngine;

public class CameraCentroid : MonoBehaviour
{
    [SerializeField] private Transform[] _followTargets;
    [SerializeField] private float[] _followTargetsWeights;
    [SerializeField] private float _followSpeed = 5f;
    private Vector3 _camVelocityRef;

    private void Start()
    {
        Vector3 centroid = _CalculateTargetsCentroid();
        Vector3 newPosition = transform.position;
        newPosition.x = centroid.x;
        newPosition.y = centroid.y;
        transform.position = newPosition;
    }

    private void LateUpdate()
    {
        Vector3 centroid = _CalculateTargetsCentroid();

        //Vector3 newPosition = transform.position;
        //newPosition.x = Mathf.Lerp(transform.position.x, centroid.x, Time.deltaTime * _followSpeed);
        //newPosition.y = Mathf.Lerp(transform.position.y, centroid.y, Time.deltaTime * _followSpeed);
        //newPosition.x = centroid.x;
        //newPosition.y = centroid.y;
        //transform.position = newPosition;

        transform.position = Vector3.SmoothDamp(transform.position, centroid, ref _camVelocityRef, 1f / _followSpeed * Time.deltaTime);
    }

    private Vector3 _CalculateTargetsCentroid()
    {
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < _followTargets.Length; ++i)
        {
            Transform target = _followTargets[i];
            float weight = _followTargetsWeights[i];
            centroid += target.position * weight;
        }

        centroid /= _followTargets.Length;
        return centroid;
    }
}
