using UnityEngine;

public enum ItemType { Damage, Score, Speed, Heal } // Типы предметов

public class Item : MonoBehaviour
{
    public ItemType type; // В инспекторе выберем тип
    public int value = 1; // Сколько урона/очков/лечения даст

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в предмет врезался именно Игрок (у которого стоит тег Player)
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                switch (type)
                {
                    case ItemType.Damage:
                        player.TakeDamage(value);
                        break;
                    case ItemType.Score:
                        player.AddScore(value);
                        break;
                    case ItemType.Heal:
                        player.Heal(value);
                        break;
                    case ItemType.Speed:
                        // Временное ускорение
                        player.maxSpeed *= 1.5f;
                        player.Invoke("ResetSpeed", 3f); // Вернуть скорость через 3 секунды
                        break;
                }
            }
            Destroy(gameObject); // Уничтожаем предмет после подбора
        }
    }
}