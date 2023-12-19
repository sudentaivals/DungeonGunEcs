using System.Collections;
using System.Collections.Generic;
using CustomAstar;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    private Astar _aStar;

    private void Start()
    {
        _aStar = new Astar();
    }

    void Update()
    {
        var coordinates = GridManager.Instance.GetGridCoordinates(transform.position);
        Debug.Log($"X: {coordinates.Item1}, Y: {coordinates.Item2}");
    }
}
