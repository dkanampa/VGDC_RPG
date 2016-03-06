﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VGDC_RPG.Players
{
    public class UserPlayer : Player
    {
        private enum UserChoice
        {
            Choosing,
            Move,
            Defend,
            Attack,
            EndTurn
        }

        private UserChoice choice;
        private bool canAttack;

        public override void Turn(int turn)
        {
            choice = UserChoice.Choosing;

            base.Turn(turn);

            canAttack = false;
            for (int i = 0; i < GameLogic.Instance.TeamCount; i++)
            {
                if (i == TeamID)
                    continue;
                foreach (var p in GameLogic.Instance.Players[i])
                    if (attackTiles != null && attackTiles.Contains(new Int2(p.X, p.Y)))
                    {
                        canAttack = true;
                        return;
                    }
            }
        }

        void OnGUI()
        {
            if (TakingTurn && GameLogic.Instance.CurrentGameState == GameLogic.GameState.SelectingStones && GameLogic.Instance.DoPlayerUpdates)
            {
                var buttonHeight = 30;
                var buttonWidth = 120;
                GUI.Label(new Rect(0, Screen.height - Stones.COUNT * buttonHeight - 50, 120, 20), GUIName);
                GUI.Label(new Rect(0, Screen.height - Stones.COUNT * buttonHeight - 20, 120, 20), "Select a stone:");
                for (int i = 0; i < Stones.COUNT; i++)
                    if (GUI.Button(new Rect(0, Screen.height - (Stones.COUNT - i) * buttonHeight, buttonWidth, buttonHeight), Stones.UIText[i + 1]))
                    {
                        SelectedStone = i + 1;
                        StoneSelected = true;
                    }
            }
            if (TakingTurn && choice == UserChoice.Choosing && GameLogic.Instance.CurrentGameState == GameLogic.GameState.Main && GameLogic.Instance.DoPlayerUpdates)
            {
                var buttonHeight = 60;
                var buttonWidth = 100;
                if (GUI.Button(new Rect((Screen.width - buttonWidth) / 2f, Screen.height / 2 - buttonHeight * 2, buttonWidth, buttonHeight), "Move"))
                    choice = UserChoice.Move;
                else if (canAttack && GUI.Button(new Rect((Screen.width - buttonWidth) / 2f, Screen.height / 2 - buttonHeight * 1, buttonWidth, buttonHeight), "Attack"))
                    choice = UserChoice.Attack;
                else if (!Defending && GUI.Button(new Rect((Screen.width - buttonWidth) / 2f, Screen.height / 2 - buttonHeight * 0, buttonWidth, buttonHeight), "Defend"))
                    choice = UserChoice.Defend;
                else if (GUI.Button(new Rect((Screen.width - buttonWidth) / 2f, Screen.height / 2 + buttonHeight * 1, buttonWidth, buttonHeight), "End Turn"))
                    choice = UserChoice.EndTurn;
            }
        }

        public override void Update()
        {
            if (this == GameLogic.Instance.CurrentPlayer && GameLogic.Instance.CurrentGameState == GameLogic.GameState.SelectingStones && StoneSelected)
            {
                TakingTurn = false;
                GameLogic.Instance.NextPlayer();
            }
            else
            {
                base.Update();
                if (TakingTurn && movementPath == null)
                {
                    switch (choice)
                    {
                        case UserChoice.Move:
                            if (Input.GetMouseButtonDown(0))
                            {

                                float x = Input.mousePosition.x;
                                float y = Input.mousePosition.y;


                                var t = GameLogic.Instance.GetScreenTile(x, y);

                                Debug.Log(t.X + ", " + t.Y);

                                if (possibleTiles.Contains(t))
                                    Move(Map.Pathfinding.AStarSearch.FindPath/*Map.Pathfinding.JumpPointSearch.FindPath*/(GameLogic.Instance.Map, new Int2(X, Y), t/*, false*/));
                            }
                            break;
                        case UserChoice.Attack:
                            if (Input.GetMouseButtonDown(0))
                            {

                                float x = Input.mousePosition.x;
                                float y = Input.mousePosition.y;
                                
                                var t = GameLogic.Instance.GetScreenTile(x, y);
                                if (attackTiles.Contains(t))
                                {
                                    for (int i = 0; i < GameLogic.Instance.TeamCount; i++)
                                    {
                                        if (i == TeamID)
                                            continue;
                                        foreach (var p in GameLogic.Instance.Players[i])
                                            if (p.X == t.X && p.Y == t.Y)
                                            {
                                                Attack(p);
                                                TakingTurn = false;
                                                GameLogic.Instance.NextTurn();
                                                return;
                                            }
                                    }
                                }
                            }
                            break;
                        case UserChoice.Defend:
                            Defending = true;
                            TakingTurn = false;
                            GameLogic.Instance.NextTurn();
                            break;
                        case UserChoice.EndTurn:
                            TakingTurn = false;
                            GameLogic.Instance.NextTurn();
                            break;
                    }
                }
            }
        }
    }
}
