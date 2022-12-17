using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Tile { Wall, Slime };

public class AdditionHallway : MonoBehaviour
{
    public GameObject wall;

    int x = 11;
    int y = 11;
    int start_x = 1;
    int start_y = 1;
    List<Tile>[,] grid;

    void Start()
    {
        grid = new List<Tile>[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i == start_x && j == start_y)
                {
                    grid[i, j] = new List<Tile> { };

                }
                else
                {
                    grid[i, j] = new List<Tile> { Tile.Wall };
                }

            }
        }

        List<int[]> walls = new List<int[]> { new int[] { start_x + 1, start_y + 1 }, new int[] { start_x, start_y + 1 } };
        List<int[]> visited = new List<int[]> { new int[] { start_x, start_y } };
        while (walls.Count > 0)
        {
            int[] wall = walls[Random.Range(0, walls.Count - 1)];
            int num_visited = 0;
            if (wall[0] > 1 && visited.Any<int[]>(e => e[0] == wall[0] - 1 && e[1] == wall[1]))
            {
                num_visited += 1;
            }
            if (wall[0] < 9 && visited.Any<int[]>(e => e[0] == wall[0] + 1 && e[1] == wall[1]))
            {
                num_visited += 1;
            }
            if (wall[1] > 1 && visited.Any<int[]>(e => e[0] == wall[0] && e[1] == wall[1] - 1))
            {
                num_visited += 1;
            }
            if (wall[1] < 9 && visited.Any<int[]>(e => e[0] == wall[0] && e[1] == wall[1] + 1))
            {
                num_visited += 1;
            }

            if (num_visited == 1)
            {
                grid[wall[0], wall[1]] = new List<Tile> { };
                visited.Add(wall);
                if (wall[0] > 1)
                {
                    walls.Add(new int[] { wall[0] - 1, wall[1] });
                }
                if (wall[0] < 9)
                {
                    walls.Add(new int[] { wall[0] + 1, wall[1] });
                }
                if (wall[1] > 1)
                {
                    walls.Add(new int[] { wall[0], wall[1] - 1 });
                }
                if (wall[1] < 9)
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
                if (grid[i, j].Contains(Tile.Wall))
                {
                    GameObject.Instantiate(wall, new Vector3((i - 5) * 4, 2, (j - 5) * 4), Quaternion.identity).transform.SetParent(this.transform);
                }
                else
                {
                    
                }
            }
        }
    }

    void Update()
    {

    }
}
