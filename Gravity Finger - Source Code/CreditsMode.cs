using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsMode : MonoBehaviour
{
    private Text _text;
    private Transform _lightHole, _box;

    void Start ()
    {
        _text = GameObject.Find("TimerText").GetComponent<Text>();
        _lightHole = GameObject.Find("Light Hole").transform;
        _box = GameObject.FindGameObjectWithTag("Player").transform;

        InsertText();
    }

    void Update()
    {
        if (Vector3.Distance(_lightHole.position, _box.position) < 2)
            SceneManager.LoadScene("Menu");
    }

    private void InsertText()
    {
        _text.text =
            "PRODUCER" + TripleTab + TripleTab + TripleTab + Tab + Sayo + LineBreak +
            "EXECUTIVE PRODUCER" + TripleTab + Tab + Sayo + LineBreak +
            "EVENT PLANNER" + TripleTab + TripleTab + Tab + Sayo + LineBreak +
            "LEAD GAME DESIGNER" + TripleTab + Tab + Sayo + LineBreak +
            "GAME DESIGNER" + TripleTab + DoubleTab + DoubleTab + Sayo + LineBreak +
            "LEAD LEVEL DESIGNER" + TripleTab + Tab + Sayo + LineBreak +
            "LEVEL DESIGNER" + TripleTab + TripleTab + Tab + Sayo + LineBreak +
            NameOnly(Jean) +
            "LEAD GAMEPLAY DESIGNER" + DoubleTab + Sayo + LineBreak +
            "GAMEPLAY DESIGNER" + TripleTab + Tab + Sayo + LineBreak +
            NameOnly(Jean) +
            NameOnly(Jonas) +
            "LEAD PROGRAMMER" + TripleTab + DoubleTab + Sayo + LineBreak +
            "PROGRAMMER" + TripleTab + TripleTab + DoubleTab + Sayo + LineBreak +
            "LEAD ARTIST" + TripleTab + TripleTab + TripleTab + Sayo + LineBreak +
            "ARTISTS" + TripleTab + TripleTab + TripleTab + DoubleTab + Jean + LineBreak +
            NameOnly(Jonas) +
            "TECH ARTIST" + TripleTab + TripleTab + TripleTab + Jonas + LineBreak +
            "LEAD AUDIO DESIGNER" + TripleTab + Tab + Sayo + LineBreak +
            "AUDIO" + TripleTab + TripleTab + TripleTab + TripleTab + Jean + LineBreak +
            "PLAY TESTERS" + TripleTab + TripleTab + DoubleTab + Jean + LineBreak +
            NameOnly(Jonas);
    }

    private string TripleTab
    {
        get { return DoubleTab + Tab; }
    }

    private string DoubleTab
    {
        get { return Tab + Tab; }
    }

    private string NameOnly(string name)
    {
        return TripleTab + TripleTab + TripleTab + TripleTab + TripleTab + name + LineBreak;
    }

    private string Sayo
    {
        get { return "Sayo Nariño"; }
    }

    private string Jonas
    {
        get { return "Jonas Lewis"; }
    }

    private string Jean
    {
        get { return "Jean Manco"; }
    }

    private string Tab
    {
        get { return "\t"; }
    }

    private string LineBreak
    {
        get { return "\n"; }
    }
}
