﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using ServiceStack.Web;

namespace ServiceStack.Host
{
    public class InMemoryRollingRequestLogger : IRequestLogger
    {
        internal static long requestId = 0;

        public const int DefaultCapacity = 1000;
        protected readonly ConcurrentQueue<RequestLogEntry> logEntries = new ConcurrentQueue<RequestLogEntry>();
        protected readonly int capacity;

        public bool EnableSessionTracking { get; set; }

        public bool EnableRequestBodyTracking { get; set; }

        public bool EnableResponseTracking { get; set; }

        public bool EnableErrorTracking { get; set; }

        public string[] RequiredRoles { get; set; }

        public Type[] ExcludeRequestDtoTypes { get; set; }

        public Type[] HideRequestBodyForRequestDtoTypes { get; set; }

        protected InMemoryRollingRequestLogger() {}

        public InMemoryRollingRequestLogger(int? capacity = DefaultCapacity)
        {
            this.capacity = capacity.GetValueOrDefault(DefaultCapacity);
        }

        public virtual void Log(IRequest request, object requestDto, object response, TimeSpan requestDuration)
        {
            var requestType = requestDto?.GetType();

            if (ExcludeRequestType(requestType)) 
                return;

            var entry = CreateEntry(request, requestDto, response, requestDuration, requestType);

            logEntries.Enqueue(entry);

            RequestLogEntry dummy;
            if (logEntries.Count > capacity)
                logEntries.TryDequeue(out dummy);
        }

        protected RequestLogEntry CreateEntry(IRequest request, object requestDto, object response, TimeSpan requestDuration, Type requestType)
        {
            var entry = new RequestLogEntry
            {
                Id = Interlocked.Increment(ref requestId),
                DateTime = DateTime.UtcNow,
                RequestDuration = requestDuration,
            };

            if (request != null)
            {
                entry.HttpMethod = request.Verb;
                entry.AbsoluteUri = request.AbsoluteUri;
                entry.PathInfo = request.PathInfo;
                entry.IpAddress = request.UserHostAddress;
                entry.ForwardedFor = request.Headers[HttpHeaders.XForwardedFor];
                entry.Referer = request.Headers[HttpHeaders.Referer];
                entry.Headers = request.Headers.ToDictionary();
                entry.UserAuthId = request.GetItemStringValue(HttpHeaders.XUserAuthId);
                entry.Items = SerializableItems(request.Items);
                entry.Session = EnableSessionTracking ? request.GetSession() : null;
                entry.StatusCode = request.Response.StatusCode;
                entry.StatusDescription = request.Response.StatusDescription;

                var isClosed = request.Response.IsClosed;
                if (!isClosed)
                {
                    entry.UserAuthId = request.GetItemOrCookie(HttpHeaders.XUserAuthId);
                    entry.SessionId = request.GetSessionId();
                }

                if (HideRequestBodyForRequestDtoTypes != null
                    && requestType != null
                    && !HideRequestBodyForRequestDtoTypes.Contains(requestType))
                {
                    entry.RequestDto = requestDto;

                    if (!isClosed)
                        entry.FormData = request.FormData.ToDictionary();

                    if (EnableRequestBodyTracking)
                        entry.RequestBody = request.GetRawBody();
                }
            }

            if (!response.IsErrorResponse())
            {
                if (EnableResponseTracking)
                    entry.ResponseDto = response.GetResponseDto();
            }
            else
            {
                if (EnableErrorTracking)
                {
                    entry.ErrorResponse = ToSerializableErrorResponse(response);

                    var httpError = response as IHttpError;
                    if (httpError != null)
                    {
                        entry.StatusCode = (int)httpError.StatusCode;
                        entry.StatusDescription = httpError.StatusDescription;
                    }

                    var exception = response as Exception;
                    if (exception != null)
                    {
                        if (exception.InnerException != null)
                            exception = exception.InnerException;
                        
                        entry.ExceptionSource = exception.Source;
                        entry.ExceptionData = exception.Data;
                    }
                }
            }

            return entry;
        }

        protected bool ExcludeRequestType(Type requestType)
        {
            return ExcludeRequestDtoTypes != null
                   && requestType != null
                   && ExcludeRequestDtoTypes.Contains(requestType);
        }

        public Dictionary<string, string> SerializableItems(Dictionary<string, object> items)
        {
            var to = new Dictionary<string, string>();
            foreach (var item in items)
            {
                var value = item.Value?.ToString() ?? "(null)";
                to[item.Key] = value;
            }

            return to;
        }

        public virtual List<RequestLogEntry> GetLatestLogs(int? take)
        {
            var requestLogEntries = logEntries.ToArray();
            return take.HasValue
                ? new List<RequestLogEntry>(requestLogEntries.Take(take.Value))
                : new List<RequestLogEntry>(requestLogEntries);
        }

        public static object ToSerializableErrorResponse(object response)
        {
            var errorResult = response as IHttpResult;
            if (errorResult != null)
                return errorResult.Response;

            var ex = response as Exception;
            return ex?.ToResponseStatus();
        }
    }
}