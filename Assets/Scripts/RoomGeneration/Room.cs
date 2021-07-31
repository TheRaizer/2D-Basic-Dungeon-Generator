using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private bool[] openings;// index 0 = up | index 1 = down | index 2 = left | index 3 = right

    private RoomsGenerator roomGenerator;

    public int x;
    public int y;

    [SerializeField] private bool isStart;
    public bool isEnd;

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomsGenerator>();
        if (isStart)
        {
            x = roomGenerator.length / 2;
            y = roomGenerator.length / 2;
            roomGenerator.RoomPlacements[x, y] = true;
        }


        if (roomGenerator.CurrentNumberOfRooms < roomGenerator.MaxNumberOfRooms)
        {
            GenerateRoomsAtOpenings();
        }
    }

    private void GenerateRoomsAtOpenings()
    {
        for (int roomType = 0; roomType < openings.Length; roomType++)
        {
            if (openings[roomType] == false)
            {
                continue;
            }

            GameObject roomGameObject;
            int randomRoomIndex;

            switch ((RoomTypes)roomType)
            {
                case RoomTypes.up:
                    if (y + 1 < roomGenerator.length - 1) // if the room we want to generate will be in the grid
                    {
                        if (roomGenerator.RoomPlacements[x, y + 1] == false) // if the place on the grid is empty meaning no room is there
                        {
                            randomRoomIndex = Random.Range(0, roomGenerator.downRooms.Length); // pick on of the rooms that has an opening below it

                            //generate the room in the game
                            roomGameObject = Instantiate(roomGenerator.downRooms[randomRoomIndex],
                                new Vector3(transform.position.x, transform.position.y + RoomsGenerator.DISTANCE_BETWEEN_ROOMS, transform.position.z),
                                Quaternion.identity);

                            InitializeRoom(roomGameObject, x, y + 1);
                        }
                    }
                    else
                    {
                        ///<summary>
                        /// If we are trying to generate a room that cannot fit within the room grid
                        /// then that means that this current room is at the edge meaning it is leaving
                        /// an opening that we must close. So we will tally the opening it creates.
                        /// In this case it creates a opening above it.
                        ///</summary>
                        roomGenerator.AddToOpenings((RoomTypes)roomType, gameObject);
                    }
                    break;

                case RoomTypes.down:
                    if (y - 1 >= 0)
                    {
                        if (roomGenerator.RoomPlacements[x, y - 1] == false)
                        {
                            randomRoomIndex = Random.Range(0, roomGenerator.upRooms.Length);
                            roomGameObject = Instantiate(roomGenerator.upRooms[randomRoomIndex],
                                new Vector3(transform.position.x, transform.position.y - RoomsGenerator.DISTANCE_BETWEEN_ROOMS, transform.position.z),
                                Quaternion.identity);
                            InitializeRoom(roomGameObject, x, y - 1);
                        }
                    }
                    else
                    {
                        roomGenerator.AddToOpenings((RoomTypes)roomType, gameObject);
                    }
                    break;

                case RoomTypes.left:
                    if (x - 1 >= 0)
                    {
                        if (roomGenerator.RoomPlacements[x - 1, y] == false)
                        {
                            randomRoomIndex = Random.Range(0, roomGenerator.rightRooms.Length);
                            roomGameObject = Instantiate(roomGenerator.rightRooms[randomRoomIndex],
                                new Vector3(transform.position.x - RoomsGenerator.DISTANCE_BETWEEN_ROOMS, transform.position.y, transform.position.z),
                                Quaternion.identity);
                            InitializeRoom(roomGameObject, x - 1, y);
                        }
                    }
                    else
                    {
                        roomGenerator.AddToOpenings((RoomTypes)roomType, gameObject);
                    }
                    break;

                case RoomTypes.right:
                    if (x + 1 < roomGenerator.length - 1)
                    {
                        if (roomGenerator.RoomPlacements[x + 1, y] == false)
                        {
                            randomRoomIndex = Random.Range(0, roomGenerator.leftRooms.Length);
                            roomGameObject = Instantiate(roomGenerator.leftRooms[randomRoomIndex],
                                new Vector3(transform.position.x + RoomsGenerator.DISTANCE_BETWEEN_ROOMS, transform.position.y, transform.position.z),
                                Quaternion.identity);
                            InitializeRoom(roomGameObject, x + 1, y);
                        }
                    }
                    else
                    {
                        roomGenerator.AddToOpenings((RoomTypes)roomType, gameObject);
                    }
                    break;
            }
        }
    }

    private void InitializeRoom(GameObject room, int x, int y)
    {
        Room roomObject = room.GetComponent<Room>();

        roomObject.x = x;
        roomObject.y = y;
        roomGenerator.RoomPlacements[x, y] = true;
        roomGenerator.CurrentNumberOfRooms++;
    }
}
