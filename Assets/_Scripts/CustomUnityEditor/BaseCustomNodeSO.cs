using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomEditor
{    
    public abstract class BaseCustomNodeSO : ScriptableObject
    {
        [HideInInspector] public string Id;
        [HideInInspector] public BaseCustomRoomGraphSO Graph;

        #region GuiStyle
        //normal style
        private GUIStyle _normalStyle;
        private GUIStyle _selectedStyle;
        protected Texture2D _normalBackground;
        protected Texture2D _selectedBackground;
        protected int _nodeBorder;
        protected int _nodePadding;
        protected Color _textColor = Color.white;
        #endregion

        [HideInInspector] public bool IsSelected;
        [HideInInspector] public Rect Rect;

        /// <summary>
        /// Oveeride this method to set gui style variables (nodePadding, nodeBorder, normalBackground, selectedBackground)
        /// </summary>
        protected virtual void SetGuiStyleVariables()
        {
            _nodePadding = 25;
            _nodeBorder = 12;
            _normalBackground = EditorGUIUtility.Load("node1") as Texture2D;
            _selectedBackground = EditorGUIUtility.Load("node1 on") as Texture2D;
        }

        private void SetGuiStyle()
        {
            _normalStyle = new GUIStyle();
            _normalStyle.normal.background = _normalBackground;
            _normalStyle.normal.textColor = _textColor;
            _normalStyle.padding = new RectOffset(_nodePadding, _nodePadding, _nodePadding, _nodePadding);
            _normalStyle.border = new RectOffset(_nodeBorder, _nodeBorder, _nodeBorder, _nodeBorder);


            _selectedStyle = new GUIStyle();
            _selectedStyle.normal.background = _selectedBackground;
            _selectedStyle.normal.textColor = _textColor;
            _selectedStyle.padding = new RectOffset(_nodePadding, _nodePadding, _nodePadding, _nodePadding);
            _selectedStyle.border = new RectOffset(_nodeBorder, _nodeBorder, _nodeBorder, _nodeBorder);
        }

        public void Initialise(BaseCustomRoomGraphSO graph)
        {
            SetGuiStyleVariables();
            SetGuiStyle();
            Graph = graph;
        }

        public virtual void Draw()
        {
            var style = IsSelected ? _selectedStyle : _normalStyle;
            GUILayout.BeginArea(Rect, style);
            //start Region to detect popup slection changes
            EditorGUI.BeginChangeCheck();

            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this);
            }

            GUILayout.EndArea();
        }
    }
}
