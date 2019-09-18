using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquareScript : MonoBehaviour
{

    static GameObject selected;
    public GameObject wall;
    public MeshRenderer childRenderer;

    // Start is called before the first frame update
    void Start()
    {
        wall = gameObject.transform.GetChild(1).gameObject;
        childRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent("MeshRenderer") as MeshRenderer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if(selected != null) {
            GridSquareScript old = selected.GetComponent("GridSquareScript") as GridSquareScript;
            old.childRenderer.material.SetColor("_Color", Color.white);
            old.wall.SetActive(false);
        }
        selected = this.gameObject;
        childRenderer.material.SetColor("_Color", Color.blue);
        wall.SetActive(true);
    }

    void OnMouseUp() {
        //childRenderer.material.SetColor("_Color", Color.white);
    }
}
