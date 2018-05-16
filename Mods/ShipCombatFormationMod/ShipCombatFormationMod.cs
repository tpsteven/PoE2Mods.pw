using Patchwork;

using UnityEngine;
using UnityEngine.AI;

using Game;
using Game.GameData;
using Onyx;

namespace ShipCombatFormationMod
{
    [ModifiesType]
    public class StartPointNew : StartPoint
    {
        [ModifiesMember("SpawnPartyHere")]
        public void SpawnPartyHereNew()
        {
            Vector3 markerPosition;
            NavMeshHit navMeshHit;
            foreach (PartyMember activePartyMember in SingletonBehavior<PartyManager>.Instance.GetActivePartyMembers())
            {
                if (activePartyMember != null)
                {
                    AIController component = activePartyMember.GetComponent<AIController>();
                    if (component != null)
                    {
                        Mover mover = component.Mover;
                        mover.enabled = false;
                        PartyWaypoint partyWaypoint = base.GetComponent<PartyWaypoint>();
                        if (GameState.Instance.IsAtSea)
                        {
                            // TODO: check that this condition is entered and that it's valid
                            // TODO: check what happens if you just visit your ship and you're not in combat
                            // TODO: see if rotation is related to player character index
                        }
                        else if (partyWaypoint == null || true)
                        {
                            // TODO: print waypoints, adjust manually if sea
                            SceneTransition sceneTransition = StartPoint.FindClosestSceneTransition(base.transform.position);
                            markerPosition = ((true || (sceneTransition == null || GameState.Instance.IsAtSea)) ? SingletonBehavior<PartyManager>.Instance.CalculateFormationPosition(component.GetComponent<PartyMember>(), base.transform.position, !GameState.Instance.IsAtSea) : sceneTransition.GetMarkerPosition(activePartyMember, true));
                        }
                        else
                        {
                            markerPosition = partyWaypoint.GetMarkerPosition(activePartyMember, false);
                        }
                        if (NavMeshUtility.SamplePosition(markerPosition, out navMeshHit, 100f, component.Mover))
                        {
                            markerPosition = navMeshHit.position;
                        }
                        component.transform.position = markerPosition;
                        component.transform.position = markerPosition;
                        component.transform.rotation = base.transform.rotation;
                        mover.enabled = true;
                        component.RecordRetreatPosition(component.transform.position);
                        component.transform.rotation = base.transform.rotation;
                        for (int i = 0; i < component.SummonedCreatureList.Count; i++)
                        {
                            if (component.SummonedCreatureList[i] != null)
                            {
                                AIController aIController = component.SummonedCreatureList[i].GetComponent<AIController>();
                                if (aIController && aIController.SummonType == AISummonType.Pet)
                                {
                                    Mover component1 = aIController.GetComponent<Mover>();
                                    aIController.transform.position = NavMeshUtility.NearestUnoccupiedLocation(component.transform.position, 10f, component1);
                                    aIController.BehaviorStack.PopAllBehaviors();
                                    aIController.RecordRetreatPosition(aIController.transform.position);
                                }
                            }
                        }
                    }
                }
            }
            CameraControl instance = SingletonBehavior<CameraControl>.Instance;
            if (instance)
            {
                instance.FocusOnPoint(base.transform.position, 0f);
            }
            GameCursor.DesiredCursor = GameCursor.CursorType.Normal;
        }
    }
}
