using UnityEngine;

public class bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("bullet"))
            Destroy(this.gameObject);
    }
}
