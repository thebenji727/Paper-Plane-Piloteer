using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlaneSelector : MonoBehaviour
{
    [Header("Plane Prefabs")]
    [Tooltip("Drag all aircraft prefabs here (e.g. from Generic Aircraft Models/Prefabs/Aircrafts)")]
    public List<GameObject> planePrefabs = new List<GameObject>();

    [Header("UI References")]
    public GameObject selectionPanel;
    public Transform buttonParent;
    public GameObject buttonPrefab; // Prefab of a UI Button with a TMP_Text child

    [Header("Optional")]
    public Button changePlaneButton; // Assign a permanent "Change Plane" button if desired

    private SlingShot slingShot;

    void Start()
    {
        slingShot = FindObjectOfType<SlingShot>();
        if (slingShot == null)
        {
            Debug.LogError("PlaneSelector: No SlingShot found in the scene!");
            return;
        }

        PopulateButtons();
        ShowPanel(); // Show selection popup at start

        if (changePlaneButton != null)
        {
            changePlaneButton.onClick.AddListener(OpenSelection);
        }
    }

    void PopulateButtons()
    {
        // Clear any existing buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject prefab in planePrefabs)
        {
            if (prefab == null) continue;

            GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>();

            if (label != null)
            {
                label.text = CleanName(prefab.name);
            }
            else
            {
                // Fallback if using legacy Text
                Text legacyText = btnObj.GetComponentInChildren<Text>();
                if (legacyText != null) legacyText.text = CleanName(prefab.name);
            }

            // Capture the prefab for the closure
            GameObject selectedPrefab = prefab;
            btn.onClick.AddListener(() => SelectPlane(selectedPrefab));
        }
    }

    string CleanName(string name)
    {
        // Make names nicer, e.g. "aircraft-a" -> "Aircraft A"
        name = name.Replace("aircraft-", "Aircraft ");
        name = name.Replace("-", " ");
        if (name.Length > 0)
            name = char.ToUpper(name[0]) + name.Substring(1);
        return name;
    }

    public void SelectPlane(GameObject prefab)
    {
        if (slingShot != null && prefab != null)
        {
            slingShot.projectilePrefab = prefab;
            Debug.Log("Selected plane for slingshot: " + prefab.name);
        }
        HidePanel();
    }

    public void ShowPanel()
    {
        if (selectionPanel != null)
            selectionPanel.SetActive(true);

        // Optional: pause while choosing (comment out if you don't want pause)
        Time.timeScale = 0f;
    }

    public void HidePanel()
    {
        if (selectionPanel != null)
            selectionPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSelection()
    {
        ShowPanel();
    }
}