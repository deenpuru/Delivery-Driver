using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;
    void Update()
    {
        transform.position = _player.transform.position + new Vector3(0,0,-10);
    }
}
