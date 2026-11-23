using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows a victory panel when a delivery is successful.
/// "Back to Start" button resets the board and re-randomizes the delivery zone.
/// Compatible with Unity 2022.3+.
/// </summary>
public class VictoryScreen : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("The panel GameObject to show/hide (should start inactive)")]
    public GameObject victoryPanel;

    [Tooltip("Text for the score (TMP or legacy)")]
    public TMP_Text scoreText;
    public Text legacyScoreText;

    [Tooltip("Text for stars / rating")]
    public TMP_Text starsText;
    public Text legacyStarsText;

    [Header("References")]
    [Tooltip("Optional: assign so the Back button can reset the board")]
    public ResetBoard resetBoard;

    [Tooltip("Optional: assign so the Back button also moves the zone")]
    public RandomizeDeliveryZone zoneRandomizer;

    [Header("Settings")]
    [Tooltip("Delay before showing the panel after delivery (seconds)")]
    public float showDelay = 0.8f;

    private int lastScore;
    private int lastStars;

    void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Auto-find if not assigned
        if (resetBoard == null)
            resetBoard = FindObjectOfType<ResetBoard>();

        if (zoneRandomizer == null)
            zoneRandomizer = FindObjectOfType<RandomizeDeliveryZone>();

        // Subscribe to any DeliveryTarget in the scene
        var targets = FindObjectsOfType<DeliveryTarget>();
        foreach (var target in targets)
        {
            target.onDelivery.AddListener(OnDelivery);
        }
    }

    void OnDelivery(int score)
    {
        lastScore = score;
        // Rough star calculation matching DeliveryTarget
        lastStars = score >= 200 ? 3 : score >= 100 ? 2 : 1;

        if (showDelay <= 0f)
            ShowPanel();
        else
            Invoke(nameof(ShowPanel), showDelay);
    }

    void ShowPanel()
    {
        if (victoryPanel == null) return;

        victoryPanel.SetActive(true);

        string scoreStr = $"Score: {lastScore}";
        string starsStr = new string('★', lastStars) + new string('☆', 3 - lastStars);

        if (scoreText != null) scoreText.text = scoreStr;
        if (legacyScoreText != null) legacyScoreText.text = scoreStr;

        if (starsText != null) starsText.text = starsStr;
        if (legacyStarsText != null) legacyStarsText.text = starsStr;
    }

    /// <summary>
    /// Wire this to the "Back to Start" button OnClick.
    /// </summary>
    public void BackToStart()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Reset the board (destroys plane, returns camera)
        if (resetBoard != null)
            resetBoard.DoReset();

        // Re-randomize the delivery zone
        if (zoneRandomizer != null)
            zoneRandomizer.Randomize();
    }
}
