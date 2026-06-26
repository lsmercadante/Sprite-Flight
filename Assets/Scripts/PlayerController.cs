using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;



public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    public GameObject boosterFlame;
    private float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public UIDocument uiDocument;
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;

    private Label highScore;

    private bool isDead = false;

    public GameObject borderParent;


    public InputAction moveForward;
    public InputAction lookPosition;
   

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{
    moveForward.Enable();
    lookPosition.Enable();
    rb = GetComponent<Rigidbody2D>();
    scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
    restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
    highScore = uiDocument.rootVisualElement.Q<Label>("HighScore");
    restartButton.style.display = DisplayStyle.None;
    highScore.style.display = DisplayStyle.None;
    restartButton.clicked += ReloadScene;
}

void Update()
{
    UpdateScore();

    // WasReleasedThisFrame/WasPressedThisFrame are frame-based so must stay in Update
    if (moveForward.WasReleasedThisFrame())
    {
        boosterFlame.SetActive(false);
    }
    else if (moveForward.WasPressedThisFrame())
    {
        boosterFlame.SetActive(true);
    }
}

void FixedUpdate()
{
    if (moveForward.IsPressed())
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
        Vector2 direction = (mousePos - transform.position).normalized;

        transform.up = direction;
        rb.AddForce(direction * thrustForce);
    }
    if (rb.linearVelocity.magnitude > maxSpeed)
    {
        rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}

void UpdateScore()
{
    elapsedTime += Time.deltaTime;
    score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
    scoreText.text = "Score: " + score;
}

IEnumerator FadeInLabel(Label label, float duration)
{
    float timer = 0f;
    label.style.opacity = 0;
    label.style.display = DisplayStyle.Flex;

    while (timer < duration)
    {
        timer += Time.deltaTime;
        label.style.opacity = timer / duration;
        yield return null;
    }

    label.style.opacity = 1;
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (isDead) return;
    isDead = true;
    borderParent.SetActive(false);
    Instantiate(explosionEffect, transform.position, transform.rotation);
    restartButton.style.display = DisplayStyle.Flex;

    int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
    if (score > savedHighScore)
    {
        PlayerPrefs.SetInt("HighScore", (int)score);
        PlayerPrefs.Save();
        savedHighScore = (int)score;
    }

    highScore.text = "High Score: " + savedHighScore.ToString();
    Debug.Log("Score value: " + score);
    Debug.Log("High score label found: " + (highScore != null));

    uiDocument.StartCoroutine(FadeInLabel(highScore, 1f));

    Destroy(gameObject);
}

void ReloadScene()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}


}



