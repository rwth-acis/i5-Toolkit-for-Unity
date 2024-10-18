using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlet : MonoBehaviour
{
    [SerializeField] private bool usePool = false;

    [SerializeField] private GameObject prefab;
    [SerializeField] private float averageUpwardsVelocity = 10f;
    [SerializeField] private float sideDeviation = 0.5f;
    [SerializeField] private int maximumNumberOfObjects = 200;

    private const float ejectionRate = 0.01f;

    private Queue<GameObject> objects = new Queue<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(Eject), 0f, ejectionRate);
    }

    private void Eject()
    {
        GameObject instance;

        if (usePool)
        {
            instance = ObjectPool<GameObject>.RequestResource(() => { return Instantiate(prefab); });
            instance.SetActive(true);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
        }
        else
        {
            instance = Instantiate(prefab, transform.position, transform.rotation);
        }

        ConstantMovement movementScript = instance.GetComponent<ConstantMovement>();
        movementScript.Velocity = new Vector3(1.5f * instance.transform.localScale.x / ejectionRate, 0, 0);

        objects.Enqueue(instance);

        while (objects.Count > maximumNumberOfObjects)
        {
            GameObject obj = objects.Dequeue();

            if (usePool)
            {
                obj.SetActive(false);
                ObjectPool<GameObject>.ReleaseResource(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }
}
