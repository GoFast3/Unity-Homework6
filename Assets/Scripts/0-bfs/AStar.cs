using System.Collections.Generic;
using System.Linq;


public class AStar {
    public static void FindPath<NodeType>(
            IGraph<NodeType> graph, 
            NodeType startNode, NodeType endNode, 
            List<NodeType> outputPath, int maxiterations=1000, 
            System.Func<NodeType, NodeType, float> heuristic = null)
    {
        if (heuristic == null)
        {
            heuristic = (a, b) => 0; // Default heuoristic
        }

        

        // Priority Queue for A* (sorted by f = g + h)
        var openSet = new Dictionary<NodeType, float>(); // Used to store nodes and their fScores
        var gScores = new Dictionary<NodeType, float>();
        var fScores = new Dictionary<NodeType, float>();
        var previous = new Dictionary<NodeType, NodeType>();

        openSet[startNode] = 0; // Starting node fScore = 0
        gScores[startNode] = 0;
        fScores[startNode] = heuristic(startNode, endNode);

        while (openSet.Count > 0 && maxiterations-- > 0)
        {
            // Get the node with the lowest fScore value
            var current = openSet.OrderBy(x => x.Value).First().Key;
            openSet.Remove(current);

            if (current.Equals(endNode))
            {
                // We found the target -- now construct the path
                outputPath.Add(endNode);
                while (previous.ContainsKey(current))
                {
                    current = previous[current];
                    outputPath.Add(current);
                }
                outputPath.Reverse();
                return;
            }

            foreach (var neighbor in graph.Neighbors(current))
            {
                
                float tentativeGScore = gScores[current] + graph.getWeight(neighbor); 
                if (!gScores.ContainsKey(neighbor) || tentativeGScore < gScores[neighbor])
                {
                    previous[neighbor] = current;
                    gScores[neighbor] = tentativeGScore;
                    fScores[neighbor] = gScores[neighbor] + heuristic(neighbor, endNode);

                    if (!openSet.ContainsKey(neighbor))
                    {
                        openSet.Add(neighbor, fScores[neighbor]);
                    }
                    else
                    {
                        openSet[neighbor] = fScores[neighbor]; // Update with better fScore
                    }
                }
            }
        }

        // If the loop ends without finding a path,it will output an empty path
        outputPath.Clear();
    }

    //An upgraded version of GetPath so it will work with the same call in TargetMover without changes
    public static List<NodeType> GetPath<NodeType>(IGraph<NodeType> graph, NodeType startNode, NodeType endNode, int maxiterations=1000, System.Func<NodeType, NodeType, float> heuristic = null) {
        List<NodeType> path = new List<NodeType>();
        FindPath(graph, startNode, endNode, path, maxiterations, heuristic);
        return path;
    }

}
