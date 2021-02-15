using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_DirectionText = null;
    [SerializeField]
    private TextMeshProUGUI m_KickStrengthText = null;
    [SerializeField]
    private TextMeshProUGUI m_ScoreText = null;
    [SerializeField]
    private TextMeshProUGUI m_SpeedText = null;
    [SerializeField]
    private Slider m_KickScrollBar = null;

    public void OnRequestUpdateUI(Vector3 vDir, float fKickStrength, int iScore)
    {
        m_DirectionText.text = "direction: " + vDir.ToString();
        m_KickStrengthText.text = "Kick Strenght: " + fKickStrength + " m/s";
        UpdateScore(iScore);
        UpdateSpeed(0);
    }

    public void UpdateKickStrength(float fKickStrength)
    {
        m_KickStrengthText.text = "Kick Strenght: " + fKickStrength + " m/s";
        m_KickScrollBar.value = fKickStrength;
    }

    public void UpdateScore(int score)
    {
        m_ScoreText.text = "Score: " + score;
    }
    public void UpdateSpeed(float speed)
    {
        m_SpeedText.text = "Speed: " + speed + " m/s";
    }
}
