using UnityEngine;

public class YFloor : MonoBehaviour
{
    [SerializeField] private int floor = -100;

    void Update()
    {
        if (transform.position.y < floor) {
            Destroy(gameObject);
        }
    }
}
