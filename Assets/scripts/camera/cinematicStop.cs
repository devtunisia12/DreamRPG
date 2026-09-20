using UnityEngine;
using Unity.Cinemachine;

public class cinematicStop : MonoBehaviour
{
    public CinemachineConfiner2D confiner;

    public void DisableConfiner()
    {
        confiner.enabled = false;
    }

    public void EnableConfiner()
    {
        confiner.enabled = true;
    }
}