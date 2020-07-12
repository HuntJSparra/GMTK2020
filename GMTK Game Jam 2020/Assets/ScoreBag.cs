using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScoreBag : MonoBehaviour
{
    public TextMeshProUGUI nameGUI;
    public TextMeshProUGUI titleGUI;
    public TextMeshProUGUI scoreGUI;

    public void Initialize(string name, string title, int score)
    {
        nameGUI.text = name;
        titleGUI.text = title;
        scoreGUI.text = score.ToString();
    }
}
