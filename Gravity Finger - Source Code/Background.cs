using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    private List<Transform> _backgrounds;

    void Start ()
    {
        _backgrounds = new List<Transform>();

        foreach (Transform t in transform)
            _backgrounds.Add(t);
    }
    
    void FixedUpdate ()
    {
        for (int i = 0; i < _backgrounds.Count; i++)
        {
            float spin = 0.03f;
            if (_backgrounds[i].name == "00")
                spin *= -1;
            _backgrounds[i].Rotate(new Vector3(0, 0, spin));
        }
    }
}
