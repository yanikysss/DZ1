using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        currentLives = maxLives;

        // Включаем гироскоп (полное имя класса, чтобы не было ошибки)
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }
    }

    void Update()
    {
        Vector2 keyboardInput = Vector2.zero;

        // 1. Ввод с клавиатуры
        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveY = 0f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
            keyboardInput = new Vector2(moveX, moveY);
        }

        // 2. Ввод с джойстика
        Vector2 joystickInput = Vector2.zero;
        if (Gamepad.current != null)
        {
            joystickInput = Gamepad.current.leftStick.ReadValue();
        }

        // 3. Ввод с гироскопа
        Vector2 gyroInput = Vector2.zero;
        if (UnityEngine.InputSystem.Gyroscope.current != null && UnityEngine.InputSystem.Gyroscope.current.enabled)
        {
            Vector3 angularVel = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
            gyroInput = new Vector2(angularVel.y, angularVel.x) * 0.5f;
        }

        // Выбираем самый сильный сигнал
        if (joystickInput.magnitude > keyboardInput.magnitude) keyboardInput = joystickInput;
        if (gyroInput.magnitude > keyboardInput.magnitude) keyboardInput = gyroInput;

        moveInput = keyboardInput.normalized;
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
        if (currentLives > maxLives) currentLives = maxLives;
        Debug.Log("Полечение! Жизней: " + currentLives);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Очки: " + score);
    }

    public void ResetSpeed()
    {
        maxSpeed /= 1.5f;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}