// See https://aka.ms/new-console-template for more information


string input;

var vipsSet = new HashSet<string>();
var regularSet = new HashSet<string>();

while ((input = Console.ReadLine()) != "PARTY")
{
    if (!string.IsNullOrWhiteSpace(input) && char.IsDigit(input[0]))
    {
        vipsSet.Add(input);
    }
    else
    {
        regularSet.Add(input);
    }
}

while ((input = Console.ReadLine()) != "END")
{
    if (vipsSet.Contains(input))
    {
        vipsSet.Remove(input);
    }
    else if (regularSet.Contains(input))
    {
        regularSet.Remove(input);
    }
}

var total = vipsSet.Count + regularSet.Count;
Console.WriteLine($"Total: {total}");

foreach (var vip in vipsSet)
{
    Console.WriteLine(vip);
}

foreach (var reg in regularSet)
{
    Console.WriteLine(reg);
}