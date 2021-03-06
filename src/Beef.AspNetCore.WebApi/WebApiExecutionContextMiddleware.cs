﻿// Copyright (c) Avanade. Licensed under the MIT License. See https://github.com/Avanade/Beef

using Beef.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Beef.AspNetCore.WebApi
{
    /// <summary>
    /// Provides an <see cref="ExecutionContext"/> handling middleware that creates (using dependency injection) and enables additional configuration (<see cref="UpdateAction"/>) where required.
    /// </summary>
    /// <remarks>A new <see cref="ExecutionContext"/> <see cref="ExecutionContext.Current"/> is instantiated through dependency injection using the <see cref="HttpContext.RequestServices"/>.
    /// <para>Where no <see cref="UpdateAction"/> has been specified then the <see cref="ExecutionContext.Username"/> will be set to the <see cref="System.Security.Principal.IIdentity.Name"/>
    /// from the <see cref="HttpContext"/> <see cref="HttpContext.User"/>; otherwise, <see cref="DefaultUsername"/> where <c>null</c>.</para></remarks>
    public class WebApiExecutionContextMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Gets the default username where it is unable to be inferred (<see cref="System.Security.Principal.IIdentity.Name"/> from the <see cref="HttpContext"/> <see cref="HttpContext.User"/>).
        /// The default is 'Anonymous'.
        /// </summary>
        public static string DefaultUsername { get; set; } = "Anonymous";

        /// <summary>
        /// Represents the default <see cref="UpdateAction"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="ec">The <see cref="ExecutionContext"/>.</param>
        internal static void DefaultUpdateAction(HttpContext context, ExecutionContext ec)
        {
            ec.Username = context.User.Identity.Name ?? DefaultUsername;
            ec.Timestamp = Entities.Cleaner.Clean(DateTime.Now);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiExecutionContextMiddleware"/>.
        /// </summary>
        /// <param name="next">The next <see cref="RequestDelegate"/>.</param>
        /// <param name="updateAction">The optional <see cref="UpdateAction"/>.</param>
        public WebApiExecutionContextMiddleware(RequestDelegate next, Action<HttpContext, ExecutionContext> updateAction)
        {
            _next = Check.NotNull(next, nameof(next));
            UpdateAction = Check.NotNull(updateAction, nameof(updateAction));
        }

        /// <summary>
        /// Gets the <see cref="Action{HttpContext, ExecutionContext}"/> to enable further updates to the <see cref="ExecutionContext"/>.
        /// </summary>
        public Action<HttpContext, ExecutionContext> UpdateAction { get; private set; }

        /// <summary>
        /// Invokes the <see cref="WebApiExceptionHandlerMiddleware"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var ec = context.RequestServices.GetService<ExecutionContext>();
            UpdateAction.Invoke(context, ec);
            ec.ServiceProvider = context.RequestServices;

            if (context.Request.Headers.TryGetValue(WebApiConsts.CorrelationIdHeaderName, out var val))
                ec.CorrelationId = val.FirstOrDefault();

            ExecutionContext.Reset();
            ExecutionContext.SetCurrent(ec);

            await _next(context).ConfigureAwait(false);
        }
    }
}