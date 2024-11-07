using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using UnityEngine;

namespace CustomAstar
{
    public class Astar
    {
        private FastPriorityQueue<Node> _openList;

        private readonly int CARDINAL_COST = 70;
        private readonly int DIAGONAL_COST = 99;

        private int _gCost;
        private int Octile8DirCost(Node currentNode, Node goalNode)
        {
            var dX = Mathf.Abs(currentNode.X - goalNode.X);
            var dY = Mathf.Abs(currentNode.Y - goalNode.Y);
            if(dX > dY) return CARDINAL_COST * dX + (DIAGONAL_COST - CARDINAL_COST) * dY;
            else return CARDINAL_COST * dY + (DIAGONAL_COST - CARDINAL_COST) * dX;
        }

        public Astar(int numOfNodes = 2000)
        {
            _openList = new(numOfNodes);
        }

        public Stack<Node> FindPath(Node startNode, Node goalNode)
        {
            GridManager.Instance.CurrentGrid.Reset();
            _openList.Clear();

            _openList.Enqueue(startNode, Octile8DirCost(startNode, goalNode));
            Node node = null;
            while(_openList.Count > 0)
            {
                node = _openList.Dequeue();
                if(node.WorldPosition == goalNode.WorldPosition)
                {
                    return CalculatePath(node);
                }
                node.IsClosed = true;

                var neighbors = GridManager.Instance.CurrentGrid.GetNeighbors(node);
                foreach (Node neighbourNode in neighbors)
                {
                    if(neighbourNode.IsClosed) continue;

                    if(neighbourNode.IsObstacle)
                    {
                        neighbourNode.IsClosed = true;
                        continue;
                    }
                    _gCost = node.G + Octile8DirCost(node, neighbourNode);
                    if(!_openList.Contains(neighbourNode))
                    {
                        int addition = neighbourNode.IsNearObstacle ? 70 : 0;
                        neighbourNode.G = _gCost + addition;
                        neighbourNode.H = Octile8DirCost(neighbourNode, node);
                        neighbourNode.Parent = node;
                        neighbourNode.F = neighbourNode.G + neighbourNode.H ;
                        _openList.Enqueue(neighbourNode, neighbourNode.F);
                    }
                    else if(_gCost + neighbourNode.G < neighbourNode.F)
                    {
                        neighbourNode.G = _gCost;
                        neighbourNode.F = neighbourNode.G + neighbourNode.H;
                        neighbourNode.Parent = node;
                    } 
                }
            }
            if(node.WorldPosition != goalNode.WorldPosition)
            {
                return null;
            }

            return CalculatePath(node);
        }

        public Stack<Node> FindPath(Vector3 startPos, Vector3 goalPos)
        {
            if(GridManager.Instance.CurrentGrid == null) return null;
            var startNode = GridManager.Instance.CurrentGrid.GetNodeByCoordinates(startPos);
            if(startNode == null) return null;
            var endNode = GridManager.Instance.CurrentGrid.GetNodeByCoordinates(goalPos);
            if(endNode == null || endNode.IsObstacle) return null;
            return FindPath(startNode, endNode);
        }

        private Stack<Node> CalculatePath(Node node)
        {
            Stack<Node> path = new();
            while(node != null)
            {
                path.Push(node);
                node = node.Parent;
            }
            return path;
            //return path.ToList();
        }
    }
}
