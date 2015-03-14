using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
    public static event GameEvent GameStart, GameOver;
    
    public delegate void ScoreEvent(int value);
    public static event ScoreEvent UpdatePoints;
    static int points = 0;
    
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
    
    public static void AddPoints(int value){
		points += value;
        if(UpdatePoints != null) {
            UpdatePoints(points);
        }
	}
    
    public static void AddNewColumn(Vector3 position){
		NewColumn(position);
	}
    
}