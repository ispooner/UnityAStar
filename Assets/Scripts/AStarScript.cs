using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarScript : MonoBehaviour
{
    static List<GridSquareScript> openList;
    static List<GridSquareScript> closedList;

    // There should only be one pathfinder at any given time, so yeah, everything is static. 
    public static IEnumerator findPath() {
        openList = new List<GridSquareScript>();
        closedList = new List<GridSquareScript>();
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        GridSquareScript.start.distanceFromStart = 0;
        GridSquareScript.start.distanceFromEnd = heuristic(GridSquareScript.start);
        closedList.Add(GridSquareScript.start);

        foreach(GridSquareScript n in GridSquareScript.start.neighbors) {
            //add the neighbors to the open list.
            n.parent = GridSquareScript.start;
            n.distanceFromEnd = heuristic(n);
            n.distanceFromStart = n.parent.distanceFromStart + (n.position - n.parent.position).magnitude;
            n.childRenderer.material.SetColor("_Color", Color.yellow);
            openList.Add(n);
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

                //We've found the path
                break;
            }
            else {
                //Shows the selected open node.
                current.childRenderer.material.SetColor("_Color", Color.magenta);
                yield return wait;
                foreach(GridSquareScript n in current.neighbors) {
                    if(closedList.Contains(n)) {
                        continue;
                    }
                    if(!n.walkable) {
                        continue;
                    }
                    if(openList.Contains(n)) {
                        float th = heuristic(n);
                        float tds = current.distanceFromStart + (n.position - current.position).magnitude;
                        if(n.distanceFromStart + n.distanceFromEnd > th + tds) {
                            n.parent = current;
                            n.distanceFromEnd = th;
                            n.distanceFromStart = tds;
                            n.text.text = n.text.text + 
                                "\nD=" + n.distanceFromStart + 
                                "\nF=" + n.distanceFromEnd + 
                                "\nT=" + (n.distanceFromEnd + n.distanceFromStart);
                        }
                        continue;
                    }
                    n.parent = current;
                    n.distanceFromEnd = heuristic(n);
                    n.distanceFromStart = n.parent.distanceFromStart + (n.position - n.parent.position).magnitude;
                    n.childRenderer.material.SetColor("_Color", Color.yellow);
                    n.text.text = n.text.text + 
                        "\nD=" + n.distanceFromStart + 
                        "\nF=" + n.distanceFromEnd + 
                        "\nT=" + (n.distanceFromEnd + n.distanceFromStart);

                    openList.Add(n);
                    yield return wait;
                }

                openList.Remove(current);
                closedList.Add(current);
                current.childRenderer.material.SetColor("_Color", Color.grey);
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
        float difX = Mathf.Abs(GridSquareScript.end.position.x - current.position.x);
        float difY = Mathf.Abs(GridSquareScript.end.position.y - current.position.y);

        //The shorter distance is the diagonal distance. 1.41 is a suitable distance measure for the diagonal.
        float diag = (difX < difY ? difX : difY) * Mathf.Sqrt(2);
        //The straight distance is the difference between the two axes. 
        float straight = Mathf.Abs(difX - difY);
        //The total distance is the sum of the two.
        return diag + straight;
    }
}
