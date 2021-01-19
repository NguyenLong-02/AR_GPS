using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUpdate : MonoBehaviour
{
    [SerializeField] Transform Player_Position;
    Vector3 velocity;
    float SmoothStep = 0.1f;
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 New_Position = Player_Position.position;
        New_Position.y = transform.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, New_Position, ref velocity, SmoothStep);
    }
}
