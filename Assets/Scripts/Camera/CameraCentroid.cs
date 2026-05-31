using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Splines;

public class CameraCentroid : MonoBehaviour
{
    [SerializeField] private Transform[] _followTargets;
    [SerializeField] private float[] _followTargetsWeights;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private Rect _boundsRect = new Rect(0f, 0f, 10f, 10f);
    [SerializeField] private Camera _camera;
    private Vector3 _position;

    private Vector3 _ref;

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
        //transform.DOMove(centroid, _followSpeed * Time.deltaTime, false).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal);
        transform.position = Vector3.SmoothDamp(transform.position, centroid, ref _ref, 1f / _followSpeed * Time.deltaTime);
    }

    private Vector3 _CalculateTargetsCentroid()
    {
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < _followTargets.Length; ++i)
        {
            Transform target = _followTargets[i];
            float weight = _followTargetsWeights[i];
            centroid += target.position * weight;
            centroid = _CameraClampPosition(centroid);
        }

        centroid /= _followTargets.Length;
        return centroid;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_boundsRect.center, _boundsRect.size);
    }

    private Vector3 _CameraClampPosition(Vector3 position)
    {
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));
        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        if (position.x > _boundsRect.xMax - (screenSize.x / 2f))
        {
            position.x = _boundsRect.xMax - (screenSize.x / 2f);
        }
        if (position.x < _boundsRect.xMin + (screenSize.x / 2f))
        {
            position.x = _boundsRect.xMin + (screenSize.x / 2f);
        }

        if (position.y > _boundsRect.yMax - (screenSize.y / 2f))
        {
            position.y = _boundsRect.yMax - (screenSize.y / 2f);
        }
        if (position.y < _boundsRect.yMin + (screenSize.y / 2f))
        {
            position.y = _boundsRect.yMin + (screenSize.y / 2f);
        }
        Debug.Log(position);
        return position;
    }
}