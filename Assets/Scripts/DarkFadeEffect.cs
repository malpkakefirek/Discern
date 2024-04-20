using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DarkFadeEffect : MonoBehaviour
{
    [SerializeField] private Image panelImage; 

    void Start()
    {
        // Check if the panelImage reference is assigned
        if (panelImage != null)
        {
            // Fade out the panel gradually
            StartCoroutine(FadeOutPanel());
        }
        else
        {
            Debug.LogError("Panel Image component not assigned!");
        }
    }

    IEnumerator FadeOutPanel()
    {
        while (panelImage.color.a > 0.05)
        {
            Color newColor = panelImage.color;
            newColor.a -= Time.deltaTime * 0.5f;
            panelImage.color = newColor;
            yield return null;
        }

        panelImage.gameObject.SetActive(false);
    }
}
