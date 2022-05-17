﻿using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;

namespace PathingAPI
{
    public static class PathingAPILoggerSinkExtensions
    {
        public static LoggerConfiguration PathingAPILoggerSink(this LoggerSinkConfiguration loggerConfiguration)
        {
            return loggerConfiguration.Sink(new PathingAPILoggerSink());
        }
    }

    public class PathingAPILoggerSink : ILogEventSink
    {
        public static event EventHandler<string> OnLog;

        public void Emit(LogEvent logEvent)
        {
            OnLog?.Invoke(this, logEvent.RenderMessage());
        }

        public override bool Equals(object obj)
        {
            return obj is PathingAPILoggerSink;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}