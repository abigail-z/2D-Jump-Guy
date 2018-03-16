using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsManager : MonoBehaviour
{
    private PickupBehavior[] pickups;

	void Start()
    {
        pickups = GetComponentsInChildren<PickupBehavior>();
        uint i = 0;
        foreach (PickupBehavior pickup in pickups)
        {
            pickup.gameObject.SetActive(false);
            pickup.Index = i;
            i++;
        }

        uint index = (uint)Random.Range(0, pickups.Length);
        pickups[index].gameObject.SetActive(true);
    }

    public void PickupCollected(uint i)
    {
        uint index;
        do
        {
            index = (uint)Random.Range(0, pickups.Length);
        }
        while (index == i);
#if UNITY_EDITOR
        Debug.Log("Spawning pickup " + index);
#endif

        pickups[index].gameObject.SetActive(true);
    }
}
