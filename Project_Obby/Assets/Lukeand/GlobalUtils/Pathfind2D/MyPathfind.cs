
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MyPathfind : MonoBehaviour
{
    public static MyPathfind instance;

    //whats the ifference between the two.

    //maybe its better to not be diagonal because of the animation.


    public MyNode[,] gridArray;
    List<MyNode> nodeList = new();
    Dictionary<Vector2Int, MyNode> map = new Dictionary<Vector2Int, MyNode>();



    private void Awake()
    {
        instance = this;
        CreateGrid();
    }


    [SerializeField] UnityEngine.Transform pathableContainer;
    [SerializeField] Tilemap pathableTileMap;
    [SerializeField] Tilemap kitchenTileMap;
    [SerializeField] Tilemap clientTileMap;

    #region GRID CREATION
    public void CreateGrid()
    {
        pathableTileMap.CompressBounds();
        BoundsInt bounds = pathableTileMap.cellBounds;
        MyNode nodeTemplate = new GameObject().AddComponent<MyNode>();
        gridArray = new MyNode[bounds.size.x, bounds.size.y];


        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {

                var tileLocation = new Vector3Int(x, y, 0);
                var tileKey = new Vector2Int(x, y);

                if (pathableTileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                {
                    MyNode newNode = CreateNode(nodeTemplate, tileLocation, tileKey);
                    gridArray[x - bounds.min.x, y - bounds.min.y] = newNode;

                }
            }
        }

        //when this is over then we call the connect nodes.
        ConnectNodes();
    }
    MyNode CreateNode(MyNode nodeTemplate, Vector3Int tileLocation, Vector2Int tileKey)
    {
       
        MyNode newNode = Instantiate(nodeTemplate, pathableContainer.transform.position, Quaternion.identity);
        newNode.transform.parent = pathableContainer.transform;
        var cellWorldPosition = pathableTileMap.GetCellCenterWorld(tileLocation);
        newNode.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);
        newNode.SetUpPos(newNode.transform.position);
        newNode.gridLocation = tileKey;

        newNode.name = tileKey.ToString();
        nodeList.Add(newNode);
        map.Add(tileKey, newNode);
        return newNode;
    }

    void ConnectNodes()
    {
        //we create the neighboors here.
        for (int i = 0; i < nodeList.Count; i++)
        {
            ConnectSingleNode(nodeList[i]);
        }
    }

    void ConnectSingleNode(MyNode node)
    {
        //connect one especifc node to every fella around
        Vector2Int up = node.gridLocation + Vector2Int.up;
        Vector2Int down = node.gridLocation + Vector2Int.down;
        Vector2Int right = node.gridLocation + Vector2Int.right;
        Vector2Int left = node.gridLocation + Vector2Int.left;

        if (map.ContainsKey(up)) if (!map[up].isBlocked) node.AddNeighbor(map[up]); // top neighbor
        if (map.ContainsKey(down)) if (!map[down].isBlocked) node.AddNeighbor(map[down]);
        if (map.ContainsKey(right)) if (!map[right].isBlocked) node.AddNeighbor(map[right]);
        if (map.ContainsKey(left)) if (!map[left].isBlocked) node.AddNeighbor(map[left]);

        return;
        Vector2Int upCornerLeft = node.gridLocation + Vector2Int.up + Vector2Int.left;
        Vector2Int upCornerRight = node.gridLocation + Vector2Int.up + Vector2Int.right;
        Vector2Int downCornerLeft = node.gridLocation + Vector2Int.down + Vector2Int.left;
        Vector2Int downCornerRight = node.gridLocation + Vector2Int.down + Vector2Int.right;

        if (map.ContainsKey(upCornerLeft)) if (!map[upCornerLeft].isBlocked) node.AddNeighbor(map[upCornerLeft]); 
        if (map.ContainsKey(upCornerRight)) if (!map[upCornerRight].isBlocked) node.AddNeighbor(map[upCornerRight]);
        if (map.ContainsKey(downCornerLeft)) if (!map[downCornerLeft].isBlocked) node.AddNeighbor(map[downCornerLeft]);
        if (map.ContainsKey(downCornerRight)) if (!map[downCornerRight].isBlocked) node.AddNeighbor(map[downCornerRight]);

    }
    #endregion

    #region GETPATH

    //problems - i want to know the last node.
    //its not over when 
    public List<MyNode> GetPathThroughVector(Vector3 start, Vector3 end)
    {
        Vector3Int firstStart = pathableTileMap.WorldToCell(start);
        Vector2Int newStart = new Vector2Int(firstStart.x, firstStart.y);

        Vector3Int firstEnd = pathableTileMap.WorldToCell(end);
        Vector2Int newEnd = new Vector2Int(firstEnd.x, firstEnd.y);

        return GetPath(newStart, newEnd);
    }

   


    public List<MyNode> GetPath(Vector2Int start, Vector2Int end)
    {

        MyNode startNode = map.ContainsKey(start) ? map[start] : null;
        MyNode endNode = map.ContainsKey(end) ? map[end] : null;
      
        if (endNode == null || startNode == null)
        {
            if (endNode == null) Debug.LogError("END NODE WRONG " + gameObject.name);
            if (startNode == null) Debug.LogError("START NODE WRONG " + gameObject.name);
            return null;
        }

        List<MyNode> openList = new();
        HashSet<MyNode> closedSet = new();
        List<MyNode> path = new();

        openList.Add(startNode);

        int brake = 0;

        //set up all nodes for the pathing
        for (int i = 0; i < nodeList.Count; i++)
        {
            //calculate the pathing.
            nodeList[i].g = int.MaxValue;
            nodeList[i].cameFrom = null;
        }

        startNode.g = 0;
        startNode.h = CalculateDistance(startNode, endNode);
        
        while(openList.Count > 0)
        {
            MyNode currentNode = GetLowestFCostNode(openList);

            brake++;

            if (brake > 1000)
            {
                Debug.Log("broke");
                break;
            }

            if (currentNode == endNode)
            {
                //finally the path.
                return RetracePath(startNode, endNode);
            }
 
            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            

            var neighboorTiles = currentNode.neighborsList;
            foreach (var neighboor in neighboorTiles)
            {
                if (neighboor.isBlocked || closedSet.Contains(neighboor)) continue;

                //but if it works in the 10 it can work in the 14.
                //find the nodes with the lowest f score.
                //f = g + h
                //g is easy. distance between the current node to the end node.
                //h is going to be the manhattan

                int tentativeGCost = currentNode.g + CalculateDistance(currentNode, neighboor);

                if(tentativeGCost < neighboor.g)
                {
                    neighboor.cameFrom = currentNode;
                    neighboor.g = tentativeGCost;
                    neighboor.h = CalculateDistance(neighboor, endNode);

                    if (!openList.Contains(neighboor)) openList.Add(neighboor);
                }            

            }

        }

        //out of nodes in the openlist.
        Debug.Log("out of nodes in the openlist");

        return null;
    }


   

    //ok i will try bymyself.
    //to find the pathi need to check the distance of each.
    //i will add the neighboors from the corners first.
    //then for each one i will check its distance to endnode and create a path by doing it.
    //improve it with time.

    List<MyNode> RetracePath(MyNode start, MyNode end)
    {
        //what i do here is bascially.
        //i get the came from from each node starting from the end.

        List<MyNode> path = new();
        MyNode currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.cameFrom;
        }

        path.Reverse();
        return path;


    }

    #endregion

    #region UTILS
    
    int CalculateDistance(MyNode start, MyNode end)
    {
        int xDistance = Mathf.Abs(start.gridLocation.x - end.gridLocation.x);
        int yDistance = Mathf.Abs(start.gridLocation.y - end.gridLocation.y);

        int remaining = Mathf.Abs(xDistance - yDistance);
        return 14 * Mathf.Min(xDistance, yDistance) + 10 * remaining;
    }

    MyNode GetLowestFCostNode(List<MyNode> nodeList)
    {
        MyNode lowestFCostNode = nodeList[0];

        for (int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].f < lowestFCostNode.f)
            {
                lowestFCostNode = nodeList[i];
            }
        }

        return lowestFCostNode;
    }

    public Vector3Int GetGridPos(Vector3 pos)
    {
        return pathableTileMap.WorldToCell(pos);


    }
  
    


    #endregion


    //we get things and then assign all th pathing.
    //also every house has a bunch

}


//i will create teh queue and the table with chair now.
//table and chair come first.
//spawn a client. a client will look for a free table if there is none
//the client sits in the chair.
