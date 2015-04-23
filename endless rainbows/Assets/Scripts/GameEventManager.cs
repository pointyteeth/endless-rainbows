using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public static class GameEventManager {

	public delegate void GameEvent();
    public static event GameEvent GameStart, GameOver;
    public static bool game = false;
    
    public delegate void ScoreEvent(long total, int value, Vector3 position);
    public static event ScoreEvent UpdatePoints;
    public static long points = 0;
    
    public delegate void ItemEvent(string name);
    public static event ItemEvent StartItem;
    
    public delegate void ColumnEvent(Vector3 position);
    public static event ColumnEvent NewColumn;
    
    public delegate void SceneEvent();
    public static event SceneEvent EndScene, NewScene;
    public static bool scene = false;
    public static bool firstScene = true;
    
    public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();
		}
        game = true;
        firstScene = true;
        UpdatePoints(0, 0, Vector3.one*-10);
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
            game = false;
		}
	}
    
    public static void AddPoints(int value, Vector3 position){
		points += value;
        if(UpdatePoints != null) {
            UpdatePoints(points, value, position);
        }
	}
    
    public static void MultiplyPoints(long value){
		points *= value;
        if(UpdatePoints != null) {
            UpdatePoints(points, (int) value, new Vector3(0, 0, 0));
        }
	}
    
    public static void AddNewColumn(Vector3 position){
        if(NewColumn != null) {
            NewColumn(position);
        }
	}
    
    public static void EndedScene(){
        EndScene();
        UnityAnalytics.CustomEvent("Scene End", new Dictionary<string, object> {
            { "points", points },
            //{ "distance", distance }
        } );
        scene = false;
	}
    
    public static void AddedNewScene(){
        NewScene();
        scene = true;
        firstScene = false;
	}
    
    public static void AddItem(string name){
        if(StartItem != null) {
            StartItem(name);
        }
	}
    
}