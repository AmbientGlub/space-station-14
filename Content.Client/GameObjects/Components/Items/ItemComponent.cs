using Content.Shared.GameObjects.Components.Inventory;
using SS14.Client.Graphics;
using SS14.Client.Interfaces.ResourceManagement;
using SS14.Client.ResourceManagement;
using SS14.Shared.GameObjects;
using SS14.Shared.GameObjects.Components.Renderable;
using SS14.Shared.IoC;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using SS14.Shared.ViewVariables;

namespace Content.Client.GameObjects
{
    public class ItemComponent : Component
    {
        public override string Name => "Item";

        [ViewVariables] protected ResourcePath RsiPath;

        private string _equippedPrefix;

        [ViewVariables(VVAccess.ReadWrite)]
        public string EquippedPrefix
        {
            get => _equippedPrefix;
            set => _equippedPrefix = value;
        }

        public (RSI rsi, RSI.StateId stateId)? GetInHandStateInfo(string hand)
        {
            if (RsiPath == null)
            {
                return null;
            }

            var rsi = GetRSI();
            var stateId = EquippedPrefix != null ? $"{EquippedPrefix}-inhand-{hand}" : $"inhand-{hand}";
            if (rsi.TryGetState(stateId, out _))
            {
                return (rsi, stateId);
            }

            return null;
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataFieldCached(ref RsiPath, "sprite", null);
            serializer.DataFieldCached(ref _equippedPrefix, "prefix", null);
        }

        protected RSI GetRSI()
        {
            var resourceCache = IoCManager.Resolve<IResourceCache>();
            return resourceCache.GetResource<RSIResource>(SharedSpriteComponent.TextureRoot / RsiPath).RSI;
        }
    }
}
