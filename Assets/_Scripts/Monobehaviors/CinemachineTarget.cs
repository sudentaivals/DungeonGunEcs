using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{   
    private static CinemachineTargetGroup _cinemachineTargetGroup;

    public static CinemachineTargetGroup CinemachineTargetGroup => _cinemachineTargetGroup;
    private void Awake()
    {
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }
}
