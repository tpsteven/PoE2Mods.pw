using Patchwork;

using Game;
using Game.GameData;
using Onyx;

namespace ShipMechanicsMod
{
    [ModifiesType]
    class mod_Conditionals
    {
        [ModifiesMember("IsAtShipRetreatDistance")]
        public static bool mod_IsAtShipRetreatDistance()
        {
            ShipDuelState state = SingletonBehavior<ShipDuelManager>.Instance.CurrentState;

            return state.IsFirstTurnOfRound 
                   || state.Distance >= ShipDuelSettingsGameData.Instance.RetreatDistance;

            // return SingletonBehavior<ShipDuelManager>.Instance.CurrentState.Distance >= ShipDuelSettingsGameData.Instance.RetreatDistance;
        }
    }
}
