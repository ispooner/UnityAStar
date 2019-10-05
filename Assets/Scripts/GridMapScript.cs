using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This script creates a simple map and initiates the neighbours. */

public class GridMapScript : MonoBehaviour
{
    public GridSquareScript gridSquare;

    public int width, height;

    //This is really hacky and I fucking hate doing this, but it works
    public Toggle wallToggle;
    public Toggle startToggle;
    public Toggle endToggle;
    GridSquareScript[,] grid;
    public Text labelText;

    void Awake()
    {
        //Old code for testing coroutines
        //StartCoroutine(Generate());
        grid = new GridSquareScript[width, height];
        makeMap();
        fillNeighbours();
        GridSquareScript.WallToggle = wallToggle;
        GridSquareScript.StartToggle = startToggle;
        GridSquareScript.EndToggle = endToggle;
    }

    /*
    private IEnumerator Generate() {
        WaitForSeconds wait = new WaitForSeconds(0f);
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                Instantiate(gridSquare, new Vector3(x, 0, z), Quaternion.identity);
                yield return wait;
            }
        }
    }
    */

    //Generate the map squares.
    private void makeMap() {
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                grid[x,z] = Instantiate(gridSquare, new Vector3(x, 0, z), Quaternion.identity);
                grid[x,z].position = new Vector2(x, z);
                grid[x,z].mapScript = this;
                Text label = Instantiate<Text>(labelText);
                label.rectTransform.SetParent(grid[x,z].canvas.transform, false);
                label.rectTransform.anchoredPosition = new Vector2(0f, 0f);
                label.text = x.ToString() + ", " + z.ToString();
                grid[x,z].text = label;
            }
        }
        grid[0,0].setStart();
        grid[9,9].setEnd();
    }

    //After the squares are generated add their neighbours to each other.
    private void fillNeighbours() {
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                GridSquareScript obj = grid[x,z];
                obj.position = new Vector2(x,z);
                for(int x1 = x - 1; x1 <= x + 1; x1++) {
                    for(int z1 = z - 1; z1 <= z + 1; z1++) {
                        if(x1 < 0 || x1 >= width) {
                            continue;
                        }
                        if(z1 < 0 || z1 >= height) {
                            continue;
                        }
                        if(x1 == x && z1 == z) {
                            continue;
                        }
                        obj.neighbors.Add(grid[x1, z1]);
                    }
                }
            }
        }
    }

    public void resetGrid() {
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                if(grid[x,z].walkable) {
                    grid[x,z].childRenderer.material.SetColor("_Color", Color.white);
                }
                else {
                    grid[x,z].childRenderer.material.SetColor("_Color", Color.blue);
                }
                grid[x,z].text.text = x.ToString() + ", " + z;
                grid[x,z].parent = null;
            }
        }

        GridSquareScript.start.childRenderer.material.SetColor("_Color", Color.green);
        GridSquareScript.end.childRenderer.material.SetColor("_Color", Color.red);
    }

}
