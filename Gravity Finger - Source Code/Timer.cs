using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    public float winTimeLimit;
    public float multiPlayerTimeScale = 1;

    private Text _timerText, _winTimerText;
    private List<Transform> _players;
    private GoalManager _goalManagerScript;
    private float _timeLimit, _winTimeLimit, _gameEndTime, _tick;
    private bool _gameEnd, _VSMode;

    void Start ()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _players = new List<Transform>();
        foreach(GameObject go in players)
            _players.Add(go.transform);

        _VSMode = SceneManager.GetActiveScene().name == "level death 10";

        _goalManagerScript = GameObject.Find("Goal Manager").GetComponent<GoalManager>();

        _timerText = GameObject.Find("TimerText").GetComponent<Text>();
        _timeLimit = timeLimit;
        _winTimerText = GameObject.Find("Result").GetComponent<Text>();
        _winTimeLimit = winTimeLimit;

        _gameEndTime = 1.5f;
        _tick = 0;

        _gameEnd = false;
    }
    
    void Update ()
    {
        ResultText();
        MinuteFormat();
        if (_VSMode)
            CheckGameEndVS();
        else
            CheckGameEnd();
        HandleGameEnd();
    }

    void FixedUpdate()
    {
        TimerTick();
    }

    private void CheckGameEndVS()
    {
        float limit = 16;
        if (_players[0].position.x < -limit)
            GUIEnd(false);
        else if (_players[0].position.x > limit)
            GUIEnd(true);
    }

    private void GUIEnd(bool win)
    {
        ShowZero(ref _timeLimit);
        ShowZero(ref _winTimeLimit);

        ShowResult(win);

        _gameEnd = true;
    }

    private void HandleGameEnd()
    {
        if (GameEnd)
        {
            _tick += Time.deltaTime;
            if (_tick >= _gameEndTime)
            {
                int i = SceneManager.GetActiveScene().buildIndex;
                bool win = _winTimerText.text.Contains("W");
                if (win)
                    i++;
                if (SceneManager.GetActiveScene().name == "Level26" || SceneManager.GetActiveScene().name == "level death 11")
                    i = 0;
                
                SceneManager.LoadScene(i);
            }
        }
    }

    private void CheckGameEnd()
    {
        bool win = (_winTimeLimit <= 0 && _goalManagerScript.AllInsideLightHole);
        if (_timeLimit <= 0 || win)
            GUIEnd(win);
    }

    public void ShowResult(bool win)
    {
        string s = "LOSER";
        if (win)
            s = "WINNER";
        _winTimerText.enabled = true;
        _winTimerText.text = s;
    }

    private void ShowZero(ref float time)
    {
        time = time < 0 ? 0 : time;
    }

    private void ResultText()
    {
        _winTimerText.enabled = _goalManagerScript.AllInsideLightHole;
        if (_VSMode)
            _winTimerText.enabled = false;
    }

    private void TimerTick()
    {
        if (!_goalManagerScript.AllInsideLightHole)
        {
            _winTimerText.enabled = false;
            _timeLimit -= GetPlayersSpeed();
        }
        else
        {
            _winTimerText.enabled = true;
            _winTimeLimit -= GetPlayersSpeed();
        }
    }

    private float GetPlayersSpeed()
    {
        float playerSpeed = 0;

        for (int i = 0; i < _players.Count; i++)
        {
            Transform p = _players[i];
            float baseScale = 1000;
            float velocity = p.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (i > 0)
            {
                float indexScaling = i * multiPlayerTimeScale;
                float scale = baseScale / indexScaling;
                float offsetScaling = scale / 2;
                float lastScale = baseScale + offsetScaling;
                playerSpeed += CalculateScaledVelocity(velocity, lastScale);
            }
            else
                playerSpeed += CalculateScaledVelocity(velocity, baseScale);
        }
        return playerSpeed;
    }

    private float CalculateScaledVelocity(float velocity, float scale)
    {
        return velocity / scale;
    }

    private void MinuteFormat()
    {
        bool specialLevels = SceneManager.GetActiveScene().name == "Level26" || SceneManager.GetActiveScene().name == "level death 10" ||
            SceneManager.GetActiveScene().name == "level death 11";
        if (specialLevels)
            ShowInfinite();
        else
            ShowTimer(_timeLimit, _timerText);
        ShowTimer(_winTimeLimit, _winTimerText);
    }

    private void ShowInfinite()
    {
        _timerText.text = "99:99:99";
    }

    private void ShowTimer(float time, Text text)
    {
        TimeSpan ts = TimeSpan.FromSeconds(time);

        string ms = (ts.Milliseconds / 10).ToString();
        text.text = string.Format("{0}:{1}:{2}", ts.Minutes, ts.Seconds, ms);
    }

    public bool GameEnd
    {
        get { return _gameEnd; }
    }
}
