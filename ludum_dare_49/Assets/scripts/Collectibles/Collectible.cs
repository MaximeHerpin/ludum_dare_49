using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    public LayerMask mask;
    public System.Action<GameObject> onCollected;
    public bool DestroyOnCollect = true;
    private bool hasBeenCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenCollected)
            return;

        if ((mask.value & 1 << other.gameObject.layer) > 0)
        {
            onCollected?.Invoke(gameObject);
            hasBeenCollected = true;
            Destroy(gameObject);
        }
    }


      
}
