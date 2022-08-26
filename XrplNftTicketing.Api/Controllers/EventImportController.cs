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
using System.Collections.Generic;

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

        /// <summary>
        /// function for app demo use only
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserConfigSettings")]
        public ActionResult GetUserConfigSettings()
        {
            return new JsonResult(new List<string>() { _xrplSettings.WssUrl, _xrplSettings.UserAccountSeed } );
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
                var message = "An error occurred whilst trying to insert a new event." + ex.Message;
                _logger.LogCritical(ex, message);
                Response.StatusCode = 500;
                return Content(message);
            }
        }

        [HttpPost("ClaimEventTicket")]
        public async Task<ActionResult> ClaimEventTicket(TicketClaimDto ticketClaim)
        {
            try
            {
                // Call method to claim tickets
                if (verifyClaimant(ticketClaim.NfTokenId, ticketClaim.Guid)) 
                {
                    // Guid matches...Offer should be created
                    var claimResult = await _xrplService.NfTokenAcceptBuyOffer(_xrplSettings.NftMintingAccountSeed, ticketClaim);
                    return Ok(claimResult);
                }
            }
            catch (Exception ex)
            {
                var message = "An error occurred whilst trying to Claim Event Ticket." + ex.Message;
                _logger.LogCritical(ex, message);
                Response.StatusCode = 500;
                return BadRequest(message);
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
