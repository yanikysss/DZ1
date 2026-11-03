using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float maxSpeed = 5f;        // Максимальная скорость
    public float acceleration = 30f;   // Плавность разгона

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        // Получаем компонент физики при старте
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Считываем нажатия клавиш (WASD или стрелки)
        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
        }

        // Нормализуем вектор, чтобы по диагонали скорость не была выше
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Применяем физику движения (плавный разгон и торможение)
        Vector2 targetVelocity = moveInput * maxSpeed;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
}