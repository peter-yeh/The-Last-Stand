//using System.Numerics;
//using System.Numerics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject[] players;
    //public Transform[] players;

    public Vector3 offset;
    private Vector3 velocity;
    private float smoothTime;
    public float defaultSmoothTime;

    public float minZoom;
    public float maxZoom;
    public float zoomLimiter = 20f;

    private Camera cam;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        cam = GetComponent<Camera>();
        //players = GameObject.FindGameObjectsWithTag("Player");
        smoothTime = defaultSmoothTime; 
    }
    void LateUpdate() {
        //Checks the number of players on the map
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return;
        }

        if (players.Length == 1)
        {
            smoothTime = 0.1f;
        } else if (players.Length == 2)
        {
            smoothTime = defaultSmoothTime;
        }

        Move();
        Zoom();

    }

    void Move() 
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        //newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        newPosition = new Vector3(Mathf.Clamp(newPosition.x, minX, maxX), Mathf.Clamp(newPosition.y, minY, maxY), transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);

        if (players.Length == 2)
        {
            bounds.Encapsulate(players[1].transform.position);
        }

        return Mathf.Max(bounds.size.x, bounds.size.y);

    }

    Vector3 GetCenterPoint()
    {
        if (players.Length == 1)
        {
            return players[0].transform.position;
        }
        else
        {
            var bounds = new Bounds(players[0].transform.position, Vector3.zero);
            bounds.Encapsulate(players[1].transform.position);
            return bounds.center;
        }
    }
}
