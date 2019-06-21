using System;
using System.Collections;
using UnityEngine;

public class HexagonHandler : MonoBehaviour
{
    public bool isGenerator;

    public SpriteRenderer sr;
    public int team;

    public bool deactivated;
    
    private Collider2D[] colliders = new Collider2D[2];

    private void Update()
    {
        if (!deactivated && team != 0 && MainHandler.TurnHandler.turn != team) Deactivate();
        else if (deactivated && MainHandler.TurnHandler.turn == team) Activate();
    }
    
    private void Activate()
    {
        sr.color = new Color(1,1,1);
        deactivated = false;
        if (isGenerator) StartCoroutine(SpawnPawn());
    }

    private void Deactivate()
    {
        sr.color = new Color(0.5f,0.5f,0.5f);
        deactivated = true;
        //if (isGenerator) StartCoroutine(SpawnPawn());
    }

    public void ChangeTeam(int newTeam)
    {
        switch (newTeam)
        {
            case 0:
                sr.sprite = isGenerator ? MainHandler.Resources.generatorHexagonSprite : MainHandler.Resources.hexagonSprite;
                break;
            case 1:
                sr.sprite = isGenerator ? MainHandler.Resources.blueGeneratorHexagonSprite : MainHandler.Resources.blueHexagonSprite;
                break;
            case 2:
                sr.sprite = isGenerator ? MainHandler.Resources.redGeneratorHexagonSprite : MainHandler.Resources.redHexagonSprite;
                break;
        }
        
        team = newTeam;
    }

    private IEnumerator SpawnPawn()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        
        if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) > 1) yield break;
        var collidersAround = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        
        foreach (var col in collidersAround)
        {
            if (col.CompareTag("Hexagon")) continue;
            if (col.GetComponent<PawnHandler>() == null || col.GetComponent<PawnHandler>().team != team) yield break;
        }

        var spawnPosition = transform.position;
        spawnPosition.z = -1;
        Instantiate(team == 1 ? MainHandler.Resources.BlueSwordsMan : MainHandler.Resources.RedSwordsMan, spawnPosition, Quaternion.identity).transform.SetParent(MainHandler.Resources.Grid, true);
    }
    
}
