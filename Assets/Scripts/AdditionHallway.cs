using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Tile { Wall, Slime };

public class AdditionHallway : MonoBehaviour
{
    int x = 20;
    int y = 20;
    int start_x = 9;
    int start_y = 18;
    List<Tile>[,] grid;

    void Start()
    {
        grid = new List<Tile>[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i != start_x && j != start_y)
                {
                    grid[i, j] = new List<Tile> { Tile.Wall };
                }

            }
        }

        List<int[]> walls = new List<int[]> { new int[] { start_x, start_y - 1 }, new int[] { start_x - 1, start_y }, new int[] { start_x + 1, start_y } };
        List<int[]> visited = new List<int[]> { new int[] { start_x, start_y } };
        while (walls.Count > 0)
        {
            int[] wall = walls[Random.Range(0, walls.Count - 1)];
            int count = 0;
            if (wall[0] > 1 && visited.Any<int[]>(e => e[0] == wall[0] - 1 && e[1] == wall[1]))
            {
                count += 1;
            }
            if (wall[0] < 18 && visited.Any<int[]>(e => e[0] == wall[0] + 1 && e[1] == wall[1]))
            {
                count += 1;
            }
            if (wall[1] > 1 && visited.Any<int[]>(e => e[0] == wall[0] && e[1] == wall[1] - 1))
            {
                count += 1;
            }
            if (wall[1] < 18 && visited.Any<int[]>(e => e[0] == wall[0] && e[1] == wall[1] + 1))
            {
                count += 1;
            }

            if (count < 2)
            {
                grid[wall[0], wall[1]] = new List<Tile> { };
                visited.Add(wall);
                if (wall[0] > 1)
                {
                    walls.Add(new int[] { wall[0] - 1, wall[1] });
                }
                if (wall[0] < 18)
                {
                    walls.Add(new int[] { wall[0] + 1, wall[1] });
                }
                if (wall[1] > 1)
                {
                    walls.Add(new int[] { wall[0], wall[1] - 1 });
                }
                if (wall[1] < 18)
                {
                    walls.Add(new int[] { wall[0], wall[1] + 1 });
                }
            }
            walls.RemoveAll(e => e[0] == wall[0] && e[1] == wall[1]);
        }

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i != start_x && j != start_y)
                {
                    if (grid[i, j].Count > 0)
                    {
                        Debug.Log(grid[i, j][0]);
                    }
                    else
                    {
                        Debug.Log(grid[i, j]);
                    }
                }

            }
        }
    }

    void Update()
    {

    }
}
