﻿using UnityEngine;
using System.Collections;
using VGDC_RPG;
using UnityEngine.UI;

public class ActionPanelScript : MonoBehaviour
{
    public bool isUnitMine = false;
    private RectTransform rt;

    private Button moveButton, attackButton;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
        moveButton = transform.FindChild("MoveButton").GetComponent<Button>();
        attackButton = transform.FindChild("AttackButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        isUnitMine = GameLogic.IsMyTurn;

        if (isUnitMine && rt.anchoredPosition.y < 0)
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y + 1);
        if (!isUnitMine && rt.anchoredPosition.y > -30)
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y - 1);

        if (isUnitMine)
        {
            var u = GameLogic.Units[GameLogic.CurrentPlayer][GameLogic.CurrentUnitID];

            if (u != null && !u.HasMoved)
                moveButton.interactable = true;
            else
                moveButton.interactable = false;

            if (u != null && !u.HasAttacked)
                attackButton.interactable = true;
            else
                attackButton.interactable = false;
        }
    }

    public void EndTurnPressed()
    {
        GameLogic.EndTurn();
    }

    public void MovePressed()
    {
        GameLogic.SetState(GameLogic.ActionState.Move);
    }
}
