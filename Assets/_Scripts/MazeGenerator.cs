using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _mazeSize;
    [SerializeField] private Node _nodePrefab;



    private void Start()
    {
        StartCoroutine(GenerateMazeWithDelay());
    }

    private void GenerateMaze()
    {
        List<Node> nodes = new List<Node>();

        for (int x = 0; x < _mazeSize.x; x++)
        {
            for (int y = 0; y < _mazeSize.y; y++)
            {
                Vector3 nodePos = new Vector3(x - _mazeSize.x, 0, y - _mazeSize.y);

                Node node = Instantiate(_nodePrefab, nodePos, Quaternion.identity, transform);

                nodes.Add(node);
            }
        }
    }


    private IEnumerator GenerateMazeWithDelay()
    {
        Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();

        for (int x = 0; x < _mazeSize.x; x++)
        {
            for (int y = 0; y < _mazeSize.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (_mazeSize.x / 2), 0, y - (_mazeSize.y / 2));

                Node node = Instantiate(_nodePrefab, nodePos, Quaternion.identity, transform);

                node.NodeIndex.x = x;
                node.NodeIndex.y = y;
                nodes.Add(new Vector2Int(node.NodeIndex.x, node.NodeIndex.y), node);

                yield return new WaitForSeconds(0.1f);

            }
        }


        Stack<Node> pathStack = new Stack<Node>();
        Node currentNode = nodes[new Vector2Int(Random.Range(0, _mazeSize.x), Random.Range(0, _mazeSize.y))];
        currentNode.SetState(NodeState.Current);
        pathStack.Push(currentNode);

        while (pathStack.Count > 0)
        {
            List<int> neigborsList = GetUnvisitedNeigbors(currentNode, nodes);

            if (neigborsList.Count == 0)
            {
                pathStack.Pop();
                currentNode.SetState(NodeState.Complete);

                if(pathStack.Count > 0)
                    currentNode = pathStack.Peek();
                continue;
            }

            int neigborsCount = neigborsList.Count;
            int randomNeigborIndex = Random.Range(0, neigborsCount);

            Node newCurrentNode = SelectNextNode(currentNode, nodes, neigborsList[randomNeigborIndex]);

            pathStack.Push(newCurrentNode);
            newCurrentNode.SetState(NodeState.Current);
            currentNode.SetState(NodeState.Complete);
            currentNode = newCurrentNode;

            yield return new WaitForSeconds(0.1f);

        }

    }


    private List<int> GetUnvisitedNeigbors(Node currentNode, Dictionary<Vector2Int, Node> nodes)
    {
        //List<Node> neigbors = new List<Node>();
        //List<int> neigbourInt = new List<int>();
        List<int> neiborsList = new List<int>();

        Vector2Int[] directions =
        {
            new Vector2Int (0,1),
            new Vector2Int (1,0),
            new Vector2Int (0,-1),
            new Vector2Int (-1,0),
        };

        //foreach (var dir in directions) 
        //{
        //   Node neigbor = new Node();
        //    neigbor.NodeIndex = new Vector2Int(currentNode.NodeIndex.x + dir.x, currentNode.NodeIndex.y + dir.y);

        //    if (neigbor.NodeIndex.x >= 0 && neigbor.NodeIndex.x < _mazeSize.x &&
        //       neigbor.NodeIndex.y >= 0 && neigbor.NodeIndex.y < _mazeSize.y &&
        //       neigbor.GetState() == NodeState.Avaliable) 
        //    {
        //        neigbors.Add(neigbor);
        //    }
        //}


        for (int i = 0; i < directions.Length; i++)
        {
            if (currentNode.NodeIndex.x + directions[i].x >= 0 && currentNode.NodeIndex.x + directions[i].x < _mazeSize.x &&
               currentNode.NodeIndex.y + directions[i].y >= 0 && currentNode.NodeIndex.y + directions[i].y < _mazeSize.y)
            {
                Node neigbor = nodes[new Vector2Int(currentNode.NodeIndex.x + directions[i].x, currentNode.NodeIndex.y + directions[i].y)];
                neigbor.NodeIndex = new Vector2Int(currentNode.NodeIndex.x + directions[i].x, currentNode.NodeIndex.y + directions[i].y);

                if (neigbor.GetState() == NodeState.Avaliable)
                    neiborsList.Add(i);
            }
        }

        return neiborsList;
    }


    private Node SelectNextNode(Node currentNode, Dictionary<Vector2Int, Node> nodes, int randomNeigbourIndex) 
    {

        switch (randomNeigbourIndex) 
        {
            case 0:
                
                    currentNode.RemoveWall(0);
                nodes[new Vector2Int(currentNode.NodeIndex.x + 0, currentNode.NodeIndex.y + 1)].RemoveWall(2);

                return nodes[new Vector2Int(currentNode.NodeIndex.x + 0, currentNode.NodeIndex.y + 1)];
            case 1:            
                    currentNode.RemoveWall(1);
                    nodes[new Vector2Int(currentNode.NodeIndex.x + 1, currentNode.NodeIndex.y + 0)].RemoveWall(3);

                return nodes[new Vector2Int(currentNode.NodeIndex.x + 1, currentNode.NodeIndex.y + 0)];
            case 2:       
                    currentNode.RemoveWall(2);
                    nodes[new Vector2Int(currentNode.NodeIndex.x + 0, currentNode.NodeIndex.y -1)].RemoveWall(0);

                return nodes[new Vector2Int(currentNode.NodeIndex.x + 0, currentNode.NodeIndex.y - 1)];
            case 3:  
                    currentNode.RemoveWall(3);
                nodes[new Vector2Int(currentNode.NodeIndex.x -1, currentNode.NodeIndex.y + 0)].RemoveWall(1);

                return nodes[new Vector2Int(currentNode.NodeIndex.x - 1, currentNode.NodeIndex.y + 0)];
        }

        return currentNode;
    }
}
