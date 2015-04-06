using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
    public static event GameEvent GameStart, GameOver;
    
    public delegate void ScoreEvent(long total, int value, Vector3 position);
    public static event ScoreEvent UpdatePoints;
    public static long points = 0;
    
    public delegate void ItemEvent(string name);
    public static event ItemEvent StartItem;
    
    public delegate void SceneEvent(Vector3 position);
    public static event SceneEvent NewColumn;
    
    public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
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
    
    public static void AddItem(string name){
        if(StartItem != null) {
            StartItem(name);
        }
	}
    
}