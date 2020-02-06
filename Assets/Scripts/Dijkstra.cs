using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable{

    public bool Visited;
    public float Cost;

    readonly Vector2 position;
    List<Path> paths;
    Node previousNode;

    public Node(Vector2 position) {
        this.position = position;
        paths = new List<Path>();

        Visited = false;
        Cost = int.MaxValue;
        previousNode = null;
    }

    public Vector2 Position {
        get { return this.position; }
    }

    public void UpdateNodeCost(float cost, Node prev) {
        this.Cost = cost;
        this.previousNode = prev;
    }

    public void SetPaths(List<Path> paths) {
        this.paths = paths;
    }

    public void AddPaths(Path path) {
        this.paths.Add(path);
    }

    public List<Path> GetPaths() {
        return this.paths;
    }

    public int CompareTo(object obj) {
        Node other = obj as Node;
        if(this.Cost > other.Cost) {
            return 1;
        }else if(this.Cost < other.Cost) {
            return -1;
        } else {
            return 0;
        }

    }
}

public class Path {

    readonly Node A;
    readonly Node B;
    readonly float cost;

    public Path(Node a, Node b, float cost) {
        this.A = a;
        this.B = b;
        this.cost = cost;

        a.AddPaths(this);
        b.AddPaths(this);
    }

    public float Cost {
        get { return this.cost; }
    }

    public Node GetOtherSide(Node a) {
        if (this.A.Equals(a)) {
            return this.B;
        }
        if (this.B.Equals(a)) {
            return this.A;
        }

        throw new Exception("This path does not contain the node " + a + ", check your graph definition code again.");
    }

}


public class Dijkstra
{
    List<Node> nodes;
    List<Path> paths;

    public Dijkstra(List<Node> nodes, List<Path> paths) {
        this.nodes = nodes;
        this.paths = paths;
    }

    public void Start() {
        Node currentNode = nodes[0];
        currentNode.Cost = 0;

        List<Node> notVisited = new List<Node>(nodes);

        while(notVisited.Count > 0) {
            foreach(Path x in currentNode.GetPaths()) {
                Node neighbor = x.GetOtherSide(currentNode);
                if (neighbor.Visited) continue;
                if (neighbor.Cost > currentNode.Cost + x.Cost) {
                    neighbor.UpdateNodeCost(currentNode.Cost + x.Cost, currentNode);
                }
            }

            currentNode.Visited = true;
            notVisited.Remove(currentNode);
            notVisited.Sort();
            if (notVisited.Count == 0) break;
            currentNode = notVisited[0];

        }
            
            

    }

    public void PrintState() {
        foreach(Node x in this.nodes) {
            Debug.Log("Node " + x + " has cost " + x.Cost);
        }
    }

    public void Reset() {
        foreach(Node x in nodes) {
            x.Cost = int.MaxValue;
            x.Visited = false;
        }
    }



}
