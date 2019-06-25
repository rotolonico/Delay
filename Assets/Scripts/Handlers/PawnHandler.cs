using System.Collections;
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
            if (!generated) Instantiate(team == 1 ? MainHandler.GameResources.blueHexagon : MainHandler.GameResources.redHexagon,
                transform.position, Quaternion.identity);
            thisTransform = transform;
            var currentPosition = thisTransform.position;
            currentPosition.z = -1;
            thisTransform.position = currentPosition;
            sr = GetComponent<SpriteRenderer>();
            protectedSr = thisTransform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!deactivated && MainHandler.TurnHandler.turn != team) Deactivate();
            else if (deactivated && MainHandler.TurnHandler.turn == team) Activate();
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
            if (deactivated) return;

            if (MainHandler.TileSelector.selectedPawn == thisTransform)
            {
                deactivatedSelector = true;
                MainHandler.TileSelector.DeactivateSelector();
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

            if (MainHandler.TileSelector.selectedPawn != thisTransform)
            {
                MainHandler.TileSelector.ActivateSelector(thisTransform);
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
            MainHandler.TileSelector.DoubleClick(thisTransform);
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
