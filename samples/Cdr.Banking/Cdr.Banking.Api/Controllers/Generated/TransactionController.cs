/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable
#pragma warning disable IDE0005 // Using directive is unnecessary; are required depending on code-gen options

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Beef;
using Beef.AspNetCore.WebApi;
using Beef.Entities;
using Cdr.Banking.Business;
using Cdr.Banking.Common.Entities;
using RefDataNamespace = Cdr.Banking.Common.Entities;

namespace Cdr.Banking.Api.Controllers
{
    /// <summary>
    /// Provides the <see cref="Transaction"/> Web API functionality.
    /// </summary>
    [Route("api/v1/banking/accounts")]
    public partial class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="manager">The <see cref="ITransactionManager"/>.</param>
        public TransactionController(ITransactionManager manager)
            { _manager = Check.NotNull(manager, nameof(manager)); TransactionControllerCtor(); }

        partial void TransactionControllerCtor(); // Enables additional functionality to be added to the constructor.

        /// <summary>
        /// Get transaction for account.
        /// </summary>
        /// <param name="accountId">The Account Id.</param>
        /// <param name="fromDate">The From (oldest time).</param>
        /// <param name="toDate">The To (newest time).</param>
        /// <param name="minAmount">The Min Amount.</param>
        /// <param name="maxAmount">The Max Amount.</param>
        /// <param name="text">The Text.</param>
        /// <returns>The <see cref="TransactionCollection"/></returns>
        [HttpGet("{accountId}/transactions")]
        [ProducesResponseType(typeof(TransactionCollection), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult GetTransactions([FromRoute] string? accountId, [FromQuery(Name = "oldest-time")] DateTime? fromDate = default, [FromQuery(Name = "newest-time")] DateTime? toDate = default, [FromQuery(Name = "min-amount")] decimal? minAmount = default, [FromQuery(Name = "max-amount")] decimal? maxAmount = default, string? text = default)
        {
            var args = new TransactionArgs { FromDate = fromDate, ToDate = toDate, MinAmount = minAmount, MaxAmount = maxAmount, Text = text };
            return new WebApiGet<TransactionCollectionResult, TransactionCollection, Transaction>(this, () => _manager.GetTransactionsAsync(accountId, args, WebApiQueryString.CreatePagingArgs(this)),
                operationType: OperationType.Read, statusCode: HttpStatusCode.OK, alternateStatusCode: HttpStatusCode.NoContent);
        }
    }
}

#pragma warning restore IDE0005
#nullable restore