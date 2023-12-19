using System.Collections.Generic;
using UnityEngine;

namespace CustomAstar
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] int _numOfRows;
        [SerializeField] int _numOfColumns;

        [SerializeField] float _gridCellSize;


        [SerializeField] bool _showGrid = true;

        [Header("Obstacles")]
        [SerializeField] float _obstacleEpsilon = 0.2f;

        [SerializeField] bool _showObstacles = true;

        [SerializeField] LayerMask _obstacleLayer;

        private Collider2D[] _overlappedObstacles = new Collider2D[1];

        public Node[,] Nodes {get;set;}

        public int GridSize => _numOfColumns * _numOfRows;

        public Vector3 Origin => transform.position;

        public float StepCost => _gridCellSize;

        protected override void Awake()
        {
            base.Awake();
            ComputeGrid();
            MarkObstacleNeighbors();
        }

        private void ComputeGrid()
        {
            Nodes = new Node[_numOfRows, _numOfColumns];

            for (int i = 0; i < _numOfColumns; i++)
            {
                for (int j = 0; j < _numOfRows; j++)
                {
                    Vector3 cellPos = GetGridCellCenter(i, j);
                    Node node = new(cellPos);
                    node.X = i;
                    node.Y = j;
                    int collisions = Physics2D.OverlapCircleNonAlloc(cellPos, _gridCellSize / 2.0f - _obstacleEpsilon, _overlappedObstacles, _obstacleLayer);
                    if(collisions != 0) node.MarkAsObstacle();

                    Nodes[i, j] = node;
                }
            }
        }

        private void MarkObstacleNeighbors()
        {
            for (int i = 0; i < _numOfColumns; i++)
            {
                for (int j = 0; j < _numOfRows; j++)
                {
                    var node = Nodes[i, j];
                    if(!node.IsObstacle) continue;
                    var neighbors = GetTraversableNeighbors(i, j);
                    foreach(Node neighbor in neighbors)
                    {
                        neighbor.IsNearObstacle = true;
                    }
                }
            }
        }

        private Vector3 GetGridCellCenter(int i, int j)
        {
            Vector3 cellPos = GetGridCellPosition(i, j);
            cellPos.x += _gridCellSize / 2.0f;
            cellPos.y += _gridCellSize / 2.0f;

            return cellPos;
        }

        private Vector3 GetGridCellPosition(int i, int j)
        {
            float xPosInGrid = i * _gridCellSize;
            float yPosInGrid = j * _gridCellSize;

            return Origin + new Vector3(xPosInGrid, yPosInGrid, 0);
        }

        public bool IsInBounds(Vector3 pos)
        {
            float width = _gridCellSize * _numOfColumns;
            float height = _gridCellSize * _numOfRows;

            if(pos.x < Origin.x) return false;
            if(pos.x > Origin.x + width) return false;
            if(pos.y < Origin.y) return false;
            if(pos.y > Origin.y + height) return false;
            return true;
        }
        public bool IsTraversable(int i, int j)
        {
            return i >= 0
                && j >= 0
                && i < _numOfColumns
                && j < _numOfRows
                && !Nodes[i, j].IsObstacle;
        }

        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            var (column, row) = GetGridCoordinates(node.WorldPosition);
            neighbors.AddRange(GetTraversableNeighbors(column, row));
            return neighbors;
        }
        private List<Node> GetTraversableNeighbors(int column, int row)
        {
            List<Node> traversableNeighbors = new List<Node>();
            if (IsTraversable(column - 1, row)) traversableNeighbors.Add(Nodes[column - 1, row]); // west
            if (IsTraversable(column + 1, row)) traversableNeighbors.Add(Nodes[column + 1, row]); // east
            if (IsTraversable(column, row - 1)) traversableNeighbors.Add(Nodes[column, row - 1]); // south
            if (IsTraversable(column, row + 1)) traversableNeighbors.Add(Nodes[column, row + 1]); // north
            if (IsTraversable(column - 1, row - 1)) traversableNeighbors.Add(Nodes[column - 1, row - 1]); // south-west
            if (IsTraversable(column + 1, row - 1)) traversableNeighbors.Add(Nodes[column + 1, row - 1]); // south-east
            if (IsTraversable(column - 1, row + 1)) traversableNeighbors.Add(Nodes[column - 1, row + 1]); // north-west
            if (IsTraversable(column + 1, row + 1)) traversableNeighbors.Add(Nodes[column + 1, row + 1]); // north-east

            return traversableNeighbors;
        }

        public (int, int) GetGridCoordinates(Vector3 position)
        {
            if(!IsInBounds(position)) return (-1, -1);
            int col = (int)Mathf.Floor((position.x - Origin.x) / _gridCellSize);
            int row = (int)Mathf.Floor((position.y - Origin.y) / _gridCellSize);
            return (col, row);
        }

        public Node GetNodeByCoordinates(Vector3 position)
        {
            var (startCol, startRow) = GetGridCoordinates(position);
            if(startCol < 0 || startRow < 0) return null;

            return Nodes[startCol, startRow];
        }

        public void Reset()
        {
            for (int i = 0; i < _numOfColumns; i++)
            {
                for (int j = 0; j < _numOfRows; j++)
                {
                    var node = Nodes[i,j];
                    node.Parent = null;
                    node.IsClosed = false;
                    node.F = 0;
                    node.G = 0;
                    node.H = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(_showGrid) DrawGrid(Color.blue);

            Gizmos.DrawSphere(Origin, 0.5f);

            if(Nodes == null) return;

            if(_showObstacles)
            {
                Vector3 cellSize = new Vector3(_gridCellSize, _gridCellSize, 1.0f);
                for (int i = 0; i < _numOfColumns; i++)
                {
                    for (int j = 0; j < _numOfRows; j++)
                    {
                        Gizmos.color = Color.red;
                        if(Nodes[i,j].IsObstacle) Gizmos.DrawCube(GetGridCellCenter(i, j), cellSize);
                        Gizmos.color = Color.yellow;
                        if(Nodes[i,j].IsNearObstacle) Gizmos.DrawCube(GetGridCellCenter(i, j), cellSize);
                    }
                }
            }
        }

        private void DrawGrid(Color color)
        {
            float width = (float)_numOfColumns * _gridCellSize;
            float height = (float)_numOfRows * _gridCellSize;

            //draw horizontal
            for (int i = 0; i < _numOfRows; i++)
            {
                Vector3 startPos = Origin + i * _gridCellSize * new Vector3(1, 0, 0);
                Vector3 endPos = startPos + width * new Vector3(0, 1, 0);
                Debug.DrawLine(startPos, endPos, color);
            }

            //draw vertical
            for (int i = 0; i < _numOfColumns; i++)
            {
                Vector3 startPos = Origin + i * _gridCellSize * new Vector3(0, 1, 0);
                Vector3 endPos = startPos + height * new Vector3(1, 0, 0);
                Debug.DrawLine(startPos, endPos, color);
            }
        }
    }
}
