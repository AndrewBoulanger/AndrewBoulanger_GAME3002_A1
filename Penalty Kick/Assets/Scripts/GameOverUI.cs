using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_ResultsText = null;
    // Start is called before the first frame update
    private int m_score = 0;

    private void OnEnable()
    {
        m_score = PlayerPrefs.GetInt("Score");
        m_ResultsText.text = "Your Score: " + m_score + "/10";
    }
}
