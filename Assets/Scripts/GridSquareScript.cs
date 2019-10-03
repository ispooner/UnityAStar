using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquareScript : MonoBehaviour
{

    public static GridSquareScript start;
    public static GridSquareScript end;
    public GameObject wall;
    public MeshRenderer childRenderer;
    bool walkable;

    //toggles for selecting things
    public static Toggle WallToggle;
    public static Toggle StartToggle;
    public static Toggle EndToggle;
    public Vector2 position;
    public float distanceFromStart;
    public List<GridSquareScript> neighbors;

    // Start is called before the first frame update

    //Learning moment. Start is not called upon instantiating a prefab. 
    //good to know
    void Start()
    {
        //wall = gameObject.transform.GetChild(1).gameObject;
        //childRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent("MeshRenderer") as MeshRenderer;
        Debug.Log(position.x + ", " + position.y + "is starting");
        walkable = true;
    }

    void Awake() {
        Debug.Log(position.x + ", " + position.y + "is awakening");
        walkable = true;
    }

    void OnMouseDown() {
        Debug.Log("Clicked on " + position.x + ", " + position.y);
        if(WallToggle.isOn) {
            setWall();
        }
        else if(StartToggle.isOn) {
            setStart();
        }
        else if(EndToggle.isOn) {
            setEnd();
        }

        if(start != null && end != null) {
            AStarScript.findPath();
        }
    }

    public void setWall() {
        if(walkable) {
                childRenderer.material.SetColor("_Color", Color.blue);
                wall.SetActive(true);
                walkable = false;
                Debug.Log("Wall is active");
            }
            else {
                childRenderer.material.SetColor("_Color", Color.white);
                wall.SetActive(false);
                walkable = true;
                Debug.Log("Wall is inactive");
            }
            if(start == this) {
                start = null;
            }
            if(end == this) {
                end = null;
            }
    }

    public void setStart() {
        if(start != null) {
                start.childRenderer.material.SetColor("_Color", Color.white);
            }
            start = this;
            childRenderer.material.SetColor("_Color", Color.green);
            if(end == start) {
                end = null;
            }
            Debug.Log("Start selected");
    }

    public void setEnd() {
        if(end != null) {
                end.childRenderer.material.SetColor("_Color", Color.white);
            }
            end = this;
            childRenderer.material.SetColor("_Color", Color.red);
            if(start == end) {
                start = null;
            }
            Debug.Log("End Selected");
    }

    void OnMouseUp() {
        //childRenderer.material.SetColor("_Color", Color.white);
    }
}
