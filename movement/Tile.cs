using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Dictionary<string, int[,,]> TileMaps = new Dictionary<string, int[,,]>(){
	{"0_0_democellmap", new int[,,]{
	    {{1,1,1,1,1,1,1,1,1}, // 0
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1}, // 1
	     {1,0,0,1,1,1,0,0,1},
	     {1,0,0,1,1,1,0,0,1},
	     {1,0,0,1,1,1,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1}, // 2
	     {1,0,1,0,0,0,1,0,1},
	     {1,0,1,0,0,0,1,0,1},
	     {1,0,0,1,1,1,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1}, // 3
	     {1,1,1,0,0,0,1,1,1},
	     {1,1,1,0,0,0,1,1,1},
	     {1,0,0,1,1,1,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1}, // 4
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1}},	 
	    }},
    };

    public static int TestCoordinate(string tilename, int depth, float x, float y) {
	int i = Math.Min(8, Math.Max(0, (int)Math.Floor((x + 0.6f) / 1.2) + 4));
	int j = 6 - Math.Min(6, Math.Max(0, (int)Math.Floor((y + 0.6f) / 1.2) + 3));
	int[,,] map = TileMaps[tilename];
	Debug.Log("---");
	Debug.Log(depth);
	Debug.Log(i);
	Debug.Log(j);
	return map[depth, j, i];
    }
}
