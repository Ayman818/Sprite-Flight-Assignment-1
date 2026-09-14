using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class PlayerController: MonoBehaviour
{
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    Rigidbody2D rb;
    public float maxSpeed = 5f;
    public GameObject boosterFlame;
    private float elapsedTime = 0f;
    public UIDocument uiDocument;
    private Label scoreText;

    public GameObject explosionEffect;

    private Button restartButton;
    public GameObject borderParent;
    private Label bestScoreText;
    private int highScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;


        bestScoreText = uiDocument.rootVisualElement.Q<Label>("BestScoreLabel");

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        bestScoreText.text = "Best: " + highScore;
    }


    // Update is called once per frame
    void Update()
    {


        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        Debug.Log("Score: " + score);
        scoreText.text = "Score: " + score;
       

       if (score > highScore)
{
    highScore = (int)score;

    bestScoreText.text = "Best Score: " + highScore;

    PlayerPrefs.SetInt("HighScore", highScore);
    PlayerPrefs.Save();
}






        if (Mouse.current.leftButton.isPressed)
    {//Calculate mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        
        
        Vector2 direction = (mousePos - transform.position).normalized;
        // mover player in direction of mouse 
        transform.up = direction;
        rb.AddForce(direction * thrustForce);
        if (rb.linearVelocity.magnitude > maxSpeed)
{
rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
}

    }
    if (Mouse.current.leftButton.wasPressedThisFrame)
{
boosterFlame.SetActive(true);
}
else if (Mouse.current.leftButton.wasReleasedThisFrame)
{
boosterFlame.SetActive(false);
}
        
    }
    void OnCollisionEnter2D(Collision2D collision)
{
Destroy(gameObject);
Instantiate(explosionEffect, transform.position, transform.rotation);
restartButton.style.display = DisplayStyle.Flex;


borderParent.SetActive(false);
}
void ReloadScene()
{
SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}


}
