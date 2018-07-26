using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

public class StartMode : MonoBehaviour
{
    public int unlockedCoins = 0;
    public string levelStartName;

    private Transform _player;
    private Text _winText, _totalScore;
    private float _timeLimit, _transitionTick;
    private bool _locked;

    public static int MENUCOINS = 0;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _winText = GameObject.Find("Result").GetComponent<Text>();

        _totalScore = GameObject.Find("TotalScore").GetComponent<Text>();
        _totalScore.text = ScoreMode.TOTALSCORE.ToString();

        _timeLimit = 0.5f;
        _transitionTick = 0;

        if(Player.COINS > MENUCOINS)
            MENUCOINS = Player.COINS;

        if (MENUCOINS >= unlockedCoins)
            _locked = false;
        else
            _locked = true;
    }

    void Update()
    {
        if (_timeLimit < 0)
            HandleModeStart();
    }

    private void LightHoleTimer(bool enable)
    {
        _timeLimit -= enable ? GetPlayerSpeed : 0;
        _winText.enabled = enable;
        if (_timeLimit < 0)
        {
            ShowEnd();
            enable = false;
            _player.GetComponent<Player>().FreezeBox();
        }
        if (enable)
            ShowTimer(_timeLimit);
    }

    private void HandleModeStart()
    {
        _transitionTick += Time.deltaTime;
        if (_transitionTick >= 1.5f)
        {
            Player.COINS = 0;
            SceneManager.LoadScene(levelStartName);
        }
        ShowEnd();
    }

    private void LockedText()
    {
        _winText.enabled = true;
        _winText.text = MENUCOINS + "/" + unlockedCoins + " COIN" + (unlockedCoins == 1 ? "" : "S");
    }

    private void ShowEnd()
    {
        _winText.text = "0.00";
    }

    private void ShowTimer(float time)
    {
        TimeSpan ts = TimeSpan.FromSeconds(time);

        string ms = (ts.Milliseconds / 10).ToString();
        _winText.text = string.Format("{0}:{1}", ts.Seconds, ms);
    }

    private float GetPlayerSpeed
    {
        get { return _player.GetComponent<Rigidbody2D>().velocity.magnitude / 1000; }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (_locked)
                LockedText();
            else
                LightHoleTimer(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            LightHoleTimer(false);
    }
}
