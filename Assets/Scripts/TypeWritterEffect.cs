using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    [SerializeField] private AudioSource soundEffect;

    public float delayBetweenChars = 0.05f;

    private string fullText;
    private float timer;
    private int currentIndex;
    public bool fullyDisplayed;

    void Start()
    {
        fullyDisplayed = false;
        fullText = objectiveText.text;
        objectiveText.text = "";
        timer = 0f;
        currentIndex = 0; 

    }

    void Update()
    {

        if (fullyDisplayed)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= delayBetweenChars)
        {
            if (!soundEffect.isPlaying)
            {
                soundEffect.Play();
            }

            timer = 0f;
            objectiveText.text += fullText[currentIndex];

            currentIndex++;

            if (currentIndex >= fullText.Length)
            {
                fullyDisplayed = true;
            }
        }
    }

}