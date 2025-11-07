using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShapeSpawner : MonoBehaviour
{
    // Spawn position vector (set in Inspector on the GameManager)
    public Vector3 spawnPosition = new Vector3(0f, 0f, 0f);

    public GameOverUI gameOverUI;
    public CameraSwitcher cameraSwitcher;

    // --- Core Game Settings ---
    private const float DesiredSpawnInterval = 1f;
    private const int DesiredMaxShapes = 5;

    public float spawnInterval = DesiredSpawnInterval;
    public int maxShapesOnScreen = DesiredMaxShapes;

    // --- SCORING ADDITIONS ---
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;

    private float timer = 0f;
    private int currentShapeCount = 0;
    private bool gameActive = true;

    void Awake()
    {
        spawnInterval = DesiredSpawnInterval;
        maxShapesOnScreen = DesiredMaxShapes;
    }

    void Start()
    {
        UpdateScoreDisplay();
    }

    void Update()
    {
        if (!gameActive) return;

        if (currentShapeCount >= maxShapesOnScreen)
        {
            GameOver();
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomShape();
            timer = 0f;
        }
    }

    void SpawnRandomShape()
    {
        int shapeType = Random.Range(0, 2);
        GameObject shape;

        float shapeScale = 3f;
        float colliderSize = 4.5f;

        // ALL shapes are created as cubes
        shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer renderer = shape.GetComponent<Renderer>();

        if (shapeType == 0)
        {
            // --- RED SHAPE ---
            shape.name = "RedShape";
            shape.tag = "RedShape"; // ⭐️ NEW UNIQUE TAG
            renderer.material.color = Color.red;
        }
        else
        {
            // --- BLUE SHAPE ---
            shape.name = "BlueShape";
            shape.tag = "BlueShape"; // ⭐️ NEW UNIQUE TAG
            renderer.material.color = Color.blue;
        }

        shape.transform.localScale = Vector3.one * shapeScale;
        shape.transform.rotation = Quaternion.identity;

        Rigidbody rb = shape.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        // Use the public Vector3 to set the X and Z spawn coordinates.
        shape.transform.position = new Vector3(spawnPosition.x, shape.transform.position.y, spawnPosition.z);

        Collider shapeCollider = shape.GetComponent<Collider>();
        if (shapeCollider != null && shapeCollider is BoxCollider boxCollider)
        {
            boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize);
        }

        shape.transform.SetParent(null);

        MovingDraggableShape draggable = shape.AddComponent<MovingDraggableShape>();
        draggable.spawner = this;

        currentShapeCount++;
    }

    // --- (Methods for scoring, game state, etc. are unchanged) ---
    public void OnShapeDestroyed()
    {
        currentShapeCount--;
    }

    void GameOver()
    {
        gameActive = false;
        gameOverUI.ShowGameOver();
    }

    public void RestartGame()
    {
        currentScore = 0;
        UpdateScoreDisplay();

        GameObject[] redShapes = GameObject.FindGameObjectsWithTag("RedShape");
        GameObject[] blueShapes = GameObject.FindGameObjectsWithTag("BlueShape");

        foreach (GameObject obj in redShapes) Destroy(obj);
        foreach (GameObject obj in blueShapes) Destroy(obj);

        currentShapeCount = 0;
        timer = 0f;

        spawnInterval = DesiredSpawnInterval;
        maxShapesOnScreen = DesiredMaxShapes;

        gameActive = true;
        gameOverUI.HideGameOver();
    }

    public void EndGame()
    {
        GameObject[] redShapes = GameObject.FindGameObjectsWithTag("RedShape");
        GameObject[] blueShapes = GameObject.FindGameObjectsWithTag("BlueShape");
        foreach (GameObject obj in redShapes) Destroy(obj);
        foreach (GameObject obj in blueShapes) Destroy(obj);

        currentShapeCount = 0;
        gameOverUI.HideGameOver();
        gameActive = false;

        SceneManager.LoadScene("SampleScene");

        Debug.Log("Minigame Ended. Loading SampleScene.");
    }

    public void StartMinigame()
    {
        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchTo2DMinigame();
        }
        RestartGame();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Points: " + currentScore.ToString();
        }
    }
}