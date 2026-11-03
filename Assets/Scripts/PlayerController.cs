using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // нужно для перезапуска сцены

public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float maxSpeed = 5f;
    public float acceleration = 30f;

    [Header("Характеристики")]
    public int maxLives = 3;
    private int currentLives;
    private int score = 0;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives; // при старте даем полное здоровье
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
        }

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * maxSpeed;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }


    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("Получен урон! Осталось жизней: " + currentLives);

        if (currentLives <= 0)
        {
            RestartLevel();
        }
    }

    public void Heal(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives) currentLives = maxLives; // Не даем превысить максимум
        Debug.Log("Полечение! Жизней: " + currentLives);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Очки: " + score);
    }

    public void ResetSpeed()
    {
        maxSpeed /= 1.5f; // Возвращаем исходную скорость (если было ускорение)
    }

    private void RestartLevel()
    {
        // Перезапускаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}