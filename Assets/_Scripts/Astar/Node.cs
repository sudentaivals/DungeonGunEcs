using System;
using Priority_Queue;
using UnityEngine;

namespace CustomAstar
{
    public class Node : FastPriorityQueueNode
    {
        public int X;
        public int Y;
        public int G;
        public int F;
        public int H;
        public bool IsObstacle;
        public bool IsNearObstacle;
        public Node Parent;
        public Vector3 WorldPosition;
        public bool IsClosed;

        public Node(Vector3 pos)
        {
            F = 0;
            G = 0;
            H = 0;
            IsObstacle = false;
            IsNearObstacle = false;
            Parent = null;
            WorldPosition = pos;
        }

        public void MarkAsObstacle()
        {
            IsObstacle = true;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && WorldPosition.Equals(node.WorldPosition);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorldPosition);
        }

    }

}
