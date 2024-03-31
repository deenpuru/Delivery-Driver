using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    [SerializeField] private float _moveXSpeed = 1.0f;
    [SerializeField] private float _moveYSpeed = 0.01f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _destroyDelay = 0;
    [SerializeField] private float _turnTiny = 1f;
    [SerializeField] private float _turnNormal = 1f;
    [SerializeField] private float _turnBig = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Color32 _hasPackageColor = new Color32(1 ,1 ,1 ,1);
    [SerializeField] private Color32 _noPackageColor = new Color32(1, 1, 1, 1);

    private bool _hasPackage;
    private bool _isJumping;


    void Update()
    {
        float _moveX = Input.GetAxis("Horizontal") * _moveXSpeed * Time.deltaTime;
        float _moveY = Input.GetAxis("Vertical") * _moveYSpeed * Time.deltaTime;
        transform.Translate(_moveX,0, 0);
        transform.Translate(0,_moveY, 0);
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (Input.GetAxis("Horizontal") >= 0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetAxis("Horizontal") <= -0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            rb.AddForce(new Vector2(rb.velocity.x, _jumpHeight));
        }
      
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectable" && !_hasPackage)
        {
            _hasPackage = true;
            Debug.Log("Package is picked up");
            Destroy(other.gameObject, _destroyDelay);
            gameObject.GetComponent<SpriteRenderer>().color = _noPackageColor;
        }

        if (other.tag == "Destination" && _hasPackage)
        {
            _hasPackage = false;
            Debug.Log("Package delivered");
            gameObject.GetComponent<SpriteRenderer>().color = _hasPackageColor;
        }

        if (other.tag == ("ExtraJump"))
        {
            Debug.Log("touched the extra jump cube");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump");
                rb.AddForce(new Vector2(rb.velocity.x, _jumpHeight));
            }
        }


        switch (other.tag)
        {
            case "Tiny":
                transform.localScale = new Vector3(_turnTiny, _turnTiny, 0);
                break;
            case "Normal":
                transform.localScale = new Vector3(_turnNormal, _turnNormal, 0);
                break;
            case "Big":
                transform.localScale = new Vector3(_turnBig, _turnBig, 0);
                break;
            default:
                break;
        }
    }
}

