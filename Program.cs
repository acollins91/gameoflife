// See https://aka.ms/new-console-template for more information

using System.Text;
using GameOfLife;

const int NumSimsToRun = 10;
const bool Debug = false;

Console.WriteLine("Input Option (pick one)");
Console.WriteLine("0: Line-by-line");
Console.WriteLine("1: File");
Console.WriteLine("2: Auto");

const string AutoInput = @"#Life 1.06
0 1
1 2
2 0
2 1
2 2
-2000000000000 -2000000000000
-2000000000001 -2000000000001
-2000000000000 -2000000000001";

string? optionStr = Console.ReadLine();
string inputState;
if (int.TryParse(optionStr, out int option))
{
    inputState = option switch
    {
        0 => LineByLineInput(),
        1 => FileInput(),
        2 => AutoInput,
        _ => throw new Exception()
    };
}
else
{
    return;
}

if (!string.IsNullOrEmpty(inputState))
{
    Console.WriteLine("Using Input:");
    Console.WriteLine(inputState);
    
    Simulation simulation = new(inputState);

    if (Debug)
    {
        for (int i = 0 ; i < NumSimsToRun ; i++)
        {
            simulation.Run(1);
            Console.WriteLine($"State after '{i}' simulations:");
            Console.WriteLine(simulation.GetState());
        }
    }
    else
    {
        simulation.Run(NumSimsToRun);
    }

    Console.WriteLine();
    Console.WriteLine($"State after '{NumSimsToRun}' simulations:");
    Console.WriteLine(simulation.GetState());
}
else
{
    Console.WriteLine("No input state found!");
}

string LineByLineInput()
{
    Console.WriteLine("Enter input coordinates (empty line when done):");

    StringBuilder sb = new();
    string? input;

    do
    {
        input = Console.ReadLine();
        sb.AppendLine(input);
    }
    while (!string.IsNullOrEmpty(input));

    return sb.ToString();
}

string FileInput()
{
    Console.WriteLine("Enter file path:");
    string? input = Console.ReadLine();
    input = $"../../../{input}";
    return File.Exists(input) ? File.ReadAllText(input) : string.Empty;
}
