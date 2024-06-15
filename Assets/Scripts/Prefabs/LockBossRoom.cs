using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBossRoom : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 rootPosition;
    [SerializeField] Vector2 hidePosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = hidePosition;
    }

    public void Active(){
        MoveTo(rootPosition);
    }

    public void InActive(){
        MoveTo(hidePosition);
    }

    private void MoveTo(Vector2 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition));
    }

    // Coroutine để di chuyển đối tượng
    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Đợi đến frame tiếp theo
        }

        // Đảm bảo đối tượng dừng chính xác tại vị trí mục tiêu
        transform.position = targetPosition;
    }
}
