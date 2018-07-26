using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class MemoryCard : MonoBehaviour
{
    public bool save = true;

    private string filePath;

    private static bool _FIRSTBURST = false;

    void Awake ()
    {
        if (!save)
            return;
        filePath = Application.persistentDataPath + "/" + "MemoryCard.txt";
        if (!_FIRSTBURST)
            LoadData();
        else
            SaveData();
        _FIRSTBURST = true;
    }

    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            //0: Level name. 1: Player Coins. 2: Menu coins. 3: Total score.
            string[] lines = File.ReadAllLines(filePath);

            SceneManager.LoadScene(lines[0]);

            int num;
            if (int.TryParse(lines[1], out num))
                Player.COINS = num;
            if (int.TryParse(lines[2], out num))
                StartMode.MENUCOINS = num;
            if (int.TryParse(lines[3], out num))
                ScoreMode.TOTALSCORE = num;
        }
        else
            SaveData();
    }

    public void SaveData()
    {
        //0: Level name. 1: Player Coins. 2: Menu coins. 3: Total score.
        string[] newLines = new string[4];
        newLines[0] = SceneManager.GetActiveScene().name;
        newLines[1] = Player.COINS.ToString();
        newLines[2] = StartMode.MENUCOINS.ToString();
        newLines[3] = ScoreMode.TOTALSCORE.ToString();

        File.WriteAllLines(filePath, newLines);
    }
}
