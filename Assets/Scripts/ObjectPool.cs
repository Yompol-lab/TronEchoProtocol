using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    [SerializeField] private GameObject motoPrefab;
    [SerializeField] private int poolSize = 3;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject moto = Instantiate(motoPrefab);
            moto.SetActive(false);
            pool.Enqueue(moto);
        }
    }

    public GameObject GetMoto()
    {
        GameObject moto = pool.Count > 0 ? pool.Dequeue() : Instantiate(motoPrefab);
        moto.SetActive(true);
        return moto;
    }

    public void ReturnMoto(GameObject moto)
    {
        moto.SetActive(false);
        pool.Enqueue(moto);
    }
}
