using System.Collections;
using Globals;
using UnityEngine;

namespace Handlers
{
    public class PawnHandler : MonoBehaviour
    {
        public bool deactivated;
        public bool generated;
        public bool isProtected;

        public int team;
        public string pawnType;

        private Transform thisTransform;
        private SpriteRenderer sr;
        private SpriteRenderer protectedSr;

        private bool possibleDoubleClick;
        private bool deactivatedSelector;

        private void Start()
        {
            if (!generated) Instantiate(team == 1 ? GameHandler.GameResources.blueHexagon : GameHandler.GameResources.redHexagon,
                transform.position, Quaternion.identity).transform.SetParent(GameHandler.reference.grid.transform, true);
            thisTransform = transform;
            var currentPosition = thisTransform.position;
            currentPosition.z = -1;
            thisTransform.position = currentPosition;
            sr = GetComponent<SpriteRenderer>();
            protectedSr = thisTransform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!deactivated && GameHandler.TurnHandler.turn != team) Deactivate();
            else if (deactivated && GameHandler.TurnHandler.turn == team) Activate();
        }
    
        private void Activate()
        {
            sr.color = new Color(1,1,1);
            if (isProtected) UnProtect();
            deactivated = false;
        }

        private void Deactivate()
        {
            sr.color = new Color(0.5f,0.5f,0.5f);
            deactivated = true;
        }

        public void Protect()
        {
            isProtected = true;
            protectedSr.color = new Color(0.25f, 0.5f, 0.25f, 1);
        }

        private void UnProtect()
        {
            isProtected = false;
            protectedSr.color = new Color(0.25f, 0.5f, 0.25f, 0);
        }

        private void OnMouseDown()
        {
            if (Global.IsOnlineMatch && !Global.IsPlayerTurn) return;
            
            if (deactivated) return;

            if (GameHandler.TileSelector.selectedPawn == thisTransform)
            {
                deactivatedSelector = true;
                GameHandler.TileSelector.DeactivateSelector();
            }
        
            if (possibleDoubleClick)
            {
                DoubleClick();
                possibleDoubleClick = false;
                return;
            }

            StartCoroutine(DoubleClickInterval());
        
            if (deactivatedSelector)
            {
                deactivatedSelector = false;
                return;
            }

            if (GameHandler.TileSelector.selectedPawn != thisTransform)
            {
                GameHandler.TileSelector.ActivateSelector(thisTransform);
            }
        }

        private IEnumerator DoubleClickInterval()
        {
            possibleDoubleClick = true;
            yield return new WaitForSeconds(0.25f);
            possibleDoubleClick = false;
        }

        private void DoubleClick()
        {
            GameHandler.TileSelector.DoubleClick(thisTransform);
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            newPosition.z = -1;
            thisTransform.position = newPosition;
        
        
            var tile = Physics2D.OverlapCircle(newPosition, 0.1f);
            tile.GetComponent<HexagonHandler>().ChangeTeam(team);
        }
    }
}
