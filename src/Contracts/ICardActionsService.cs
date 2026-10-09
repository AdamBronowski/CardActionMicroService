namespace CardActionService.Contracts
{
    public interface ICardActionsService
    {
        ICollection<string> GetAllowedActions(CardDetails cardDetails);
    }
}
