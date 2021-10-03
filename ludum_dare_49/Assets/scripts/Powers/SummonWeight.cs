using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SummonWeight : MonoBehaviour
{
    public bool canSummon;
    public float coolDown = 5;
    public Rigidbody weightPrefab;
    public float forceAmount;
    public float delay = 2;
    public float speed = 100;
    public float height = 50;


    private void Update()
    {
        if (canSummon && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Summon(transform.position));
        }
    }


    public IEnumerator Summon(Vector3 position)
    {
        canSummon = false;
        float fallDuration = height / speed;
        yield return new WaitForSeconds(delay - fallDuration);
        Vector3 spawnPosition = position + Vector3.up * 50;
        var instance = Instantiate<Rigidbody>(weightPrefab, spawnPosition, Quaternion.identity);
        instance.mass = forceAmount / (speed*speed)*2;
        instance.AddForce(Vector3.down * speed, ForceMode.VelocityChange);

        yield return new WaitForSeconds(coolDown);
        canSummon = true;
    }
}
