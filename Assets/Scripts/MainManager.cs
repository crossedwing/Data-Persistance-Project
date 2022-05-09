using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int c_High_Score;
    
    private bool m_GameOver = false;

    private string playerName;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        playerName = NameKeeper.instance.playerName;

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        NameSaver currentLeader = GetHighScore();

        c_High_Score = currentLeader.score;

        highScoreText.text = "Best Score: " + currentLeader.playerName + ": " + currentLeader.score;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                    EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
            }
        }
    }


    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > c_High_Score)
        {
            SetHighScore();
        }
    }

    [System.Serializable]
    public class NameSaver
    {
        public string playerName;
        public int score;
    }
    public void SetHighScore()
    {
        NameSaver player = new NameSaver();
        player.playerName = playerName;
        player.score = m_Points;

        string json = JsonUtility.ToJson(player);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public NameSaver GetHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            NameSaver currentLeader = JsonUtility.FromJson<NameSaver>(json);
            return currentLeader;
        }
        else
        {
            NameSaver currentLeader = new NameSaver();
            currentLeader.playerName = "";
            currentLeader.score = 0;
            return currentLeader;
        }
    }
}
