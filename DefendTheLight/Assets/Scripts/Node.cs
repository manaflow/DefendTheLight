using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    public int row;
    public int col;
    public int cost = 3000;
    public List<int> path;

    public Node(int i, int j, int Cost, List<int> Path)
    {
        row = i;
        col = j;
        cost = Cost;
        path = new List<int>();

        if (Path != null)
            for (int x = 0; x < Path.Count; x++)
                path.Add(Path[x]);
    }
}
