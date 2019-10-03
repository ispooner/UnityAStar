using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarScript
{
    static List<GridSquareScript> openList;
    static List<GridSquareScript> closedList;

    // There should only be one pathfinder at any given time, so yeah, everything is static. 
    public static void findPath() {

    }

    /*
    The Heuristic needs to be strictly less than the actual distance to 
    get a correct implementation. The easiest is Euclidean distance, but 
    a better heuristic would be octile distance. 
     */
    public static float heuristic(GridSquareScript current) {
        //Octile distance is the shortest possible distance on a regular square grid. 

        //The squares themselves lie on the XZ plane. Vector2 uses XY, so a bit of translation is needed.
        float difX = GridSquareScript.end.position.x - current.position.x;
        float difY = GridSquareScript.end.position.y - current.position.y;

        //The shorter distance is the diagonal distance. 1.41 is a suitable distance measure for the diagonal.
        float diag = (difX < difY ? difX : difY) * 1.41f;
        //The straight distance is the difference between the two axes. 
        float straight = Mathf.Abs(difX - difY);
        //The total distance is the sum of the two.
        return diag + straight;
    }
}
