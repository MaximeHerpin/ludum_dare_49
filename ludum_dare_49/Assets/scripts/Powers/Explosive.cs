using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject mine;
    public Transform mineDeposit;
    public float forceMultiplier = 1;
    public LayerMask environment;
    private Rigidbody rb;
    public int minLevel = 2;

    bool canUse = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canUse = SaveManager.instance.GetHasShownPowerUp();
        LevelsManager.instance.onLevelStart.AddListener(ActivateSkill);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlaceExplosive());
        }
    }

    private void ActivateSkill()
    {
        if (LevelsManager.instance.GetCurrentLevel() == minLevel && !SaveManager.instance.GetHasShownPowerUp())
        {
            canUse = true;
            UIManager.instance.ShowMinePowerUp();
            SaveManager.instance.SetHasShownPowerUp();
        }
    }

    IEnumerator PlaceExplosive()
    {
        if (!Physics.Raycast(mineDeposit.position, -transform.up, out RaycastHit hit, 1, environment))
        {
            yield break;
        }

        var instance = Instantiate(mine, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);

        while (Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }


        Destroy(instance);
        var joint = hit.collider.gameObject.GetComponent<Joint>();
        if (joint == null)
        {
            yield break;
        }


        Vector3 pivot = joint.connectedAnchor;

        float appliedForce = Mathf.Sqrt((pivot - hit.point).magnitude);
        float receivedForce = Mathf.Sqrt((transform.position - pivot).magnitude);

        Vector3 tangent = Vector3.ProjectOnPlane(instance.transform.position - pivot, instance.transform.up).normalized;

        float oppositeSides = Vector3.Dot(transform.position - pivot, tangent) < 0 ? 1 : 0;


        float jumpForce = appliedForce * receivedForce * forceMultiplier * oppositeSides;

        if (Physics.Raycast(mineDeposit.position, -transform.up, out hit, 1, environment))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }


        Rigidbody floorRb = hit.transform.gameObject.GetComponent<Rigidbody>();
        floorRb.AddForceAtPosition(-instance.transform.up * forceMultiplier /2, instance.transform.position, ForceMode.VelocityChange);
    }

}
