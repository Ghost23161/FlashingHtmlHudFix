using System.Linq;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace FlashingHtmlHudFix
{
    public partial class FlashingHtmlHudFix : BasePlugin
    {
        public override string ModuleName => "FlashingHtmlHudFix";
        public override string ModuleVersion => "1.0";
        public override string ModuleAuthor => "Ghost";

        private CCSGameRulesProxy? _gameRulesProxy;

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnTick>(OnTick);
            RegisterListener<Listeners.OnMapStart>(OnMapStartHandler);
        }

        private void OnMapStartHandler(string mapName)
        {
            _gameRulesProxy = null;
        }

        private CCSGameRulesProxy? GetGameRulesProxy()
        {
            if (_gameRulesProxy == null || !_gameRulesProxy.IsValid)
            {
                _gameRulesProxy = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault();
            }

            return _gameRulesProxy;
        }

        private void OnTick()
        {
            var proxy = GetGameRulesProxy();

            if (proxy == null || !proxy.IsValid || proxy.GameRules == null) return;

            var gameRules = proxy.GameRules;

            if (gameRules.WarmupPeriod) return;

            bool expectedState = gameRules.RestartRoundTime < Server.CurrentTime;

            if (gameRules.GameRestart != expectedState)
            {
                gameRules.GameRestart = expectedState;
            }
        }
    }
}
