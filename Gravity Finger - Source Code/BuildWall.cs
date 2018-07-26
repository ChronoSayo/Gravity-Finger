using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildWall : MonoBehaviour
{
    public Transform wall;

    private Camera _camera;
    private List<Transform> _walls;

    private const float Z = 13;

    void Start ()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _walls = new List<Transform>();

        for (int i = 0; i < 4; i++)
            _walls.Add(Instantiate(wall.gameObject).transform);

        SetUpWalls();
    }

    void Update()
    {
        //SetUpWalls();
    }

    private void SetUpWalls()
    {
        float sizeX = _walls[0].transform.localScale.x;
        float fromFullOffset = 20;
        float fromZeroOffset = -15;
        int j = 0;
        //Left
        _walls[j] = SetWall(_walls[j], sizeX, Screen.height / 60,
            _camera.ScreenToWorldPoint(new Vector3(fromZeroOffset, Screen.height, Z)));
        j++;
        //Right
        _walls[j] = SetWall(_walls[j], sizeX, Screen.height / 60,
            _camera.ScreenToWorldPoint(new Vector3(Screen.width - fromFullOffset, Screen.height, Z)));
        j++;
        //Up
        _walls[j] = SetWall(_walls[j], sizeX, Screen.width / 60,
            _camera.ScreenToWorldPoint(new Vector3(0, Screen.height - fromFullOffset, Z)));
        _walls[j].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        j++;
        //Down
        _walls[j] = SetWall(_walls[j], sizeX, Screen.width / 60,
            _camera.ScreenToWorldPoint(new Vector3(0, fromZeroOffset, Z)));
        _walls[j].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    private Transform SetWall(Transform wall, float w, float h, Vector3 v)
    {
        Transform wallInfo = wall;

        Vector3 s = new Vector3(w, h);
        wallInfo.localScale = s;
        wallInfo.position = v;

        return wallInfo;
    }
}
