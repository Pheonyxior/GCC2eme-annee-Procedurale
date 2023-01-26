using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorBehaviour : GenericFactory<RoomBehaviour>
{
    List<string> keyDataList = new List<string> 
    {
        "TOP-WALL", "TOP-ENTRY",
        "RIGHT-WALL", "RIGHT-ENTRY",
        "BOTTOM-WALL", "BOTTOM-ENTRY",
        "LEFT-WALL" , "LEFT-ENTRY"
    };
    List<string> wallDataList = new List<string> 
    {
        "TOP-WALL", 
        "RIGHT-WALL", 
        "BOTTOM-WALL", 
        "LEFT-WALL" 
    };
    List<string> entryDataList = new List<string> 
    {
        "TOP-ENTRY",
        "RIGHT-ENTRY",
        "BOTTOM-ENTRY",
        "LEFT-ENTRY"
    };

    [SerializeField] private Vector2[] directions = new Vector2[] 
    {
        new Vector2(0,1),
        new Vector2(1,0),
        new Vector2(0,-1),
        new Vector2(-1,0)
    };

    
    
    [SerializeField] private float roomOffset;
    [SerializeField] private int generationCycleNumber;

    Vector2[,] tileGrid;
	RoomBehaviour[,] roomGrid;

    

    private void Awake()
    {
        generateDungeon(GenerateRoom(new Vector3(generationCycleNumber * roomOffset, 0f, generationCycleNumber * roomOffset), ""), generationCycleNumber);

    


        // generateRoom(Vector3.zero, "");
        // generateRoom(Vector3.forward*10, "");
        // generateRoom(Vector3.back*10, "");
        
        
    }
   

    // private void generateDungeon(RoomBehaviour startingRoom, int cycles)
    // {
    //     List<RoomBehaviour> reached = new List<RoomBehaviour>();
    //     reached.Add(startingRoom);

    //     List<List<RoomBehaviour>> fringes = new List<List<RoomBehaviour>>();
    //     fringes.Add(new List<RoomBehaviour>());

    //     fringes[0].Add(startingRoom);

    //     for(int currentCycle = 1; currentCycle < cycles; currentCycle++)
    //     {
    //         fringes.Add(new List<RoomBehaviour>());

    //         foreach(RoomBehaviour room in fringes[currentCycle-1])
    //         {
    //             for(int dir = 0; dir < directions.Length-1; dir++)
    //             {
    //                 RoomBehaviour neighbor = generateRoom(room.transform.position + directions[dir], "");
    //                 fringes[currentCycle].Add(neighbor);
    //             }
    //         }
    //     }

    private void generateDungeon(RoomBehaviour startingRoom, int cycles)
    {
        tileGrid = new Vector2[(cycles*2)+1, (cycles*2)+1]; // créer un array de vector 2 assez grand pour stocker tout les emplacements de rooms
		roomGrid = new RoomBehaviour[(cycles*2)+1, (cycles*2)+1];

        // List<RoomBehaviour> reached = new List<RoomBehaviour>();
        // reached.Add(startingRoom);

        List<List<RoomBehaviour>> fringes = new List<List<RoomBehaviour>>();
        fringes.Add(new List<RoomBehaviour>());

        fringes[0].Add(startingRoom);
        roomGrid[cycles, cycles] = startingRoom;
        startingRoom.SetTile(new Vector2(cycles, cycles));

        for(int currentCycle = 1; currentCycle < cycles; currentCycle++) // Pour chaque cycles de générations
        {
            fringes.Add(new List<RoomBehaviour>());

            foreach(RoomBehaviour currentRoom in fringes[currentCycle-1]) // Pour chaque room instanciés dans le cycle précédent
            {
				foreach(Vector2 tile in AdgacentTiles(currentRoom.GetTile())) // Pour chaque tile autour de room 
				{
                    //Debug.Log(currentRoom.GetTile());
                    if (RoomOnTile(tile) != null) // s'il y a déjà une room sur la tile que l'on visite
                    {
                        continue;
                    }

					string data = "";
					
					for(int dir = 0; dir < directions.Length-1; dir++) // Pour chaque directions autour de la tile que l'on visite
					{
						Vector2 neighborTile = new Vector2(tile.x + directions[dir].x, tile.y + directions[dir].y);
						// On vérifie s'il y a déjà une room voisine en regardant la position de la tile(tile.x, tile.y) + la direction vers laquelle on regarde(directions[dir]...)
						//Debug.Log(neighborTile);
                        RoomBehaviour neighborRoom = RoomOnTile(neighborTile);
						
						if (neighborRoom != null)
						{
							if(neighborRoom.HasOppositeWall(directions[dir]))
							{
								data += wallDataList[dir] + ' ';
							}
							else 
							{
								data += entryDataList[dir] + ' ';
							}
						}
					}

                    Debug.Log(tile.x);
                    Debug.Log(tile.y);
					
                    RoomBehaviour newRoom = GenerateRoom(new Vector3(tile.x * roomOffset, 0f, tile.y * roomOffset), data);
					fringes[currentCycle].Add(newRoom);
                    newRoom.SetTile(new Vector2(tile.x, tile.y));
                    roomGrid[(int)tile.x, (int)tile.y] = newRoom;
                    
				}
            }
        }
    }


    private RoomBehaviour GenerateRoom(Vector3 position, string data)
    {
        RoomBehaviour room = GetNewInstance();
        room.transform.position = position;
        room._setupWalls(getRelevantData(data));
        Debug.Log(data);
        return room;
    }

    private Vector2[] AdgacentTiles(Vector2 tile)
    {
        Vector2[] adgacentTiles = new Vector2[]
        {
            // tileGrid[(int)tile.x, (int)tile.y + 1],
            // tileGrid[(int)tile.x + 1, (int)tile.y],
            // tileGrid[(int)tile.x, (int)tile.y - 1],
            // tileGrid[(int)tile.x - 1, (int)tile.y]
            new Vector2(tile.x, tile.y+1),
            new Vector2(tile.x+1, tile.y),
            new Vector2(tile.x, tile.y-1),
            new Vector2(tile.x-1, tile.y),
        };

        Debug.Log(tile.x);
        Debug.Log(tile.y);

        Debug.Log(tileGrid[(int)tile.x, (int)tile.y + 1]);


        return adgacentTiles;
    }

    private RoomBehaviour RoomOnTile(Vector2 tile)
    {
        return roomGrid[(int)tile.x,(int)tile.y];
    }


    private List<string> getRelevantData(string data)
    {
        List<string> dataList = new List<string>();
        string[] dataStringArray = data.Split(' ');
        foreach (string dataString in dataStringArray)
        {
            if (keyDataList.Contains(dataString))
            {
                dataList.Add(dataString);
                //Debug.Log(dataString);
            }
        }

        return dataList;

    }

    private void debugDataList(List<string> dataList)
    {
        foreach (string str in dataList)
        {
            //Debug.Log(str);
        }
    }

    
}
