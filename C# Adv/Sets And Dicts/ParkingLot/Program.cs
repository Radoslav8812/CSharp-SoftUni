// See https://aka.ms/new-console-template for more information

var input = Console.ReadLine().Split();
var set = new HashSet<string>();

while (input[0] != "END")
{
    var direction = input[0];
    var number = input[1];

    if (direction == "IN" && !set.Contains(number))
    {
        set.Add(number);
    }
    else if (direction == "OUT" && set.Contains(number))
    {
        set.Remove(number);
    }

    input = Console.ReadLine().Split();
}

foreach (var item in set)
{
    if (set.Count != 0)
    {
        Console.WriteLine($"Number Still Inside: {item}");
    }
    else
    {
        Console.WriteLine("Our parking is empty.");
    }
}

