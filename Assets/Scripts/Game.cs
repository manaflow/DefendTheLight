using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Effect { None, SlowMovement };
public enum AbilityType { Red, Blue, Yellow, Orange, Green, Purple};
public enum Difficulty { Easy, Normal, Hard, VeryHard};
public enum Facing { Up, Down, Left, Right};

public static class Game
{
    public static GameGrid grid;
    public static int Health = 20;

    // Abilities
    public static int redCount;
    public static int blueCount;
    public static int yellowCount;
    public static int purpleCount;
    public static int greenCount;
    public static int orangeCount;

    public static Enemy TargetEnemy = null;
    public static int GameSpeed = 1;    // actual speed, pause makes this 0
    public static int Speed = 1;        // UI speed
    public static bool isPaused = false;
    public static int charges = 99;
    public static int chargeCounter = 0;
    public static int energy = 200;
    public static List<Enemy> enemies;
    public static Difficulty difficulty = Difficulty.Normal;
    public static int wave = 0;
    public static int seed = 0;     // will be used to save game state for replay
    static Rect bounds = new Rect(0 - 1, 0 - 1, 19.2f + 1, 8.4f + 1); // board size plus buffer, for checking when projectiles exit the board
    public static GameControl control; // remove this later
    
    /// <summary>
    /// Checks if the object position is outside the game board
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static bool OutOfBounds(Vector2 transform)
    {
        if (bounds.Contains(transform)) return false;
        return true;
    }

    #region WaveControl

    #endregion

}
