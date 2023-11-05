﻿using Core.GOAP;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Game;

namespace Core;

public sealed class ConfigBotController : IBotController, IDisposable
{
    private readonly ILogger logger;
    private readonly CancellationTokenSource cts;

    private readonly Thread addonThread;
    private readonly IAddonReader addonReader;
    private readonly IWowScreen wowScreen;

    public GoapAgent? GoapAgent => throw new NotImplementedException();
    public RouteInfo? RouteInfo => throw new NotImplementedException();
    public string SelectedClassFilename => throw new NotImplementedException();
    public string? SelectedPathFilename => throw new NotImplementedException();

    public ClassConfiguration? ClassConfig => null;

    public bool IsBotActive => false;

    public double AvgScreenLatency => throw new NotImplementedException();
    public double AvgNPCLatency => throw new NotImplementedException();

    public event Action? ProfileLoaded;
    public event Action? StatusChanged;

    public ConfigBotController(ILogger logger,
        IAddonReader addonReader,
        IWowScreen wowScreen,
        CancellationTokenSource cts)
    {
        this.logger = logger;
        this.cts = cts;
        this.addonReader = addonReader;
        this.wowScreen = wowScreen;

        addonThread = new(AddonThread);
        addonThread.Start();
    }

    public void Dispose()
    {
        cts.Cancel();
    }

    private void AddonThread()
    {
        while (!cts.IsCancellationRequested)
        {
            wowScreen.Update();
            addonReader.Update();
            Thread.Sleep(20);
        }
        logger.LogWarning("Thread stopped!");
    }


    public void Shutdown()
    {
        cts.Cancel();
    }

    public void MinimapNodeFound()
    {
        throw new NotImplementedException();
    }

    public void ToggleBotStatus()
    {
        StatusChanged?.Invoke();
        throw new NotImplementedException();
    }

    public IEnumerable<string> ClassFiles()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> PathFiles()
    {
        throw new NotImplementedException();
    }

    public void LoadClassProfile(string classFilename)
    {
        ProfileLoaded?.Invoke();
        throw new NotImplementedException();
    }

    public void LoadPathProfile(string pathFilename)
    {
        ProfileLoaded?.Invoke();
        throw new NotImplementedException();
    }

    public void OverrideClassConfig(ClassConfiguration classConfiguration)
    {
        throw new NotImplementedException();
    }

    public ClassConfiguration ResolveLoadedProfile()
    {
        throw new NotImplementedException();
    }
}