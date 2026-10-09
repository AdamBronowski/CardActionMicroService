using CardActionService.Contracts;

namespace CardActionService.Services
{
    public class CardActionsService : ICardActionsService
    {
        private readonly List<CardVerifier> _actionVerifiers = ConfigureAllowedActions();

        public ICollection<string> GetAllowedActions(CardDetails cardDetails)
        {
            return _actionVerifiers
                .Where(v => v.VerifyForAction(cardDetails))
                .Select(v => v.ActionName)
                .ToArray();
        }

        private static List<CardVerifier> ConfigureAllowedActions()
        {
            return new List<CardVerifier>()
            {
                CardVerifier
                    .ForAction(ActionNames.Action1)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatus(CardStatus.Active),

                CardVerifier
                    .ForAction(ActionNames.Action2)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatus(CardStatus.Inactive),

                CardVerifier
                    .ForAction(ActionNames.Action3)
                    .WhichAllowsAllTypes()
                    .WhichAllowsAllStatuses(),

                CardVerifier
                    .ForAction(ActionNames.Action4)
                    .WhichAllowsAllTypes()
                    .WhichAllowsAllStatuses(),

                CardVerifier
                    .ForAction(ActionNames.Action5)
                    .WithAllowedTypes(CardType.Credit)
                    .WhichAllowsAllStatuses(),

                CardVerifier
                    .ForAction(ActionNames.Action6)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatus(CardStatus.Ordered, requiredPinState: true)
                    .WithAllowedStatus(CardStatus.Inactive, requiredPinState: true)
                    .WithAllowedStatus(CardStatus.Active, requiredPinState: true)
                    .WithAllowedStatus(CardStatus.Blocked, requiredPinState: true),

                CardVerifier
                    .ForAction(ActionNames.Action7)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatus(CardStatus.Ordered, requiredPinState: false)
                    .WithAllowedStatus(CardStatus.Inactive, requiredPinState: false)
                    .WithAllowedStatus(CardStatus.Active, requiredPinState: false)
                    .WithAllowedStatus(CardStatus.Blocked, requiredPinState: true),

                CardVerifier
                    .ForAction(ActionNames.Action8)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked),

                CardVerifier
                    .ForAction(ActionNames.Action9)
                    .WhichAllowsAllTypes()
                    .WhichAllowsAllStatuses(),

                CardVerifier
                    .ForAction(ActionNames.Action10)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active),

                CardVerifier
                    .ForAction(ActionNames.Action11)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatuses(CardStatus.Inactive, CardStatus.Active),

                CardVerifier
                    .ForAction(ActionNames.Action12)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active),

                CardVerifier
                    .ForAction(ActionNames.Action13)
                    .WhichAllowsAllTypes()
                    .WithAllowedStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active)
            };
        }
    }
}
