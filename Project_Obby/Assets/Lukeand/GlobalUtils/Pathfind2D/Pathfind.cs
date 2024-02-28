using MyBox;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfind : MonoBehaviour
{
    //i want to create a pathing here to use everyelse.
    //this is to be used in 2d tilemap. so basically we make a false tilemap to get the values.


    //i need to create the nodes based on the tilemap.
    //

    // The grid of nodes representing the game world
    //public MyNode[,] gridArray;
    Dictionary<Vector2Int, MyNode> map = new Dictionary<Vector2Int, MyNode>();
    [SerializeField] Tilemap pathableTileMap;
    Transform pathableContainer;

    [Separator("DEBUGG")]
    [SerializeField] Transform DEBUGGSTART;
    [SerializeField] Transform DEBUGGEND;
    List<MyNode> DEBUGGPATH;
    private void Awake()
    {
        pathableContainer = transform.GetChild(0).transform;
        CreateGridFromTileMap(pathableTileMap);
        return;
        if(DEBUGGSTART != null && DEBUGGEND != null)
        {
            DEBUGGPATH = GetPathThroughTransform(DEBUGGSTART, DEBUGGEND);
        }
    }



    public void CreateGridFromTileMap(Tilemap tileMap)
    {
        //we use this tilemap to create a path. only that

        tileMap.CompressBounds();
        BoundsInt bounds = tileMap.cellBounds;
        MyNode nodeTemplate = new GameObject().AddComponent<MyNode>();
        //gridArray = new MyNode[bounds.size.x, bounds.size.y];
        //we create a dictionary of all possible paths.

        //we create this thing to tell the thing based in a grid inside a dictionary.

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
                var tileKey = new Vector2Int(x, y);

                if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                {                   
                    var overlayTile = Instantiate(nodeTemplate, pathableContainer.transform.position, Quaternion.identity);
                    overlayTile.transform.parent = pathableContainer.transform;
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);

                    //gridArray[x - bounds.min.x, y - bounds.min.y] = overlayTile;
                    //overlayTile.gridLocation = tileLocation;
                    map.Add(tileKey, overlayTile);
                }
            }
        }


        //then we add the neighboors here.
        AddNeighbors(tileMap);


    }

    public void AddNeighbors(Tilemap tileMap)
    {
        tileMap.CompressBounds();
        BoundsInt bounds = tileMap.cellBounds;
        int current = 0;


        //maybe i should use the other things. this simplified thing isnt working.


        for (int i = 0; i < pathableContainer.transform.childCount; i++)
        {
            int x = (int)pathableContainer.transform.GetChild(i).transform.position.x;
            int y = (int)pathableContainer.transform.GetChild(i).transform.position.y;
            if (!map.ContainsKey(new Vector2Int(x, y)))
            {
                Debug.LogError("DOES NOT HAVE MAP " + new Vector2Int(x, y));
                continue;
            }
            MyNode node = map[new Vector2Int(x, y)];
            current++;
            Debug.Log("yo");
            // Check the nodes above, below, to the left, and to the right of this node
            AddOneNeighboor(node, new Vector2Int(x, y));

        }


        for (int y = 0; y < bounds.size.y; y++)
        {
            //we go after each to tell who their neighboors are.
            for (int x = 0; x < bounds.size.x; x++)
            {
                
               

            }
        }

    }

    void AddOneNeighboor(MyNode node, Vector2Int baseVector)
    {

        Vector2Int up = baseVector + Vector2Int.up;
        Vector2Int down = baseVector + Vector2Int.down;
        Vector2Int right = baseVector + Vector2Int.right;
        Vector2Int left = baseVector + Vector2Int.left;

        if (map.ContainsKey(up)) if (!map[up].isBlocked) node.AddNeighbor(map[up]); // top neighbor
        if (map.ContainsKey(down)) if (!map[down].isBlocked) node.AddNeighbor(map[down]);
        if (map.ContainsKey(right)) if (!map[right].isBlocked) node.AddNeighbor(map[right]);
        if (map.ContainsKey(left)) if (!map[left].isBlocked) node.AddNeighbor(map[left]);

    }


    public List<MyNode> GetPathThroughTransform(Transform start, Transform end)
    {
        Vector3Int firstStart = pathableTileMap.WorldToCell(start.position);
        Vector2Int newStart = new Vector2Int(firstStart.x, firstStart.y);

        Vector3Int firstEnd = pathableTileMap.WorldToCell(end.position);
        Vector2Int newEnd = new Vector2Int(firstEnd.x, firstEnd.y);

        return GetPath(newStart, newEnd);
    }

    public List<MyNode> GetPath(Vector2Int start, Vector2Int end)
    {

        //get path between the start and the end.

        //we make the transform into an actual information.

        MyNode startNode = map.ContainsKey(end) ? map[start]:null;
        MyNode endNode = map.ContainsKey(end)?map[end]:null;

        if(endNode == null || startNode == null)
        {
            Debug.LogError("SOMETHING WRONG IN START NODE OR ENDNODE");
            return null;
        }

        HashSet<MyNode> openSet = new();
        HashSet<MyNode> closedSet = new();
        List<MyNode> path = new();
        

        openSet.Add(startNode);

        int count = 0;
        int brake = 0;
        while(openSet.Count > 0)
        {
            count++;

            brake++;

            if(brake > 10000)
            {
                break;
            }

            Debug.Log("initial count " + count);

            MyNode currentNode = null;

            foreach (MyNode node in openSet)
            {
                if (currentNode == null || node.f < currentNode.f)
                {
                    currentNode = node;
                }
            }

            if (currentNode == endNode)
            {
                MyNode node = currentNode;
                while (node != startNode)
                {
                    path.Add(node);
                    node = node.cameFrom;
                }
                path.Reverse();
                continue;
            }


            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //we check every neighboor of the current list.
            foreach (MyNode neighbor in currentNode.neighborsList)
            {
                // Skip nodes that have already been evaluated or are blocked
                if (closedSet.Contains(neighbor) || neighbor.isBlocked)
                {
                    continue;
                }

                // Calculate the tentative G score (the distance from the start node to the neighbor through the current node)
                float tentativeGScore = currentNode.g + Heuristic(currentNode, neighbor);

                // Add the neighbor to the open set if it is not already there
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= neighbor.g)
                {
                    // If the tentative G score is greater than or equal to the neighbor's current G score, skip this neighbor
                    continue;
                }

                neighbor.cameFrom = currentNode;
                //neighbor.g = tentativeGScore;
                


            }

        }


        return path;
    }

    public void OnDrawGizmos()
    {
        if (DEBUGGPATH != null) 
        {
            Debug.Log("start debugg");
            for (int i = 0; i < DEBUGGPATH.Count; i++)
            {
                Gizmos.color = Color.red;
                if (i + 1 >= DEBUGGPATH.Count) continue;
                Gizmos.DrawLine(DEBUGGPATH[i].transform.position, DEBUGGPATH[i + 1].transform.position);
            }
            Debug.Log("drew everything");
            
        }
    }

    

    public float Heuristic(MyNode first, MyNode second)
    {
        return Vector2.Distance(first.transform.position, second.transform.position);
    }

    //any fella who wants to move will receive a list of nodes and simply walk through them
    //however, the fella needs to be able to see when 

}
