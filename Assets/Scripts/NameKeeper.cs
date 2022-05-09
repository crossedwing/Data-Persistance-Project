using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameKeeper : MonoBehaviour
{
    public static NameKeeper instance;

    public InputField inputField;


    public string playerName;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void SetName()
    {
        playerName = inputField.text;
    }
}
