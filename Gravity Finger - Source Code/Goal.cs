using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    private Camera _camera;
    private GoalManager _goalManagerScript;
    
    void Start()
    {
        _goalManagerScript = GameObject.Find("Goal Manager").GetComponent<GoalManager>();

        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _camera.GetComponent<CallVortex>().AddVortex(new Vector2((transform.localScale.x / 100) * 4.5f, 
            (transform.localScale.x / 100) * 4), _camera.WorldToViewportPoint(transform.position + (Vector3.up * 0.6f)), 10);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            _goalManagerScript.Inside(col.transform.GetInstanceID(), transform.position, true);
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            _goalManagerScript.Inside(col.transform.GetInstanceID(), Vector3.zero, false);
    }
}
