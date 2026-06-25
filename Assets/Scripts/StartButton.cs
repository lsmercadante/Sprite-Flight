using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private Button startButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("StartButton");

        if (startButton == null)
        {
            Debug.LogError("StartButton not found! Check the Name in UI Builder.");
            return;
        }

        Debug.Log("StartButton found, wiring up click");
        startButton.clicked += StartGame;
    }

    void OnDisable()
    {
        if (startButton != null)
            startButton.clicked -= StartGame;
    }

    void StartGame()
    {
        Debug.Log("Button clicked");
        SceneManager.LoadScene(1);
    }
}