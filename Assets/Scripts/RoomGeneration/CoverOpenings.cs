using System.Collections;
using UnityEngine;

public class CoverOpenings : MonoBehaviour
{
    RoomsGenerator roomGenerator;

    [SerializeField] private float waitTime;

    private WaitForSeconds timeTillCover;

    private bool choseEnd;


    private void Awake()
    {
        timeTillCover = new WaitForSeconds(waitTime);
        roomGenerator = FindObjectOfType<RoomsGenerator>();
    }

    private void Start()
    {
        StartCoroutine(CoverCo());
    }

    private IEnumerator CoverCo()
    {
        yield return timeTillCover;
        Cover();
    }

    private void Cover()
    {
        GameObject roomGameObject = null;
        if (roomGenerator.openings.ContainsKey(RoomTypes.up))
        {
            foreach (GameObject room in roomGenerator.openings[RoomTypes.up]) // for each room with an opening above it
            {
                //generate the cover with a opening below it and place it above the room with the opening.
                roomGameObject = Instantiate(roomGenerator.closingRoomDown,
                            new Vector3(room.transform.position.x, room.transform.position.y + RoomsGenerator.DISTANCE_BETWEEN_ROOMS,
                            room.transform.position.z),
                            Quaternion.identity);

                ChooseEnd(roomGameObject);
            }
        }
        if (roomGenerator.openings.ContainsKey(RoomTypes.down))
        {
            foreach (GameObject room in roomGenerator.openings[RoomTypes.down])
            {
                roomGameObject = Instantiate(roomGenerator.closingRoomUp,
                            new Vector3(room.transform.position.x, room.transform.position.y - RoomsGenerator.DISTANCE_BETWEEN_ROOMS,
                            room.transform.position.z),
                            Quaternion.identity);

                ChooseEnd(roomGameObject);
            }
        }

        if (roomGenerator.openings.ContainsKey(RoomTypes.left))
        {
            foreach (GameObject room in roomGenerator.openings[RoomTypes.left])
            {
                roomGameObject = Instantiate(roomGenerator.closingRoomRight,
                            new Vector3(room.transform.position.x - RoomsGenerator.DISTANCE_BETWEEN_ROOMS,
                            room.transform.position.y, room.transform.position.z),
                            Quaternion.identity);

                ChooseEnd(roomGameObject);
            }
        }
        if (roomGenerator.openings.ContainsKey(RoomTypes.right))
        {
            foreach (GameObject room in roomGenerator.openings[RoomTypes.right])
            {
                roomGameObject = Instantiate(roomGenerator.closingRoomLeft,
                            new Vector3(room.transform.position.x + RoomsGenerator.DISTANCE_BETWEEN_ROOMS,
                            room.transform.position.y, room.transform.position.z),
                            Quaternion.identity);

                ChooseEnd(roomGameObject);
            }
        }

        if (!choseEnd)
        {
            roomGameObject.GetComponent<Room>().isEnd = true;
        }
    }

    private void ChooseEnd(GameObject roomGameObject)
    {
        if (!choseEnd)
        {
            Room room = roomGameObject.GetComponent<Room>();
            room.isEnd = Random.Range(0, 2) == 0;
            if (room.isEnd)
                choseEnd = true;
        }
    }
}
