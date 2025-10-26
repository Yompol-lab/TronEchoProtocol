using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string nombre;
        public GameObject prefab;
        public int cantidad;
    }

    [Header("Lista de Motos")]
    public List<Pool> motos = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;

        foreach (Pool pool in motos)
        {
            Queue<GameObject> cola = new Queue<GameObject>();

            for (int i = 0; i < pool.cantidad; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                cola.Enqueue(obj);
            }

            poolDictionary.Add(pool.nombre, cola);
        }
    }

    public GameObject GetMoto(string nombre)
    {
        if (!poolDictionary.ContainsKey(nombre))
        {
            Debug.LogWarning("No existe el pool para: " + nombre);
            return null;
        }

        GameObject obj = poolDictionary[nombre].Dequeue();
        obj.SetActive(true);
        poolDictionary[nombre].Enqueue(obj);
        return obj;
    }
}
