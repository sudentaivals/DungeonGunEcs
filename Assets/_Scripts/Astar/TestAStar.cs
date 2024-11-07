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
        if(GridManager.Instance.CurrentGrid == null) return;
        var coordinates = GridManager.Instance.CurrentGrid.GetGridCoordinates(transform.position);
        Debug.Log($"X: {coordinates.Item1}, Y: {coordinates.Item2}");
    }
}
