using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCarSpawner : MonoBehaviour
{
    public static PlayerCarSpawner Instance { get; private set; }
    [SerializeField] private GameObject[] carsWithAI;
    [SerializeField] private GameObject[] playerCars;

    public GameObject[] CarWithAI => carsWithAI;
    public GameObject PlayerCar { get; private set; }

    public int PlayerCarIndex { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        PlayerCar = playerCars[PlayerCarIndex];
    }

    public void SetPlayerCatIndex(int index)
    {
        PlayerCarIndex = index;
        PlayerCar = playerCars[PlayerCarIndex];
    }
    public GameObject[] GetAvailableCarsWithAI()
    {
        List<GameObject> avialableCars = carsWithAI.ToList();
        avialableCars.RemoveAt(PlayerCarIndex);
        return avialableCars.ToArray();
    }
}
