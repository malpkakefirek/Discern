using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CutsceneController : MonoBehaviour
{   

    [SerializeField] public MainMenuScript mainMenuScript;
    [SerializeField] public TextMeshProUGUI m_Text;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private TypewriterEffect twScript;
    [SerializeField] public TextMeshProUGUI objectiveText;
    private const int GAMEPLAY_SCENE = 1;
    private bool continueMsgShown = false;

    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GAMEPLAY_SCENE, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f && twScript.fullyDisplayed)
            {   
                if (!continueMsgShown)
                {
                    continueMsgShown = true;
                    yield return new WaitForSeconds(0.5f);
                    m_Text.text = "Press the space bar to continue";
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Text.text = "";
                    objectiveText.text = "";
                    soundEffect.Play();

                    while (soundEffect.isPlaying)
                    {
                        yield return null;
                    }
                    
                    asyncOperation.allowSceneActivation = true;
                }

            }

            yield return null;
        }
    }

}
