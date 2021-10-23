using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer lr = new LineRenderer();
    GameObject line;
    public Material lineMaterial;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void DrawToPoint(Vector2 point)
    {
        line = new GameObject();
        line.transform.position = transform.position;
        lr = line.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.sortingOrder = 1;
        lr.startWidth = .2f;
        lr.endWidth = .2f;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, point);
    }

    public void DeleteLine()
    {
        Destroy(line);
    }
}
