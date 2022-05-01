using System.Text;

namespace GameOfLife;

public class Simulation
{
    private class Proximity
    {
        public int counter;
        public bool processed;
    }
    
    private List<(long, long)> aliveCells = new();
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
        Dictionary<(long, long), Proximity?> proximityCells = new();

        void AddProximity((long, long) key)
        {
            if (!proximityCells.TryGetValue(key, out Proximity? proximity))
            {
                proximityCells[key] = proximity = new Proximity { counter = 0, processed = false };
            }

            if (proximity is not null)
            {
                proximity.counter++;
            }
        }
        
        foreach ((long startX, long startY) in aliveCells)
        {
            for (long xd = -1 ; xd <= 1 ; xd++)
            {
                long x = Offset(startX, xd);
                for (long yd = -1 ; yd <= 1 ; yd++)
                {
                    if (xd == 0 && yd == 0) continue;
                    
                    long y = Offset(startY, yd);
                    AddProximity((x, y));
                }
            }
        }

        for (int i = 0 ; i < aliveCells.Count ; i++)
        {
            if (!proximityCells.TryGetValue(aliveCells[i], out Proximity? proximity) ||
                proximity?.counter is < 2 or > 3)
            {
                aliveCells.RemoveAt(i--);
            }
            else if (proximity is not null)
            {
                proximity.processed = true;
            }
        }

        foreach (((long x, long y), Proximity? proximity) in proximityCells)
        {
            if (proximity?.processed is false && proximity.counter == 3)
            {
                aliveCells.Add((x, y));
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
