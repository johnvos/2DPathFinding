using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode : IComparable {

    public bool Visited;
    public float Cost;
    public float Heuristic;

    readonly Vector2 position;
    List<APath> paths;
    ANode previousNode;

    public ANode(Vector2 position) {
        this.position = position;
        paths = new List<APath>();

        Visited = false;
        Cost = int.MaxValue;
        Heuristic = int.MaxValue;
        previousNode = null;
    }
    
    public ANode PreviousNode {
        get { return this.previousNode; }
    }

    public Vector2 Position {
        get { return this.position; }
    }

    public void UpdateNodeCost(float cost, ANode prev) {
        this.Cost = cost;
        this.previousNode = prev;
    }

    public void UpdateHeuristic(float heur) {
        this.Heuristic = heur;
    }

    public void SetPaths(List<APath> paths) {
        this.paths = paths;
    }

    public void AddPaths(APath path) {
        this.paths.Add(path);
    }

    public List<APath> GetPaths() {
        return this.paths;
    }

    public int CompareTo(object obj) {
        ANode other = obj as ANode;
        if (this.Cost + this.Heuristic > other.Cost + other.Heuristic) {
            return 1;
        } else if (this.Cost + this.Heuristic < other.Cost + other.Heuristic) {
            return -1;
        } else {
            return 0;
        }

    }
}

public class APath {

    readonly ANode A;
    readonly ANode B;
    readonly float cost;

    public APath(ANode a, ANode b, float cost) {
        this.A = a;
        this.B = b;
        this.cost = cost;

        a.AddPaths(this);
        b.AddPaths(this);
    }

    public float Cost {
        get { return this.cost; }
    }

    public ANode GetOtherSide(ANode a) {
        if (this.A.Equals(a)) {
            return this.B;
        }
        if (this.B.Equals(a)) {
            return this.A;
        }

        throw new Exception("This path does not contain the node " + a + ", check your graph definition code again.");
    }

}



class PQueue {

    List<ANode> queue;

    public PQueue() {
        this.queue = new List<ANode>();
    }

    public void Enqueue(ANode a) {
        queue.Add(a);
        queue.Sort();
    }

    public ANode Dequeue() {
        if (queue.Count < 0) return null;
        ANode a = queue[0];
        queue.RemoveAt(0);
        return a;
    }



}


public class AStar {
    List<ANode> nodes;
    List<APath> paths;

    List<ANode> pathFound;

    public AStar(List<ANode> nodes, List<APath> paths) {
        this.nodes = nodes;
        this.paths = paths;
        this.pathFound = new List<ANode>();
    }

    public bool FindPath(ANode root, ANode dest) {

        PQueue queue = new PQueue();

        if (!nodes.Contains(root)) {
            nodes.Add(root);
            foreach (APath x in root.GetPaths()) paths.Add(x);
        }

        if (!nodes.Contains(dest)) {
            nodes.Add(dest);
            foreach (APath x in dest.GetPaths()) paths.Add(x);
        }

        ANode currentNode = root;
        currentNode.Cost = 0;

        List<ANode> notVisited = new List<ANode>(nodes);

        while(notVisited.Count > 0) {
            foreach (APath x in currentNode.GetPaths()) {
                ANode neighbor = x.GetOtherSide(currentNode);
                if (neighbor.Visited) {
                    continue;
                }

                neighbor.UpdateHeuristic(GetHeuristicOf(neighbor, dest));

                if(neighbor.Cost > currentNode.Cost + x.Cost) { 
                    neighbor.UpdateNodeCost(currentNode.Cost + x.Cost, currentNode);
                }

                queue.Enqueue(neighbor);
            }
            currentNode.Visited = true;
            notVisited.Remove(currentNode);
            if (notVisited.Count == 0) break;
            currentNode = queue.Dequeue();
            if (currentNode.Equals(dest)) {
                UpdatePath(root, dest);
                return true;
            }
        }

        return false;
    }

    float GetHeuristicOf(ANode a, ANode dest) {
        return Vector2.Distance(a.Position, dest.Position);
    }

    void UpdatePath(ANode root, ANode dest) {
        ANode currentNode = dest;
        while (currentNode != root) {
            pathFound.Add(currentNode);
            currentNode = currentNode.PreviousNode;
        }
        pathFound.Add(root);
    }

    public void PrintState() {
        foreach (ANode x in this.nodes) {
            Debug.Log("Node " + x + " has cost " + x.Cost);
        }
    }

    public void PrintPath() {
        foreach(ANode x in this.pathFound) {
            Debug.Log(x.Position);
        }
    }

    public void Reset() {
        foreach (ANode x in nodes) {
            x.Cost = int.MaxValue;
            x.Visited = false;
        }
    }



}
