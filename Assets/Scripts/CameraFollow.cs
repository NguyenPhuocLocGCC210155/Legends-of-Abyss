using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float YOffest = 6f;
    public Transform target;
    public float visionSize = -30f;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.25f;

    private void Start() {
        target = PlayerController.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + YOffest, visionSize);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }
}
