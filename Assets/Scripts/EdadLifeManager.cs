using UnityEngine;

public class EdadLifeManager : MonoBehaviour
{
    public int gridWidth = 20;
    public int gridHeight = 20;
    public int Amax = 3;
    public GameObject cellPrefab;

    private Cell[,] grid;

    void Start()
    {
        grid = new Cell[gridWidth, gridHeight];
        Sprite defaultSprite = cellPrefab.GetComponent<SpriteRenderer>()?.sprite;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 pos = new Vector2(x - gridWidth / 2, y - gridHeight / 2);
                GameObject obj = Instantiate(cellPrefab, pos, Quaternion.identity);

                // Usar GetComponent<Cell>() en vez de AddComponent
                Cell cellScript = obj.GetComponent<Cell>();
                if (cellScript == null)
                {
                    cellScript = obj.AddComponent<Cell>();
                }

                // Inicializar la celda con sprite por defecto
                cellScript.Init(Random.value > 0.7f, Amax, defaultSprite);
                grid[x, y] = cellScript;
            }
        }

        InvokeRepeating(nameof(UpdateGrid), 0.5f, 0.5f);
    }

    void UpdateGrid()
    {
        bool[,] nextState = new bool[gridWidth, gridHeight];
        int[,] nextAge = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int neighbors = CountNeighbors(x, y);
                Cell c = grid[x, y];

                if (!c.alive)
                {
                    if (neighbors == 3)
                    {
                        nextState[x, y] = true;
                        nextAge[x, y] = 1;
                    }
                }
                else
                {
                    if (c.age >= Amax)
                    {
                        nextState[x, y] = false;
                        nextAge[x, y] = 0;
                    }
                    else if (neighbors == 2 || neighbors == 3)
                    {
                        nextState[x, y] = true;
                        nextAge[x, y] = c.age + 1;
                    }
                    else
                    {
                        nextState[x, y] = false;
                        nextAge[x, y] = 0;
                    }
                }
            }
        }

        // Aplicar la siguiente generación
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].SetState(nextState[x, y], nextAge[x, y]);
            }
        }
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < gridWidth && ny >= 0 && ny < gridHeight)
                {
                    if (grid[nx, ny].alive) count++;
                }
            }
        }
        return count;
    }
}
