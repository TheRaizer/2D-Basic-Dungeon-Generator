using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsGenerator : MonoBehaviour
{
    [SerializeField] public GameObject closingRoomUp;
    [SerializeField] public GameObject closingRoomDown;
    [SerializeField] public GameObject closingRoomLeft;
    [SerializeField] public GameObject closingRoomRight;

    [SerializeField] public GameObject[] leftRooms;
    [SerializeField] public GameObject[] rightRooms;
    [SerializeField] public GameObject[] upRooms;
    [SerializeField] public GameObject[] downRooms;

    [field: SerializeField] public int MaxNumberOfRooms { get; private set; }

    [field: SerializeField] public int CurrentNumberOfRooms { get; set; }

    public bool[,] RoomPlacements { get; set; }

    public int length;
    public const float DISTANCE_BETWEEN_ROOMS = 6.042f;

    public Dictionary<RoomTypes, List<GameObject>> openings = new Dictionary<RoomTypes, List<GameObject>>();

    private void Awake()
    {
        length = (int)Mathf.Sqrt(MaxNumberOfRooms);
        RoomPlacements = new bool[length, length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AddToOpenings(RoomTypes type, GameObject room)
    {
        if (!openings.ContainsKey(type))
        {
            openings.Add(type, new List<GameObject>());
            openings[type].Add(room);
        }
        else
        {
            openings[type].Add(room);
        }
    }
}
