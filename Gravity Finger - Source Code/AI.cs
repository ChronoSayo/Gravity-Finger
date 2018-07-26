using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{
    public Transform icon;
    public float iconOffset;
    public float speed;
    public float repositionThreshold;

    private CallVortex.VortexData _vortex;
    private Transform _box, _icon;
    private Camera _camera;
    private Rigidbody2D _boxPhysics;
    private Vector3 _offScreen;
    private float _gravity, _screenLimit;
    private States _states;

    private enum States
    {
        None, Reposition, Press
    }

    void Start ()
    {
        _box = GameObject.FindGameObjectWithTag("Player").transform;
        _boxPhysics = _box.GetComponent<Rigidbody2D>();

        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _gravity = GameObject.Find("Gravity").GetComponent<Gravity>().GetGravity;

        _offScreen = new Vector3(-100, -100);
        _icon = Instantiate(icon.gameObject).transform;
        RenderIcon(false);
        _vortex = _camera.GetComponent<CallVortex>().AddVortex(Vector2.one * 0.07f, _offScreen, 1, false);

        _screenLimit = 13;

        _states = States.Reposition;
        _icon.position = new Vector3(iconOffset, 0, 0);
    }
    
    void Update ()
    {
        switch(_states)
        {
            case States.None:
                break;
            case States.Reposition:
                Reposition();
                break;
            case States.Press:
                Press();
                break;
        }
    }

    void FixedUpdate()
    {
        AddForce();
    }

    private void Reposition()
    {
        if (_box.position.x + repositionThreshold < _icon.position.x)
            _states = States.Press;
        else
        {
            Vector3 box = _box.position;
            Vector3 newPos = new Vector3(box.x + GetOffset(), box.y, box.z);
            if (newPos.x > _screenLimit)
                newPos.x = _screenLimit;
            _icon.position = newPos;
        }
    }

    private void Press()
    {
        if (_box.position.x + repositionThreshold >= _icon.position.x)
            _states = States.Reposition;
        else
        {
            RenderIcon(true);
            ShowVortex(true, _camera.WorldToViewportPoint(_icon.position));
        }
    }

    private float GetOffset()
    {
        float offset = iconOffset;
        float startDecreaseOffset = 9;
        if (_box.position.x > startDecreaseOffset++)
            offset -= 2;
        if (_box.position.x > startDecreaseOffset++)
            offset -= 1;
        if (_box.position.x > startDecreaseOffset)
            offset -= 1;

        return offset;
    }

    private void AddForce()
    {
        Vector3 box = _box.position;
        Vector3 newPos = new Vector3(box.x + GetOffset(), box.y, box.z);
        if (newPos.x < 0)
            newPos.x = 0;
        if (newPos.x > _screenLimit)
            newPos.x = _screenLimit;
        _icon.position += (newPos - _icon.position) * speed;

        _boxPhysics.AddForce((_icon.position - _box.position).normalized * _gravity);
        _boxPhysics.AddTorque((_icon.position - _box.position).magnitude / 100);
    }

    private void RenderIcon(bool render)
    {
        _icon.GetComponent<Renderer>().enabled = render;
    }

    private void ShowVortex(bool enabled, Vector2 center)
    {
        _vortex.vortex.enabled = enabled;
        _vortex.vortex.center = center;
    }
}
