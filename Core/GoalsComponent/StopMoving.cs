﻿using System;
using System.Threading;
using Game;
using Core.GOAP;
using SharedLib;

namespace Core.Goals;

public sealed class StopMoving
{
    private readonly WowProcessInput input;
    private readonly PlayerReader playerReader;
    private readonly AddonBits bits;
    private readonly CancellationToken ct;

    private float direction;

    public StopMoving(WowProcessInput input,
        PlayerReader playerReader,
        CancellationTokenSource<GoapAgent> cts,
        AddonBits bits)
    {
        this.input = input;
        this.playerReader = playerReader;
        ct = cts.Token;
        this.bits = bits;
    }

    public void Stop()
    {
        StopForward();
        StopTurn();
    }

    public void StopForward()
    {
        if (!bits.Moving())
            return;

        if (input.IsKeyDown(input.ForwardKey))
        {
            input.SetKeyState(input.ForwardKey, false, true);
        }
        else if (input.IsKeyDown(input.BackwardKey))
        {
            input.SetKeyState(input.BackwardKey, false, true);
        }
        else // moving by interact key
        {
            input.PressFixed(input.ForwardKey, Random.Shared.Next(2, 5), ct);
        }
    }

    public void StopTurn()
    {
        if (direction != playerReader.Direction)
        {
            bool pressedAny = false;

            if (input.IsKeyDown(input.TurnLeftKey))
            {
                input.SetKeyState(input.TurnLeftKey, false, true);
                pressedAny = true;
            }
            else if (input.IsKeyDown(input.TurnRightKey))
            {
                input.SetKeyState(input.TurnRightKey, false, true);
                pressedAny = true;
            }

            if (pressedAny)
                ct.WaitHandle.WaitOne(1);
        }

        direction = playerReader.Direction;
    }
}