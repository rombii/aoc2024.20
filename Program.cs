var input = await File.ReadAllLinesAsync(Path.Join(Directory.GetCurrentDirectory(), "input.txt"));
(int, int) startPosition = (0, 0), endPosition = (0, 0);

for (var i = 0; i < input.Length; i++)
{
    for (var j = 0; j < input[i].Length; j++)
    {
        if(input[i][j] == 'S') startPosition = (i, j);
        else if(input[i][j] == 'E') endPosition = (i, j);
    }
}

var results = CountCheatsBetterThan100(startPosition);
Console.WriteLine($"First part: {results.Item1}");
Console.WriteLine($"Second part: {results.Item2}");
return;

(int, int) CountCheatsBetterThan100((int, int) start)
{
    var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
   
    var distances = new int[input.Length, input[0].Length];
    for (var i = 0; i < distances.GetLength(0); i++)
    {
        for (var j = 0; j < distances.GetLength(1); j++)
        {
            distances[i, j] = -1;
        }
    }

    var pq = new SortedSet<(int dist, int r, int c)>();
    distances[start.Item1, start.Item2] = 0;
    pq.Add((0, start.Item1, start.Item2));

    while (pq.Count > 0)
    {
        var (current, r, c) = pq.Min;
        pq.Remove(pq.Min);

        for (var i = 0; i < directions.Length; i++)
        {
            var newR = r + directions[i].Item1;
            var newC = c + directions[i].Item2;

            if (newR >= 0 && newR < input.Length && newC >= 0 && newC < input[0].Length && input[newR][newC] != '#')
            {
                var newDistance = current + 1;
                if (newDistance < distances[newR, newC] || distances[newR, newC] == -1)
                {
                    pq.Remove((distances[newR, newC], newR, newC));
                    distances[newR, newC] = newDistance;
                    pq.Add((newDistance, newR, newC));
                }
            }
        }
    }

    var directionsToCheat = new[] { (2, 0), (1, 1), (0, 2), (-1, 1) };
    var part1Count = 0;
    for (var r = 0; r < distances.GetLength(0); r++)
    {
        for (var c = 0; c < distances.GetLength(1); c++)
        {
            if(input[r][c] == '#') continue;
            for (var k = 0; k < directionsToCheat.Length; k++)
            {
                var newR = r + directionsToCheat[k].Item1;
                var newC = c + directionsToCheat[k].Item2;
                if(newR < 0 || newR > directions.GetLength(0)) continue;
                if(newC < 0 || newC > directions.GetLength(1)) continue;
                if(input[newR][newC] == '#') continue;
                if (Math.Abs(distances[newR, newC] - distances[r, c]) >= 102)
                {
                    part1Count++;
                }
            }
        }
    }
    
    
    var part2Count = 0;
    for (var r = 0; r < distances.GetLength(0); r++)
    {
        for (int c = 0; c < distances.GetLength(1); c++)
        {
            if(input[r][c] == '#') continue;
            for (var dr = -20; dr <= 20; dr++)
            {
                for (var dc = -20; dc <= 20; dc++)
                {
                    if(Math.Abs(dr) + Math.Abs(dc) > 20) continue;
                    if(r + dr < 0 || r + dr >= distances.GetLength(0)) continue;
                    if(c + dc < 0 || c + dc >= distances.GetLength(1)) continue;
                    if (input[r + dr][c + dc] == '#') continue;
                    if (distances[r + dr, c + dc] - distances[r, c] >= 100 + Math.Abs(dr) + Math.Abs(dc))
                    {
                        part2Count++;
                    }
                    
                }
            }
        }
    }

    return (part1Count, part2Count);
}
