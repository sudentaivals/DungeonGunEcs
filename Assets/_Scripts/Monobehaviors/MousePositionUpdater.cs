using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionUpdater : MonoBehaviour
{

    private void Start()
    {
        //Cursor.visible = false;
    }
    void Update()
    {
        transform.position = InputSingleton.Instance.MousePosition;
    }
}
