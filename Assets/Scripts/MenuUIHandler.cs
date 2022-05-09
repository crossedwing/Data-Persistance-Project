using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public Text highScoreText;
    private void Start()
    {
        MainManager.NameSaver currentLeader = new MainManager.NameSaver();
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentLeader = JsonUtility.FromJson<MainManager.NameSaver>(json);
        }
        else
        {
            currentLeader.playerName = "";
            currentLeader.score = 0;
        }
        highScoreText.text = "Best Score: " + currentLeader.playerName + ": " + currentLeader.score;
        Debug.Log(currentLeader.playerName);
        Debug.Log(currentLeader.score);
    }
    public void StartGame()
    {
        NameKeeper.instance.SetName();
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
