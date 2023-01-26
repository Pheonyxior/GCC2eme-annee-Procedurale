using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

    
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject openWall;
    [SerializeField] private GameObject closedWall;

    [SerializeField] private float wallPositionOffset = 3.8f;
    [SerializeField] private float roomPositionOffset = 9.1f;


    [SerializeField] private Vector3 northWallOffset;
    [SerializeField] private Vector3 eastWallOffset;
    [SerializeField] private Vector3 southWallOffset;
    [SerializeField] private Vector3 westWallOffset;


    [SerializeField] private float wallRotationOffset = 90f;


    //const private Vector3[] directionOffsets = new Vector3[] ;

    private Vector2 tile;

    //private static Random rand;

    private bool uClosed;
    private bool rClosed;
    private bool dClosed;
    private bool lClosed;


    private void Awake() 
    {
        
        
        
    }

   

    public void _setupWalls(List<string> data)
    {
        createFloor();
        if (data.Contains("TOP-WALL"))
        {
            createWall(closedWall, northWallOffset, wallRotationOffset*2);
            uClosed = true;
        }
        else if(data.Contains("TOP-ENTRY"))
        {
            createWall(openWall, northWallOffset, wallRotationOffset*2);
        }
        else
        {
            if (Randomize())
            {
                createWall(closedWall, northWallOffset, wallRotationOffset*2);
                uClosed = true;

            }
            else
            {
                createWall(openWall, northWallOffset, wallRotationOffset*2);
            }
        }


        if (data.Contains("RIGHT-WALL"))
        {
            createWall(closedWall, eastWallOffset, -wallRotationOffset);
            rClosed = true;
        }
        else if(data.Contains("RIGHT-ENTRY"))
        {
            createWall(openWall, eastWallOffset, -wallRotationOffset);
        }
        else
        {
            if (Randomize())
            {
                createWall(closedWall, eastWallOffset, -wallRotationOffset);
                rClosed = true;

            }
            else
            {
                createWall(openWall, eastWallOffset, -wallRotationOffset);
            }
        }


        if (data.Contains("BOTTOM-WALL"))
        {
            createWall(closedWall, southWallOffset, 0f);
            dClosed = true;
        }
        else if(data.Contains("BOTTOM-ENTRY"))
        {
            createWall(openWall, southWallOffset, 0f);
        }
        else
        {
            if (Randomize())
            {
                createWall(closedWall, southWallOffset, 0f);
                dClosed = true;

            }
            else
            {
                createWall(openWall, southWallOffset, 0f);
            }
        }


        if (data.Contains("LEFT-WALL"))
        {
            createWall(closedWall, westWallOffset, wallRotationOffset);
            lClosed = true;
        }
        else if(data.Contains("LEFT-ENTRY"))
        {
            createWall(openWall, westWallOffset, wallRotationOffset);
        }
        else
        {     
            if (Randomize())
            {
                createWall(closedWall, westWallOffset, wallRotationOffset);
                lClosed = true;

            }
            else
            {
                createWall(openWall, westWallOffset, wallRotationOffset);
            }
            
        }
    }

    private bool Randomize()
    {
        return (Random.Range(0,2) == 1);
        
    }

    public void _setupInterior(List<string> data)
    {
        

    }



    public bool HasOppositeWall(Vector2 otherOpposite) 
    {
        if (otherOpposite == Vector2.up)
        {
            if (dClosed) return true;
            
        }
        else if(otherOpposite == Vector2.right)
        {
            if (lClosed) return true;
        }
        else if(otherOpposite == Vector2.down)
        {
            if (uClosed) return true;
        }
        else if(otherOpposite == Vector2.left)
        {
            if (rClosed) return true;
        }

        return false;
    }

    public void SetTile(Vector2 newTile)
    {
        tile = newTile;
    }
    public Vector2 GetTile()
    {
        return tile;
    }





    private void createFloor()
    {
        Instantiate(floor, this.transform, false);
    }

    private void createWall(GameObject wall, Vector3 positionOffset, float rotation)
    {
        GameObject gOtoInstanciate = Instantiate(wall, this.transform, false);
        gOtoInstanciate.transform.position += positionOffset;
        gOtoInstanciate.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

    }


}
