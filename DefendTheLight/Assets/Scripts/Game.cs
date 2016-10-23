using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Effect { None, SlowMovement };
public enum AbilityType { Red, Blue, Yellow, Orange, Green, Purple};

public static class Game
{
    public static GameGrid grid;
    public static int GameSpeed = 1;    // actual speed, pause makes this 0
    public static int Speed = 1;        // UI speed
    public static bool isPaused = false;
    public static int money = 1000;
    public static int energy = 200;
    public static List<Enemy> enemies;

    public static void GetPath()
    {
        
    }

}
