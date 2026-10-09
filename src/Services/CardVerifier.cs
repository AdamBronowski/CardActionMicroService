using CardActionService.Contracts;

namespace CardActionService.Services
{
    public class CardVerifier
    {
        public string ActionName
        {
            get; private set;
        }

        private IEnumerable<CardType>? cardTypes;
        private readonly IList<(CardStatus status, bool? pinIsSet)> cardStatuses = new List<(CardStatus, bool?)>();
        private bool allowsAllCardTypes = false;
        private bool allowsAllCardStatuses = false;

        private CardVerifier(string actionName)
        {
            ActionName = actionName;
        }

        public static CardVerifier ForAction(string actionName)
        {
            return new CardVerifier(actionName);
        }

        public bool VerifyForAction(CardDetails cardDetails)
        {
            if(!allowsAllCardTypes && !cardTypes!.Contains(cardDetails.CardType))
            {
                return false;
            }

            if(allowsAllCardStatuses)
            {
                return true;
            }

            return cardStatuses.Any(s => s.status == cardDetails.CardStatus
                &&
                (!s.pinIsSet.HasValue || s.pinIsSet == cardDetails.IsPinSet));
        }


        public CardVerifier WithAllowedTypes(params CardType[] allowedTypes)
        {
            cardTypes = allowedTypes;

            return this;
        }

        public CardVerifier WhichAllowsAllTypes()
        { 
            allowsAllCardTypes = true;

            return this;
        }

        public CardVerifier WithAllowedStatuses(params CardStatus[] allowedStatuses)
        {
            foreach (var status in allowedStatuses)
            {
                cardStatuses.Add((status, null));
            }

            return this;
        }

        public CardVerifier WithAllowedStatus(CardStatus allowedStatus, bool? requiredPinState = null)
        {
            cardStatuses.Add((allowedStatus, requiredPinState));

            return this;
        }

        public CardVerifier WhichAllowsAllStatuses()
        {
            allowsAllCardStatuses = true;

            return this;
        }
    }
}
