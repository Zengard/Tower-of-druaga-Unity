using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] _walls;
    [SerializeField] private MeshRenderer _meshRenderer;
    public Vector2Int NodeIndex;
    private NodeState _nodeState;


    private void Start()
    {
        _nodeState = NodeState.Avaliable;
    }

    public void RemoveWall(int index) 
    {
        _walls[index].gameObject.SetActive(false);
    }

    public void SetState(NodeState state)
    {
        _nodeState = state;

        switch (state)
        {
            case NodeState.Avaliable:
                _meshRenderer.material.color = Color.white;
                break;
            case NodeState.Current:
                _meshRenderer.material.color = Color.yellow;
                break;
            case NodeState.Complete:
                _meshRenderer.material.color = Color.blue;
                break;
        }
    }

    public NodeState GetState() { return _nodeState; }
}

public enum NodeState 
{
    Avaliable,
    Current,
    Complete
}
