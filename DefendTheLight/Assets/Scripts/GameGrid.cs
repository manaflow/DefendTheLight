using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGrid : MonoBehaviour
{
    GameObject[,] objectGrid;
    int cols = 32;
    int rows = 14;
    Node goal = null;

    public GameGrid()
    {
        objectGrid = new GameObject[rows, cols];
    }

    public GameObject CheckGrid(int i, int j)
    {
        // out of range
        if (i < 0 || i >= rows || j < 0 || j >= cols) return null;
        return objectGrid[i, j];
    }

    public void SetGrid(int i, int j, GameObject obj)
    {
        // out of range
        if (i < 0 || i >= rows || j < 0 || j >= cols) return;
        objectGrid[i,j] = obj;
    }

    #region Pathfinding

    public List<int> GetPath(int i, int j)
    {
        goal = null;

        Node[,] pathGrid = new Node[rows, cols]; // an array of nodes. contains the total cost and shortest path so far

        Node first = new Node(i, j, 0, null);
        List<Node> minQue = new List<Node>();
        minQue.Add(first);

        while(minQue.Count > 0)
        {
            Node node = minQue[0];
            minQue.RemoveAt(0);

            Node right = Search(node.row, node.col + 1, node.cost, ref node.path, 2, ref objectGrid, ref pathGrid);
            //Node up = Search(node.row - 1, node.col, node.cost, ref node.path, 1, ref objectGrid, ref pathGrid);            
            //Node down = Search(node.row + 1, node.col, node.cost, ref node.path, 3, ref objectGrid, ref pathGrid);
            //Node left = Search(node.row, node.col - 1, node.cost, ref node.path, 4, ref objectGrid, ref pathGrid);

            if (goal != null && node == goal) break;

           // if (up != null) AddNode(ref up, ref minQue);
            if (right != null) AddNode(ref right, ref minQue);
           // if (down != null) AddNode(ref down, ref minQue);
           // if (left != null) AddNode(ref left, ref minQue);
        }

        if (goal != null) return goal.path;
        return null;
    }

    // Adds the node to the min Queue in order of its cost
    void AddNode(ref Node node, ref List<Node> minQue)
    {
        for (int i = 0; i < minQue.Count; i++)
        {
            if (node.cost < minQue[i].cost) { minQue.Insert(i, node); return; }
        }
        minQue.Add(node);
    }

    Node Search(int i, int j, int cost, ref List<int> path, int dir, ref GameObject[,] grid, ref Node[,] pathGrid)
    {
        cost += 1;

        // Is Goal
        if ( j == 31)
        {
            Node g = pathGrid[i, j];
            if (g != null && g.cost <= cost) return null;
            goal = new Node(i, j, cost, path);
            goal.path.Add(dir);
            pathGrid[i, j] = goal;
            return null;
        }
        // Check bounds
        if (i < 0 || i >= rows || j < 0 || j >= cols) return null;
        // Is obstacle 
        if (grid[i, j] != null)
        {
            Debug.Log(grid[i, j]);
            Tower tower = grid[i, j].GetComponent<Tower>();
            Debug.Log(tower);
            if (tower != null) cost += tower.ward;

        }

        Node node = pathGrid[i, j];
        // Only replace if higher
        if (node != null && node.cost <= cost) return null;
        if (node == null) node = new Node(i, j, cost, path);
        node.path.Add(dir);
        pathGrid[i, j] = node;
        return node;


    }

    #endregion
}
