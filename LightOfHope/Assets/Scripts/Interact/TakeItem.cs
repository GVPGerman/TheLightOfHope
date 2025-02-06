using UnityEngine;

public class TakeItem : PicableItem
{
    private Item currentItem; // Текущий предмет в руках
    public float minThrowForce = 5f; // Минимальная сила броска
    public float maxThrowForce = 15f; // Максимальная сила броска
    public float chargeSpeed = 2f; // Скорость накопления силы
    public float pickupDistance = 3f; // Дистанция подбора
    public LayerMask itemLayer; // Слой для предметов
    public Camera mainCamera; // Основная камера
    private float currentThrowForce; // Текущая сила броска
    private bool isCharging; // Флаг накопления силы

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Подбор предмета
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentItem != null) // Начать бросок
        {
            StartCharging();
        }

        if (Input.GetKey(KeyCode.Q) && isCharging) // Удерживаем кнопку для накопления силы
        {
            ChargeThrow();
        }

        if (Input.GetKeyUp(KeyCode.Q) && isCharging) // Отпустить кнопку для броска
        {
            ThrowItem();
        }
    }

    void StartCharging()
    {
        isCharging = true; // Начинаем накопление силы
        currentThrowForce = 0; // Сбрасываем текущую силу
    }

    void ChargeThrow()
    {
        currentThrowForce += chargeSpeed * Time.deltaTime; // Увеличиваем силу броска
        currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce); // Ограничиваем силу
    }

    void TryPickupItem()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector2.right), pickupDistance, itemLayer);

        if (hit.collider != null)
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                if (currentItem == null) // Подбор, если в руках нет предмета
                {
                    currentItem = item;
                    item.gameObject.SetActive(false); // Скрываем предмет
                }
                else // Если в руках уже есть предмет
                {
                    ThrowItem(); // Выбрасываем текущий предмет
                    currentItem = item;
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    void ThrowItem()
    {
        if (currentItem != null)
        {
            GameObject thrownItem = Instantiate(currentItem.gameObject, transform.position, Quaternion.identity);
            Rigidbody2D rb = thrownItem.AddComponent<Rigidbody2D>();
            Vector2 throwDirection = mainCamera.transform.TransformDirection(Vector2.right);
            rb.AddForce(throwDirection * currentThrowForce, ForceMode2D.Impulse);
            currentItem = null; // Сбрасываем текущий предмет
            isCharging = false; // Отключаем накопление силы
        }
    }
}