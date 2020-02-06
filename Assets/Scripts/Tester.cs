using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<ANode> nodes = new List<ANode>();
        List<APath> paths = new List<APath>();

        ANode a = new ANode(new Vector2(0, 0));
        ANode b = new ANode(new Vector2(1, 0));
        ANode c = new ANode(new Vector2(0.5f, -0.5f));
        ANode d = new ANode(new Vector2(0, -1));
        ANode e = new ANode(new Vector2(-1, -1));

        nodes.Add(a);
        nodes.Add(b);
        nodes.Add(c);
        nodes.Add(d);
        nodes.Add(e);

        APath ab = new APath(a, b, 2);
        APath ad = new APath(a, d, 1);
        APath bc = new APath(b, c, 1);
        APath ce = new APath(c, e, 1);
        APath de = new APath(d, e, 5);
        APath be = new APath(b, e, 3);

        paths.Add(ab);
        paths.Add(ad);
        paths.Add(bc);
        paths.Add(ce);
        paths.Add(de);
        paths.Add(be);

        AStar test = new AStar(nodes, paths);

        test.PrintState();

        if(test.FindPath(a, e)) {
            test.PrintPath();
        }

        test.PrintState();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
