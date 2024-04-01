using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveXSpeed = 1.0f;
    [SerializeField] private float _moveYSpeed = 0.01f;
    [SerializeField] private float _jumpSpeed = 1.0f;
    [SerializeField] private float _extraJumpSpeed = 1.0f;
    [SerializeField] private float _destroyDelay = 0;
    [SerializeField] private float _turnTiny = 1f;
    [SerializeField] private float _turnNormal = 1f;
    [SerializeField] private float _turnBig = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Color32 _hasPackageColor = new Color32(1 ,1 ,1 ,1);
    [SerializeField] private Color32 _noPackageColor = new Color32(1, 1, 1, 1);

    [SerializeField] private Sprite _jumpSprite;
    [SerializeField] private Sprite _standSprite;

    private bool _hasPackage;
    private bool _isJumping;


    void Update()
    {        
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        // movement
        float _moveX = Input.GetAxis("Horizontal") * _moveXSpeed * Time.deltaTime;
        float _moveY = Input.GetAxis("Vertical") * _moveYSpeed * Time.deltaTime;
        transform.Translate(_moveX, 0, 0);
        transform.Translate(0, _moveY, 0);
        
        // flipping the image

        if (Input.GetAxis("Horizontal") >= 0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetAxis("Horizontal") <= -0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        // jumping
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            JumpingSprite();
            _isJumping = true;
            rb.velocity = Vector2.up * _jumpSpeed;
        }  
    }
    private void JumpingSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _jumpSprite;
    }

    private void StandSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _standSprite;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
            StandSprite();
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
            if (Input.GetKey(KeyCode.Space))
            {
                _isJumping = false;

                Debug.Log("Jump");
            }
            
            //rb.velocity = Vector2.up * _jumpSpeed * _extraJumpSpeed;
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

