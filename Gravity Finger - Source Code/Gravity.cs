using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Tobii.EyeTracking;

public class Gravity : MonoBehaviour
{
    public float gravity = 2;
    public Transform icon;

    private CallVortex.VortexData _vortex;
    private Camera _camera;
    private Transform _icon;
    private Vector3 _newPosition, _offScreen;

    private const float Z = 13;

    void Start ()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _offScreen = new Vector3(-100, -100);
        if (icon)
        {
            _icon = Instantiate(icon.gameObject).transform;
            RenderIcon(false);
            _vortex = _camera.GetComponent<CallVortex>().AddVortex(Vector2.one * 0.07f, _offScreen, 1, false);
        }
    }
    
    void Update ()
    {
        NewGravity();
    }

    private void NewGravity()
    {
        bool addForce = (Player.EYE_TRACKER && EyeTracking.GetGazePoint().IsValid) ||
            (!Player.EYE_TRACKER && Input.GetMouseButton(0));
        if (addForce)
        {
            if (Player.EYE_TRACKER)
            {
                _newPosition = _camera.ScreenToWorldPoint(new Vector3(EyeTracking.GetGazePoint().Screen.x,
                    EyeTracking.GetGazePoint().Screen.y, Z));
            }
            else
                _newPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Z));
            if (icon)
            {
                AssignNewPosition();
                RenderIcon(true);
                ShowVortex(true, _camera.WorldToViewportPoint(_newPosition));
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (icon)
            {
                RenderIcon(false);
                ShowVortex(false, _offScreen);
            }
        }
    }

    private void AssignNewPosition()
    {
        if (SceneManager.GetActiveScene().name == "level death 10")
        {
            if (_newPosition.x > 0)
                _newPosition.x = 0;
        }
        _icon.position = _newPosition;
    }

    private void ShowVortex(bool enabled, Vector2 center)
    {
        _vortex.vortex.enabled = enabled;
        _vortex.vortex.center = center;
    }

    private void RenderIcon(bool render)
    {
        _icon.GetComponent<Renderer>().enabled = render;
    }

    public Vector3 NewPosition
    {
        get { return _newPosition; }
    }

    public float GetGravity
    {
        get { return gravity + 5; }
    }

    public bool ButtonClick
    {
        get { return Input.GetMouseButton(0); }
    }
}
