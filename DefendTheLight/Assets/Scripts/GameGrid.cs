using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGrid : MonoBehaviour
{
    GameObject[,] objectGrid;
    int[,] costGrid; // cost from each node to goal
    int cols = 32;
    int rows = 14;
    Node goal = null;
    int targetRow = 6;

    public List<Tower> GetAllTowers()
    {
        List<Tower> newList = new List<Tower>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (objectGrid[i, j] != null)
                {
                    Tower t = objectGrid[i, j].GetComponent<Tower>();
                    if (t != null) newList.Add(t);
                }
            }
        }
        return newList;
    }

    public GameGrid()
    {
        GameObject endWall = new GameObject();
        endWall.name = "End Wall";
        objectGrid = new GameObject[rows, cols];
        for(int i = 0; i < rows; i++)
        {
            // Last col unusable
            if(i != targetRow) { GameObject blank = new GameObject(); blank.tag = "Wall"; objectGrid[i, cols - 1] = blank; blank.transform.parent = endWall.transform; }
        }
        costGrid = new int[rows, cols];
        UpdateCostGrid();

        

    }
    public void GetPosition(GameObject obj, ref int row, ref int col)
    {
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                if (objectGrid[i, j] == obj)
                {
                    row = i;
                    col = j;
                    return;
                }
            }
        }
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

    public void UpdateCostGrid()
    {
        // Any time a tower is created or destoryed, update the cost grid.
        // Set all nodes to -1.
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                // Last col not usable except for goal
                if(j == cols - 1 && i != targetRow)
                {
                    costGrid[i, j] = 999;
                }
                else
                {
                    costGrid[i, j] = -1;
                }
            }
        

        // Set first node to 1. First node is the one directly adj to the goal
        SetNode(1, targetRow, cols - 1); // This function will call itself and fill in the entire grid
        Debug.Log("Set");

        for (int i = 0; i < rows; i++)
        {
            string s = "";
            for (int j = 0; j < cols; j++)
            {
                s += costGrid[i, j] + ", ";
            }
            Debug.Log(i + ": " + s);
        }
    }

    void SetNode(int cost, int i, int j)
    {
        // Sets current node, then branches out to all adj nodes, up,down,left,right
        // First check if the node is in boundary
        if (i < 0 || i >= rows || j < 0 || j >= cols ) return;



        if (objectGrid[i, j] == null)
        {
            // Replace if current cost is greater, or unset (-1)
            if (costGrid[i, j] > cost || costGrid[i, j] < 0)
                costGrid[i, j] = cost;
            else return; // already visited this node, dont go to adj nodes.
        }
        else
        {
            if (costGrid[i, j] < 0) // if its not set yet
            {
                if (objectGrid[i, j].tag == "Wall") { costGrid[i, j] = 99; cost = 99; }
                else { costGrid[i, j] = 50; cost = 50; }// regular towers have a cost greater than 1. 
            }
            else return; // already visited this node, dont go to adj nodes.
        }

        cost++; // increase cost by 1
        SetNode(cost, i + 1, j);
        SetNode(cost, i - 1, j);
        SetNode(cost, i, j + 1);
        SetNode(cost, i, j - 1);

    }

    public int GetNextPath(Facing facing, int i, int j, ref Tower target)
    {
        // First check if the node is in boundary
        if (i < 0 || i >= rows || j < 0 || j >= cols) return 0;

        int cost = 999;
        bool blocked = false; // whether a tower is in way
        // Check 3 directions (exclude going back) move to min cost node
        // sides cost +1 more, this priorities going forward. 
        int dir = -1;

        if (i == targetRow && j == cols - 1) return -1;

        if(facing == Facing.Up)
        {
            // Forward
            int min = CheckCost(cost, i + 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Up; }
            // Sides
            min = 1 + CheckCost(cost + 1, i, j + 1);
            if (min < cost) { cost = min; dir = (int)Facing.Right; }
            min = 1 + CheckCost(cost + 1, i, j - 1);
            if (min < cost) { cost = min; dir = (int)Facing.Left; }
        }
        else if (facing == Facing.Down)
        {
            // Forward
            int min = CheckCost(cost, i - 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Down; }
            // Sides
            min = 1 + CheckCost(cost + 1, i, j + 1);
            if (min < cost) { cost = min; dir = (int)Facing.Right; }
            min = 1 + CheckCost(cost + 1, i, j - 1);
            if (min < cost) { cost = min; dir = (int)Facing.Left; }
        }
        else if (facing == Facing.Left)
        {
            // Forward
            int min = CheckCost(cost, i, j - 1);
            if (min < cost) { cost = min; dir = (int)Facing.Left; }
            // Sides
            min = 1 + CheckCost(cost + 1, i + 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Up; }
            min = 1 + CheckCost(cost + 1, i - 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Down; }
        }
        else if (facing == Facing.Right)
        {
            // Forward
            int min = CheckCost(cost, i, j + 1);
            if (min < cost) { cost = min; dir = (int)Facing.Right;}
            // Sides
            min =  1 + CheckCost(cost + 1, i + 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Up; }
            min = 1 + CheckCost(cost + 1, i - 1, j);
            if (min < cost) { cost = min; dir = (int)Facing.Down; }
        }

        // Check if next node has a tower, if so send singal to attack
        if (dir == (int)Facing.Up) target = CheckBlock(i + 1, j);
        else if (dir == (int)Facing.Down) target = CheckBlock(i - 1, j);
        else if (dir == (int)Facing.Left) target = CheckBlock(i, j - 1);
        else if (dir == (int)Facing.Right) target = CheckBlock(i, j + 1);
        if (target != null) return 5;

        return dir; // return code to enemy
    }

    // Checks cost within bounds
    int CheckCost(int cost, int i, int j)
    {
        // First check if the node is in boundary
        if (i < 0 || i >= rows || j < 0 || j >= cols) return 999;
        else return costGrid[i, j];
    }
    Tower CheckBlock(int i, int j)
    {
        if (i < 0 || i >= rows || j < 0 || j >= cols) return null;
        if (objectGrid[i, j] != null) return objectGrid[i, j].GetComponent<Tower>();
        return null;
    }
    #endregion

}
