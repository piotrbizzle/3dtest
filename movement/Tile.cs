using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // all 0s {0,0,0,0,0,0,0,0,0},
    // all 1s {1,1,1,1,1,1,1,1,1}
    private static Dictionary<string, int[,,]> TileMaps = new Dictionary<string, int[,,]>(){
	{"0_0_democellmap", new int[,,]{
	    {{1,1,1,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1}},		
	    {{1,1,1,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,1,1,1,1,1,1}},
	    }},
	{"1_0_democellmap", new int[,,]{
	    {{1,0,0,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,1,1,1,0,0,1}},
	    {{1,0,0,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,0,0,0,0,1},
	     {1,1,1,1,0,0,0,0,1},
	     {1,1,1,1,1,1,0,0,1}},
	    {{1,0,0,1,1,1,1,1,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,0,0,0,0,0,0,0,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,0,0,1,1,1,1,1,1},
	     {1,0,0,0,0,0,1,1,1},
	     {1,0,0,0,0,0,1,1,1},
	     {1,0,0,0,0,0,1,1,1},
	     {1,0,0,0,0,0,1,1,1},
	     {1,0,0,0,0,0,1,1,1},
	     {1,1,1,1,1,1,1,1,1}},
	    {{1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1},
	     {1,1,1,1,1,1,1,1,1}},
	    }},
	 {"2_0_democellmap", new int[,,]{
	     {{1,0,0,1,1,1,0,0,1},
	      {1,0,0,0,0,0,0,0,1},
	      {1,0,0,0,0,0,0,0,1},
	      {1,1,1,1,1,1,1,1,1},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0}},
	     {{1,1,1,1,1,1,0,0,1},
	      {1,1,1,0,0,0,0,0,1},
	      {1,1,1,0,0,0,0,0,1},
	      {1,1,1,1,1,1,1,1,1},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0}},
	     {{1,0,0,1,1,1,1,1,1},
	      {1,0,0,1,1,1,1,1,1},
	      {1,0,0,1,1,1,1,1,1},
	      {1,1,1,1,1,1,1,1,1},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0},
	      {0,0,0,0,0,0,0,0,0}},
	     {{1,0,0,1,1,1,0,0,1},
	      {1,0,0,0,0,0,0,0,1},
	      {1,0,0,0,0,0,0,0,1},
	      {1,1,1,1,1,1,1,1,1},
	      {1,1,1,1,1,1,1,1,1},
	      {1,1,1,1,1,1,1,1,1},
	      {1,1,1,1,1,1,1,1,1}},
	     {{1,1,1,1,1,1,1,1,1},
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
	return map[depth, j, i];
    }
}
