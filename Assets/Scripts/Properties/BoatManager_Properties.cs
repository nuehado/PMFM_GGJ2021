using UnityEngine;

[CreateAssetMenu]
public class BoatManager_Properties : ScriptableObject
{
    public GameObject boatPrefab;
    public float timeBeforeFirstBoat;
    public float minTimeBetweenBoats;
    public float maxTimeBetweenBoats;
    public float boatWaitTime;
    public float islandRadius = 50f;
    public float distFromCoast;
    public float verticalSpeed;
    public float timeToReachShore;
    public Sprite beachNE;
    public Sprite beachSE;
    public Sprite beachSW;
    public Sprite beachNW;
    public Vector3 rotSE = new Vector3(20f, -160f, -20f);
    public Vector3 rotNW = new Vector3(60f, -180f, -30f);
    public Vector3 rotSW = new Vector3(20f, -95f, 25f);
    public Vector3 rotNE = new Vector3(15f, -110f, 20f);
    public Vector3 dockingLocationNE;
    public Vector3 dockingLocationSE;
    public Vector3 dockingLocationSW;
    public Vector3 dockingLocationNW;
}
