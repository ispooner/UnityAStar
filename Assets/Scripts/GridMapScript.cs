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

    void Awake()
    {
        //Old code for testing coroutines
        //StartCoroutine(Generate());
        GridSquareScript[,] grid = new GridSquareScript[width, height];
        makeMap(grid);
        fillNeighbours(grid);
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
    private void makeMap(GridSquareScript[,] grid) {
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                grid[x,z] = Instantiate(gridSquare, new Vector3(x, 0, z), Quaternion.identity);
                grid[x,z].position = new Vector2(x, z);
            }
        }
        grid[0,0].setStart();
        grid[9,9].setEnd();
    }

    //After the squares are generated add their neighbours to each other.
    private void fillNeighbours(GridSquareScript[,] grid) {
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

}
