using System;
using TMPro;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField] TMP_Text gameTime;

    void Update()
    {
        gameTime.text = Time.timeScale.ToString("F2");
    }
}
