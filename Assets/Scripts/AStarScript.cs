using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarScript : MonoBehaviour
{
    static List<GridSquareScript> openList;
    static List<GridSquareScript> closedList;

    // There should only be one pathfinder at any given time, so yeah, everything is static. 
    public static IEnumerator findPath() {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        GridSquareScript.start.distanceFromStart = 0;
        GridSquareScript.start.distanceFromEnd = heuristic(GridSquareScript.start);
        closedList.Add(GridSquareScript.start);

        foreach(GridSquareScript n in GridSquareScript.start.neighbors) {
            //add the neighbors to the open list.
            n.parent = GridSquareScript.start;
            n.distanceFromEnd = heuristic(n);
            n.distanceFromStart = n.parent.distanceFromStart + (n.position - n.parent.position).magnitude;
            n.childRenderer.material.SetColor("_Color", Color.yellow);
            yield return wait;
        }

        while(!closedList.Contains(GridSquareScript.end)) {
            if(openList.Count == 0) {
                //There are no more squares to explore. 
                break;
            }
            GridSquareScript current = openList[0];
            float curDist = current.distanceFromStart + current.distanceFromEnd;
            foreach(GridSquareScript sq in openList) {
                if(curDist > sq.distanceFromEnd + sq.distanceFromStart) {
                    //Swap to the smallest total distance.
                    curDist = sq.distanceFromEnd + sq.distanceFromStart;
                    current = sq;
                }
            }
            if(current == GridSquareScript.end) {
                //We've found the path, time to render it.
                //TODO: Finish the path rendering.
            }
            else {
                //Shows the selected open node.
                current.childRenderer.material.SetColor("_Color", Color.magenta);
                yield return wait);
                foreach(GridSquareScript n in current.neighbors) {
                    n.parent = current;
                    n.distanceFromEnd = heuristic(n);
                    n.distanceFromStart = n.parent.distanceFromStart + (n.position - n.parent.position).magnitude;
                    n.childRenderer.material.SetColor("_Color", Color.yellow);
                    yield return wait;
                }
                
            }

        }

        yield return wait;
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
