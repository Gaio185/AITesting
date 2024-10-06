using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public float radius;
    public float cutoffRadius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DetectionRoutine());
    }

    void Update()
    {
        if(canSeePlayer)
        {
            transform.rotation = Quaternion.LookRotation(playerRef.transform.position - transform.position);
        }
        else if(tag == "Camera")
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(28.248f, 90.049f, 0.023f), 5 * Time.deltaTime);
        }
    }

    private IEnumerator DetectionRoutine()
    {

        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            if(Vector3.Distance(transform.position, playerRef.transform.position) > cutoffRadius || tag != "Camera")
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        canSeePlayer = true;
                        Debug.Log("Player Detected");
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }  
        }else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
