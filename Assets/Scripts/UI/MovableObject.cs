using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private Vector3 difference = Vector3.zero;
    GridMap gridMap;
    private void Start()
    {
        gridMap = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();
    }
    private void OnMouseDown()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }
    private void OnMouseDrag()
    {
        gridMap.SetMovableObject(this);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        // see mouse position
        // get grid mouse is on
        // transform.position = the middle of the grid
    }
    private void OnMouseUp()
    {
        gridMap.SS();
        gridMap.SetMovableObject(null);
    }
}
