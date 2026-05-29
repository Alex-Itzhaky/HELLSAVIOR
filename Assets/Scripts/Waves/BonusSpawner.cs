using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    private List<Transform> _spawnPointsList = new List<Transform>();
    private List<bool> _isAvailable = new List<bool>();

    [SerializeField] private GameObject _healthBonusPrefab;
    [SerializeField] private GameObject _invincibilityBonusPrefab;
    [SerializeField] private GameObject _speedBonusPrefab;

    private void Start()
    {
        _spawnPointsList = GetComponentsInChildren<Transform>().ToList<Transform>();
        foreach (Transform t in _spawnPointsList)
        {
            _isAvailable.Add(true);
        }
    }

    public void SpawnBonus()
    {
        GameObject bonusInstance = Instantiate(ChooseRandomBonus());
        Transform selectedTransform = ChooseRandomSpawnPoint();
        bonusInstance.transform.position = selectedTransform.position;
        SetSpawnPointAvailablity(selectedTransform, false);
        bonusInstance.GetComponent<BonusItems>().Init(this, selectedTransform);
    }

    public void SetSpawnPointAvailablity(Transform spawnPoint, bool availability)
    {
        int index = _spawnPointsList.IndexOf(spawnPoint);
        _isAvailable[index] = availability;
    }

    private Transform ChooseRandomSpawnPoint()
    {
        List<Transform> availableTransform = _spawnPointsList
            .Where((t, i) => _isAvailable[i])
            .ToList();

        if (availableTransform.Count == 0)
            return null;
        return availableTransform[Random.Range(0, availableTransform.Count)];
    }

    private GameObject ChooseRandomBonus()
    {
        int randBonus = Random.Range(0, 3);
        switch (randBonus)
        {
            case 0:
                return _healthBonusPrefab;
            case 1:
                return _invincibilityBonusPrefab;
            case 2:
                return _speedBonusPrefab;
            default:
                return _healthBonusPrefab;
        }
    }
}
