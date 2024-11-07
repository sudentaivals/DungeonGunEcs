using UnityEditor;
using UnityEngine;

namespace CustomEditor
{
    public abstract class BaseCustomEditor : EditorWindow
    {
        protected BaseCustomEditor _currentSelecteCustomEditor;
        #region background grid
        //background grid variables
        private readonly float _gridLarge = 100f;
        private readonly float _gridlargeLineWidth = 0.2f;
        private readonly Color _gridLargeColor = Color.gray;
        private readonly float _gridLargeOpacity = 0.3f;
        private readonly float _gridSmall = 25f;
        private readonly float _gridSmallLineWidth = 0.2f;
        private readonly Color _gridSmallColor = Color.gray;
        private readonly float _gridSmallOpacity = 0.2f;
        private Vector2 _graphOffset;
        private Vector2 _graphDrag;
        #endregion

        private void DrawBackgroundGrid(float gridSize, float lineSize, Color lineColor, float gridOpacity)
        {
            int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
            int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);
            Handles.color = new Color(lineColor.r, lineColor.g, lineColor.b, gridOpacity);
            _graphOffset += _graphDrag * 0.5f;

            Vector3 gridOffset = new Vector3(_graphOffset.x % gridSize, _graphOffset.y % gridSize, 0f);
            for (int i = 0; i < verticalLineCount; i++)
            {
                Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset, lineSize);
            }
            for (int i = 0; i < horizontalLineCount; i++)
            {
                Handles.DrawLine(new Vector3(-gridSize, gridSize * i, 0) + gridOffset, new Vector3(position.width + gridSize, gridSize * i, 0f) + gridOffset, lineSize);
            }
            Handles.color = Color.white;
        }

        protected virtual void DrawGrid()
        {
            DrawBackgroundGrid(_gridSmall, _gridSmallLineWidth, _gridSmallColor, _gridSmallOpacity);
            DrawBackgroundGrid(_gridLarge, _gridlargeLineWidth, _gridLargeColor, _gridLargeOpacity);
        }

        private void OnGUI()
        {
            if(_currentSelecteCustomEditor != null)
            {
                DrawGrid();
                //dragged line
                DrawBeforeEvents();
                ProcessEvents(Event.current);
                //room nodes and node conections
                DrawAfterEvents();
            }

            if(!GUI.changed) return;
            Repaint();
        }

        protected virtual void DrawAfterEvents()
        {

        }

        protected virtual void DrawBeforeEvents()
        {
            
        }

        protected virtual void ProcessEvents(Event currentEvent)
        {
            _graphDrag = Vector2.zero;
        }

        /*protected bool IsMouseOverNode(Event currentEvent)
        {

        }*/
    }
}
