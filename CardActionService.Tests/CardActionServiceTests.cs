using CardActionService.Contracts;
using CardActionService.Services;
using System;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardActionService.Tests
{
    [TestClass]
    public sealed class CardActionServiceTests
    {
        [TestMethod]
        public void CardActionService_AllActionsHaveVerifiers()
        {
            var actions = typeof(ActionNames)
                .GetFields()
                .Where(f => f.IsPublic && f.IsStatic)
                .Where(f => f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null));

            var actionService = new CardActionsService();

            var verifiers = (List<CardVerifier>)
                typeof(CardActionsService)
                .GetField("_actionVerifiers", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(actionService)!;

            Assert.AreEqual(actions.Count(), verifiers.Count);

            foreach (var action in actions)
            {
                Assert.IsNotNull(verifiers.Single(v => v.ActionName == action));
            }
        }

        [TestMethod]
        [DataRow(CardType.Prepaid, CardStatus.Closed, false, "ACTION3", "ACTION4", "ACTION9")]
        [DataRow(CardType.Credit, CardStatus.Blocked, true, "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9")]
        [DataRow(CardType.Credit, CardStatus.Blocked, false, "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9")]
        [DataRow(CardType.Prepaid, CardStatus.Ordered, false, "ACTION3", "ACTION4", "ACTION7", "ACTION8",  "ACTION9", "ACTION10", "ACTION12", "ACTION13")]
        [DataRow(CardType.Debit, CardStatus.Restricted, true, "ACTION3", "ACTION4", "ACTION9")]
        public void CardActionService_ReturnsCorrectActions(CardType cardType, CardStatus cardStatus, bool pinIsSet, params string[] expectedActions)
        {
            var actionService = new CardActionsService();
            var testCard = new CardDetails("123", cardType, cardStatus, pinIsSet);

            var actions = actionService.GetAllowedActions(testCard);

            Assert.IsEmpty(actions.Except(expectedActions));
            Assert.IsEmpty(expectedActions.Except(actions));
        }
    }
}
