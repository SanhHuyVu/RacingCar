using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{
    public static TrackCheckPoints Instance { get; private set; }
    [SerializeField] private int lapTofinish = 3;
    [SerializeField] private Transform[] startingPoints;
    [SerializeField] private bool spawnCars = true;

    private int laps;
    public List<CheckPointSingle> CheckPointSingleList { get; private set; }
    private int nextCheckpointSingleIndex;

    private List<Placement> placements;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        startingPoints = ShuffleArray(startingPoints);

        CheckPointSingleList = new List<CheckPointSingle>();
        Transform checkPointsTranform = transform.Find("CheckPoints");

        nextCheckpointSingleIndex = 1;
        laps = 0;

        placements = new List<Placement>();

        foreach (Transform checkPointSingleTranform in checkPointsTranform)
        {
            CheckPointSingle checkPointSingle = checkPointSingleTranform.gameObject.GetComponent<CheckPointSingle>();

            CheckPointSingleList.Add(checkPointSingle);

            if (CheckPointSingleList.IndexOf(checkPointSingle) != nextCheckpointSingleIndex)
            {
                checkPointSingleTranform.gameObject.SetActive(false);
            }

            checkPointSingle.SetTrackCheckPoints(this);
        }
    }
    private void Start()
    {
        HUD.Instance?.UpdateLapCount($"{laps}/{lapTofinish}");
        if (spawnCars)
            SpawnCars();
    }

    private void SpawnCars()
    {
        var playerCar = Instantiate(PlayerCarSpawner.Instance.PlayerCar);

        playerCar.transform.position = new Vector3(startingPoints[0].position.x, startingPoints[0].position.y + 0.7f, startingPoints[0].position.z); ;
        int place = Int32.Parse(startingPoints[0].gameObject.name);
        placements.Add(Player.Instance.PlayerPlacement);

        var avialableCars = PlayerCarSpawner.Instance.GetAvailableCarsWithAI();
        for (int i = 1; i < startingPoints.Length; i++)
        {
            var carWithAI = Instantiate(avialableCars[i - 1]);
            carWithAI.transform.position = new Vector3(startingPoints[i].position.x, startingPoints[i].position.y + 0.7f, startingPoints[i].position.z);
            placements.Add(carWithAI.GetComponent<Placement>());
        }
    }

    public void GoThroughCheckPoint(CheckPointSingle checkPointSingle)
    {
        int currentCheckpoitn = CheckPointSingleList.IndexOf(checkPointSingle);
        if (currentCheckpoitn == nextCheckpointSingleIndex)
        {
            if (checkPointSingle.IsGoalCheckPoint)
            {
                laps++;
                HUD.Instance?.UpdateLapCount($"{laps}/{lapTofinish}");

                if(laps == 3) EndGameScreen.Instance.EndGame();
            }

            // if (nextCheckpointSingleIndex > CheckPointSingleList.Count) nextCheckpointSingleIndex = 0;
            // else nextCheckpointSingleIndex++;
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % CheckPointSingleList.Count;

            CheckPointSingleList[currentCheckpoitn].gameObject.SetActive(false);
            CheckPointSingleList[nextCheckpointSingleIndex].gameObject.SetActive(true);
            Player.Instance.PlayerPlacement.PassACheckPoint();
            // Debug.Log("CORRECT");
        }
    }

    // private void OnGameRestart(object sender, EventArgs e)
    // {
    //     laps = 0;
    //     nextCheckpointSingleIndex = 0;
    //     foreach (CheckPointSingle checkPointSingle in CheckPointSingleList)
    //     {
    //         if (CheckPointSingleList.IndexOf(checkPointSingle) != nextCheckpointSingleIndex) checkPointSingle.gameObject.SetActive(false);
    //         else checkPointSingle.gameObject.SetActive(true);
    //     }
    // }

    public T[] ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(1, array.Length);

            T temp = array[randomIndex];
            array[randomIndex] = array[0];
            array[0] = temp;
        }
        return array;
    }

    public int UpdatePlayerPlacement()
    {
        placements = placements.OrderByDescending(p => p.NumberOfCheckPointPassed).ToList();
        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].IsPlayer)
            {
                HUD.Instance.UpdatePlacementText(i + 1);
                return i + 1;
            }
        }
        return -1;
    }

    public CheckPointSingle GetClosestCheckPoint(Vector3 position)
    {
        CheckPointSingle closestPoint = null;
        for (int i = 0; i < CheckPointSingleList.Count; i++)
        {
            if (closestPoint == null ||
                Vector3.Distance(position, CheckPointSingleList[i].transform.position) <
                Vector3.Distance(position, closestPoint.transform.position))
            {
                closestPoint = CheckPointSingleList[i];
            }
        }
        return closestPoint;
    }
}
