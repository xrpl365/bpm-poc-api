using XrplNftTicketing.Entities.DTOs.ImportPayloads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using XrplNftTicketing.Business.Interfaces;
using System;
using XrplNftTicketing.Business.Services;
using XrplNftTicketing.Entities.Configurations;
using XrplNftTicketing.Entities.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace XrplNftTicketing.Api.Controllers
{


    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventImportController : ControllerBase
    {

        private readonly IIpfsService _ipfsService;
        private readonly IXrplService _xrplService;
        private readonly XrplSettings _xrplSettings;
        private readonly ILogger<EventImportController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventImportController(IWebHostEnvironment webHostEnvironment, IOptions<XrplSettings> xrplSettings, IIpfsService ipfsService, IXrplService xrplService, ILogger<EventImportController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _xrplSettings = xrplSettings.Value;
            _ipfsService = ipfsService;
            _xrplService = xrplService;
            _logger = logger;
        }

        
        [HttpGet]
        public ActionResult HelloWorld()
        {
            return new JsonResult("Hello World");
        }
        

        [HttpPost("CreateEventTickets")]
        public async Task<ActionResult> CreateEventTickets(EventPayload eventPayload)
        {

            try
            {
                _logger.LogTrace("Create event called");

                // Call method to create tickets
                var resourcePath = $"{ _webHostEnvironment.ContentRootPath}{ @"\Content\Images\"}";
                var result = await XrplNfTokenCreationService.CreateNftTickets(_xrplSettings, eventPayload, _ipfsService, _xrplService, resourcePath);

                // Return Claim Ticket Details
                return Ok(result);


            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred whilst trying to insert a new event");
                Response.StatusCode = 500;
                return Content("An unexpected error occurred whilst inserting the event");
            }
        }

        [HttpPost("ClaimEventTicket")]
        public async Task<ActionResult> ClaimEventTicket(TicketClaimDto ticketClaim)
        {

            // Call method to claim tickets
            if(verifyClaimant(ticketClaim.NfTokenId, ticketClaim.Guid)) 
            {
                // Guid matches...Offer should be created
                var claimResult = await _xrplService.NfTokenAcceptBuyOffer(_xrplSettings.NftMintingAccountSeed, ticketClaim.NfTokenId, ticketClaim.NfTokenOfferIndex);
                return Ok(claimResult);

            }

            return BadRequest();
        }
        private bool verifyClaimant(string nfTokenId, Guid guid)
        {
            // Dummy coded for proof of concept
            if (nfTokenId != null && guid.ToString() == "150c995c-43f8-4759-9065-94fc6bf82d41")
                return true;

            throw new Exception("Invalid ticket claim guid");
        }

    }
}
