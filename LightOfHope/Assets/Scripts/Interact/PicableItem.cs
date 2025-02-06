using UnityEngine;

public class PicableItem : MonoBehaviour
{
    protected bool _canPick;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _canPick = true;
            Debug.Log("Можно взять"); // текст о том, что можноо взять объект
        }
    }
}
