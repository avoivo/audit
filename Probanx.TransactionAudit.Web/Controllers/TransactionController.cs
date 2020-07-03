using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.Core.Services;
using Probanx.TransactionAudit.Web.Models;

namespace Probanx.TransactionAudit.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMessageDispatcher _messageDispatcher;

        public TransactionController(ILogger<TransactionController> logger, IMessageDispatcher messageDispatcher)
        {
            _logger = logger;
            _messageDispatcher = messageDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Post(Transaction transactionAudit)
        {
            await _messageDispatcher.Dispatch(new Message{});
            return transactionAudit;
        }
    }
}
