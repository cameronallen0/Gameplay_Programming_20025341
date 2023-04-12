using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpManager : MonoBehaviour
{
    private bool speedPickUpActive = false;
    public TrailRenderer trail;

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("PickUp"))
        {
            GameObject pickup = col.gameObject;
            PickUpType pickupHit;
            if(pickup.TryGetComponent<PickUpType>(out pickupHit))
            {
                if(pickupHit.type == "speed")
                {
                    Debug.Log("Speed Hit");
                    PlayerController playerController;
                    if(TryGetComponent<PlayerController>(out playerController))
                    {
                        playerController.walkSpeed = playerController.walkSpeed * 2f;
                        playerController.runSpeed = playerController.runSpeed * 2f;
                        trail.enabled = true;
                        speedPickUpActive = true;
                    }
                }
                StartCoroutine(Timer(pickupHit.durationSeconds, pickup));
            }
        }
    }

    IEnumerator Timer(float timeDelay, GameObject go)
    {
        go.SetActive(false);

        yield return new WaitForSeconds(timeDelay);

        if(speedPickUpActive)
        {
            speedPickUpActive = false;
            PlayerController playerController;
            if(TryGetComponent<PlayerController>(out playerController))
            {
                playerController.walkSpeed = playerController.walkSpeed / 2f;
                playerController.runSpeed = playerController.runSpeed / 2f;
                trail.enabled = false;
            }
        }

        go.SetActive(true);

    }
}
