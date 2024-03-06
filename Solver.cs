namespace MazeAI;

public class Solver
{
    public int Option { get; set; }
    public bool RogueMode { get; set; }
    public Maze Maze { get; set; }

    public string Algorithm
    {
        get
        {
            return (Option % 4) switch
            {
                0 => "DFS",
                1 => "BFS",
                2 => "dijkstra",
                _ => "aStar"
            };
        }
    }

    public void Solve()
    {
        var goal = Maze.Spaces.FirstOrDefault(s => s.Exit);

        switch (Option % 4)
        {
            case 0:
                DFS(Maze.Root, goal);
                break;
            case 1:
                BFS(Maze.Root, goal);
                break;
            case 2:
                Dijkstra(Maze.Root, goal);
                break;
            case 3:
                aStar(Maze.Root, goal);
                break;
        }
    }

    private static bool DFS(Space space, Space goal)
    {   
        if (space.Visited)
            return false;

        space.Visited = true;

        if (EqualityComparer<Space>.Default.Equals(space, goal))
        {
            space.IsSolution = true;
            return true;
        }

        space.IsSolution = (space.Left is not null && !space.Left.Visited && DFS(space.Left, goal))
                           || (space.Top is not null && !space.Top.Visited && DFS(space.Top, goal))
                           || (space.Right is not null && !space.Right.Visited && DFS(space.Right, goal))
                           || (space.Bottom is not null && !space.Bottom.Visited && DFS(space.Bottom, goal));

        return space.IsSolution;
    }

    private static bool BFS(Space space, Space goal)
    {
        var queue = new Queue<Space>();
        var prev = new Dictionary<Space, Space>();

        queue.Enqueue(space);

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue();

            if (currNode.Visited)
                continue;

            currNode.Visited = true;

            if (EqualityComparer<Space>.Default.Equals(currNode, goal))
            {
                currNode.IsSolution = true;
                break;
            }


            if (currNode.Left is not null && !prev.ContainsKey(currNode.Left))
            {
                prev[currNode.Left] = currNode;
                queue.Enqueue(currNode.Left);
            }

            if (currNode.Top is not null && !prev.ContainsKey(currNode.Top))
            {
                prev[currNode.Top] = currNode;
                queue.Enqueue(currNode.Top);
            }

            if (currNode.Right is not null && !prev.ContainsKey(currNode.Right))
            {
                prev[currNode.Right] = currNode;
                queue.Enqueue(currNode.Right);
            }

            if (currNode.Bottom is not null && !prev.ContainsKey(currNode.Bottom))
            {
                prev[currNode.Bottom] = currNode;
                queue.Enqueue(currNode.Bottom);
            }
        }

        var attempt = goal;
        while (attempt != space)
        {
            if (!prev.ContainsKey(attempt))
                return false;

            attempt.IsSolution = true;
            attempt = prev[attempt];
        }

        return false;
    }

    private static bool Dijkstra(Space space, Space goal)
    {
        var queue = new PriorityQueue<Space, float>();
        var dist = new Dictionary<Space, float>();
        var prev = new Dictionary<Space, Space>();

        queue.Enqueue(space, 0.0f);
        dist[space] = 0.0f;

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue();
            if (currNode == goal)
                break;

            var edge = currNode;

            if (currNode.Bottom is not null)
            {
                edge = currNode.Bottom;
                currNode.Bottom.Visited = true;

                var newWeight = dist[currNode] + 1;
                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }

            if (currNode.Top is not null)
            {
                edge = currNode.Top;
                currNode.Top.Visited = true;

                var newWeight = dist[currNode] + 1;
                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }

            if (currNode.Left is not null)
            {
                edge = currNode.Left;
                edge.Visited = true;
                var newWeight = dist[currNode] + 1;
                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }

            if (currNode.Right is not null)
            {
                edge = currNode.Right;
                edge.Visited = true;
                var newWeight = dist[currNode] + 1;
                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }
        }

        var attempt = goal;
        while (attempt != space)
        {
            if (!prev.ContainsKey(attempt))
                return false;

            attempt.IsSolution = true;
            attempt = prev[attempt];
        }
        attempt.IsSolution = true;

        return true;
    }

    private static bool aStar(Space space, Space goal)
    {
        return true;
    }
}