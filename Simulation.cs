using System.Text;

namespace GameOfLife;

public class Simulation
{
    private HashSet<(long, long)> aliveCells = new();
    private const string Header = "#Life 1.06";

    public Simulation()
    {
    }

    public Simulation(string inputState)
    {
        string[] lines = inputState.Split('\n');
        if (lines.Length > 0 && lines[0] == Header)
        {
            for (int i = 1 ; i < lines.Length ; i++)
            {
                string[] coordinates = lines[i].Split(' ');
                if (coordinates.Length == 2 &&
                    long.TryParse(coordinates[0], out long x) &&
                    long.TryParse(coordinates[1], out long y))
                {
                    aliveCells.Add((x, y));
                }
            }
        }
    }

    public void Run(int numSteps)
    {
        for (int i = 0 ; i < numSteps ; i++)
        {
            Step();
        }
    }

    private void Step()
    {
        Dictionary<(long, long), int> proximityCells = new();

        void AddProximity((long, long) key, bool increment)
        {
            if (!proximityCells.TryGetValue(key, out int proximity))
            {
                proximityCells[key] = proximity = 0;
            }

            if (increment)
            {
                proximityCells[key] = proximity + 1;
            }
        }
        
        // Mark all cells around living cells as active in the simulation
        foreach ((long startX, long startY) in aliveCells)
        {
            for (long xd = -1 ; xd <= 1 ; xd++)
            {
                long x = Offset(startX, xd);
                for (long yd = -1 ; yd <= 1 ; yd++)
                {
                    long y = Offset(startY, yd);
                    AddProximity((x, y), xd != 0 || yd != 0);
                }
            }
        }

        // Kill living cells and revive dead cells that grew this step
        foreach (((long, long) key, int proximity) in proximityCells)
        {
            if (proximity == 3 || (aliveCells.Contains(key) && proximity == 2))
            {
                aliveCells.Add(key);
            }
            else
            {
                aliveCells.Remove(key);
            }
        }
    }

    private static long Offset(long input, long delta)
    {
        return delta switch
        {
            -1 when input == long.MinValue => long.MaxValue,
            1 when input == long.MaxValue => long.MinValue,
            _ => input + delta
        };
    }

    public string GetState()
    {
        // No new-line at EOF
        StringBuilder sb = new(Header);
        foreach ((long x, long y) in aliveCells)
        {
            sb.AppendLine();
            sb.Append($"{x} {y}");
        }

        return sb.ToString();
    }
}
