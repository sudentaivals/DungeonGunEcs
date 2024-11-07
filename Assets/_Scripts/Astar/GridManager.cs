using System;
using UnityEngine;

namespace CustomAstar
{
    public class GridManager : Singleton<GridManager>
    {
        [HideInInspector] public PathfindingGrid CurrentGrid {get; private set;} = null;
        [SerializeField] private bool _overrideShowGrid = false;
        [SerializeField] private bool _showGrid = true;
        [SerializeField] private bool _showObstacles = true;

        public bool OverrideShowGrid => _overrideShowGrid;
        public bool ShowGrid => _showGrid;
        public bool ShowObstacles => _showObstacles;
        private void OnEnable() 
        {
            EcsEventBus.Subscribe(GameplayEventType.SetPathfindGrid, SetGrid);
        }

        private void OnDisable() 
        {
            EcsEventBus.Unsubscribe(GameplayEventType.SetPathfindGrid, SetGrid);
        }
        private void SetGrid(int sender, EventArgs args)
        {
            var gridArgs = args as SetPathfindGridEventArgs;
            CurrentGrid = gridArgs.NewPathfindingGrid;
        }
    }
}
