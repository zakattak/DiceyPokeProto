﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonEntity : MonoBehaviour {

    // display & graphics
    public Texture2D sprite;

    // mon prototype
    public string monName;
    public int maxHealth;
    public int maxEnergy;
    public GameObject nextEvolutionPrefab;
    public int nextEvolutionLevel;
    public List<string> learnableMovesConfig; //formatted "<MoveName>,<LevelToLearn>"
    List<KeyValuePair<int, Move>> learnableMoves = new List<KeyValuePair<int, Move>>();

    // mon instance
    public int currentLevel = 1;
    public int currentHealth;
    public int currentEnergy;
    public int remainingActions = 1;
    public List<Move> activeMoves;

    public void constructMoves() {
        foreach (string learnableMoveConfig in learnableMovesConfig) {
            var moveName = learnableMoveConfig.Split(',')[0];
            var level = int.Parse(learnableMoveConfig.Split(',')[1]);

            var constructedMove = new Move().getMoveByName(moveName);
            learnableMoves.Add(new KeyValuePair<int, Move>(level, constructedMove));
            activeMoves = learnableMoves.Where(pair => pair.Key <= currentLevel).Select(pair => pair.Value).ToList();
        }
    }

}