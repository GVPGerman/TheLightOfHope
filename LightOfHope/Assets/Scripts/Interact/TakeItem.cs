using UnityEngine;

public class TakeItem : PicableItem
{
    private Item currentItem; // ������� ������� � �����
    public float minThrowForce = 5f; // ����������� ���� ������
    public float maxThrowForce = 15f; // ������������ ���� ������
    public float chargeSpeed = 2f; // �������� ���������� ����
    public float pickupDistance = 3f; // ��������� �������
    public LayerMask itemLayer; // ���� ��� ���������
    public Camera mainCamera; // �������� ������
    private float currentThrowForce; // ������� ���� ������
    private bool isCharging; // ���� ���������� ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // ������ ��������
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentItem != null) // ������ ������
        {
            StartCharging();
        }

        if (Input.GetKey(KeyCode.Q) && isCharging) // ���������� ������ ��� ���������� ����
        {
            ChargeThrow();
        }

        if (Input.GetKeyUp(KeyCode.Q) && isCharging) // ��������� ������ ��� ������
        {
            ThrowItem();
        }
    }

    void StartCharging()
    {
        isCharging = true; // �������� ���������� ����
        currentThrowForce = 0; // ���������� ������� ����
    }

    void ChargeThrow()
    {
        currentThrowForce += chargeSpeed * Time.deltaTime; // ����������� ���� ������
        currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce); // ������������ ����
    }

    void TryPickupItem()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector2.right), pickupDistance, itemLayer);

        if (hit.collider != null)
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                if (currentItem == null) // ������, ���� � ����� ��� ��������
                {
                    currentItem = item;
                    item.gameObject.SetActive(false); // �������� �������
                }
                else // ���� � ����� ��� ���� �������
                {
                    ThrowItem(); // ����������� ������� �������
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
            currentItem = null; // ���������� ������� �������
            isCharging = false; // ��������� ���������� ����
        }
    }
}