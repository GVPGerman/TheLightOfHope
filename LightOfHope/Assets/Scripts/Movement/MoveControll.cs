using UnityEngine;

public class MoveControll : TakeItem
{
    [SerializeField, Range(1, 10)] private float _speedCharacter;
    [SerializeField, Range(1, 5)] private float _slowingDownCharacterSpeed;
    [SerializeField] private float _jumpSpeedCharacter;

    [SerializeField] private Rigidbody2D _rigidbodyCharacter;

    private bool _isGround = false;

    private void Update()
    {
        Move();
        Jump();
        Slowing();
    }
    
    private void Move()
    {
        float movementHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(movementHorizontal, 0);
        _rigidbodyCharacter.AddForce(movement * _speedCharacter);
    }

    private void Jump()
    {
        if (_isGround)
        {
            float jumping = Input.GetAxis("Jump");

            _rigidbodyCharacter.velocity = new Vector2(_rigidbodyCharacter.velocity.x, jumping * _jumpSpeedCharacter);
        }
    }

    private void Slowing()
    {
        if (false)
        {
            _speedCharacter -= _slowingDownCharacterSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsGroundedUpdate(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGroundedUpdate(collision, false);
    }

    private void IsGroundedUpdate(Collision2D collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGround = value;
        }
    }

}
