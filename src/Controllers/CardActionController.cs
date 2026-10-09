using CardActionService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace CardActionService.Controllers
{
    [ApiController]
    public class CardActionController : Controller
    {
        private ICardService _cardService;
        private ICardActionsService _cardActionsService;

        public CardActionController(ICardService cardService, ICardActionsService cardActionsService)
        {
            _cardService = cardService;
            _cardActionsService = cardActionsService;
        }

        [HttpGet("GetActionsForCard")]
        public async Task<IActionResult> GetCardActions(
            [BindRequired, RegularExpression("User\\d+")] string userId, 
            [BindRequired, RegularExpression("Card\\d{2,}")] string cardNumber)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);

            if (cardDetails == null)
            {
                return NotFound();
            }

            var result = _cardActionsService.GetAllowedActions(cardDetails);
         
            return Json(result);
        }
    }
}
