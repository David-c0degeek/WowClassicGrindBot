﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace Core;

public class BadZone
{
    public int ZoneId { get; init; } = -1;
    public Vector3 ExitZoneLocation { get; init; }
}

public enum Mode
{
    Grind = 0,
    CorpseRun = 1,
    AttendedGather = 2,
    AttendedGrind = 3,
    AssistFocus = 4
}


public sealed partial class ClassConfiguration
{
    public bool Log { get; set; } = true;
    public bool LogBagChanges { get; set; } = true;
    public bool Loot { get; set; } = true;
    public bool Skin { get; set; }
    public bool Herb { get; set; }
    public bool Mine { get; set; }
    public bool Salvage { get; set; }
    public bool GatherCorpse => Skin || Herb || Mine || Salvage;

    public bool UseMount { get; set; } = true;
    public bool KeyboardOnly { get; set; }
    public bool AllowPvP { get; set; }
    public bool AutoPetAttack { get; set; } = true;

    public string PathFilename { get; set; } = string.Empty;

    public string? OverridePathFilename { get; set; } = string.Empty;

    public bool PathThereAndBack { get; set; } = true;
    public bool PathReduceSteps { get; set; }

    public Mode Mode { get; init; } = Mode.Grind;

    public BadZone WrongZone { get; } = new BadZone();

    public int NPCMaxLevels_Above { get; set; } = 1;
    public int NPCMaxLevels_Below { get; set; } = 7;
    public UnitClassification TargetMask { get; set; } =
        UnitClassification.Normal |
        UnitClassification.Trivial |
        UnitClassification.Rare;

    public bool CheckTargetGivesExp { get; set; }
    public string[] Blacklist { get; init; } = Array.Empty<string>();

    public Dictionary<int, SchoolMask> NpcSchoolImmunity { get; } = new();

    public Dictionary<string, int> IntVariables { get; } = new();

    public KeyActions Pull { get; } = new();
    public KeyActions Combat { get; } = new();
    public KeyActions Adhoc { get; } = new();
    public KeyActions Parallel { get; } = new();
    public KeyActions NPC { get; } = new();
    public KeyActions AssistFocus { get; } = new();
    public WaitKeyActions Wait { get; } = new();
    public FormKeyActions Form { get; } = new();

    public KeyAction[] GatherFindKeyConfig { get; set; } = Array.Empty<KeyAction>();
    public string[] GatherFindKeys { get; init; } = Array.Empty<string>();

    public KeyAction Jump { get; } = new();
    public string JumpKey { get; init; } = "Spacebar";

    public KeyAction Interact { get; } = new();
    public string InteractKey { get; init; } = "I";

    public KeyAction InteractMouseOver { get; } = new();
    public string InteractMouseOverKey { get; init; } = "J";

    public KeyAction Approach { get; } = new();
    public KeyAction AutoAttack { get; } = new();

    public KeyAction TargetLastTarget { get; } = new();
    public string TargetLastTargetKey { get; init; } = "G";

    public KeyAction StandUp { get; } = new();
    public string StandUpKey { get; init; } = "X";

    public KeyAction ClearTarget { get; } = new();
    public string ClearTargetKey { get; init; } = "Insert";

    public KeyAction StopAttack { get; } = new();
    public string StopAttackKey { get; init; } = "Delete";

    public KeyAction TargetNearestTarget { get; } = new();
    public string TargetNearestTargetKey { get; init; } = "Tab";

    public KeyAction TargetTargetOfTarget { get; } = new();
    public string TargetTargetOfTargetKey { get; init; } = "F";
    public KeyAction TargetPet { get; } = new();
    public string TargetPetKey { get; init; } = "Multiply";

    public KeyAction PetAttack { get; } = new();
    public string PetAttackKey { get; init; } = "Subtract";

    public KeyAction TargetFocus { get; } = new();
    public string TargetFocusKey { get; init; } = "PageUp";

    public KeyAction FollowTarget { get; } = new();
    public string FollowTargetKey { get; init; } = "PageDown";

    public KeyAction Mount { get; } = new();
    public string MountKey { get; init; } = "O";

    public ConsoleKey ForwardKey { get; init; } = ConsoleKey.UpArrow;  // 38
    public ConsoleKey BackwardKey { get; init; } = ConsoleKey.DownArrow; // 40
    public ConsoleKey TurnLeftKey { get; init; } = ConsoleKey.LeftArrow; // 37
    public ConsoleKey TurnRightKey { get; init; } = ConsoleKey.RightArrow; // 39

    public void Initialise(IServiceProvider sp, string? overridePathFile)
    {
        Jump.Key = JumpKey;
        Jump.Name = nameof(Jump);
        Jump.BaseAction = true;

        TargetLastTarget.Key = TargetLastTargetKey;
        TargetLastTarget.Name = nameof(TargetLastTarget);
        TargetLastTarget.Cooldown = 0;
        TargetLastTarget.BaseAction = true;

        StandUp.Key = StandUpKey;
        StandUp.Name = nameof(StandUp);
        StandUp.Cooldown = 0;
        StandUp.BaseAction = true;

        ClearTarget.Key = ClearTargetKey;
        ClearTarget.Name = nameof(ClearTarget);
        ClearTarget.Cooldown = 0;
        ClearTarget.BaseAction = true;

        StopAttack.Key = StopAttackKey;
        StopAttack.Name = nameof(StopAttack);
        StopAttack.PressDuration = 20;
        StopAttack.BaseAction = true;

        TargetNearestTarget.Key = TargetNearestTargetKey;
        TargetNearestTarget.Name = nameof(TargetNearestTarget);
        TargetNearestTarget.BaseAction = true;

        TargetPet.Key = TargetPetKey;
        TargetPet.Name = nameof(TargetPet);
        TargetPet.Cooldown = 0;
        TargetPet.BaseAction = true;

        TargetTargetOfTarget.Key = TargetTargetOfTargetKey;
        TargetTargetOfTarget.Name = nameof(TargetTargetOfTarget);
        TargetTargetOfTarget.Cooldown = 0;
        TargetTargetOfTarget.BaseAction = true;

        TargetFocus.Key = TargetFocusKey;
        TargetFocus.Name = nameof(TargetFocus);
        TargetFocus.Cooldown = 0;
        TargetFocus.BaseAction = true;

        FollowTarget.Key = FollowTargetKey;
        FollowTarget.Name = nameof(FollowTarget);
        FollowTarget.Cooldown = 0;
        FollowTarget.BaseAction = true;

        PetAttack.Key = PetAttackKey;
        PetAttack.Name = nameof(PetAttack);
        PetAttack.PressDuration = 10;
        PetAttack.BaseAction = true;

        Mount.Key = MountKey;
        Mount.Name = nameof(Mount);
        Mount.BaseAction = true;
        Mount.Cooldown = 6000;

        Interact.Key = InteractKey;
        Interact.Name = nameof(Interact);
        Interact.Cooldown = 0;
        Interact.PressDuration = 30;
        Interact.BaseAction = true;

        InteractMouseOver.Key = InteractMouseOverKey;
        InteractMouseOver.Name = nameof(InteractMouseOver);
        InteractMouseOver.Cooldown = 0;
        InteractMouseOver.PressDuration = 10;
        InteractMouseOver.BaseAction = true;

        Approach.Key = InteractKey;
        Approach.Name = nameof(Approach);
        Approach.PressDuration = 10;
        Approach.BaseAction = true;

        AutoAttack.Key = InteractKey;
        AutoAttack.Name = nameof(AutoAttack);
        AutoAttack.BaseAction = true;

        RequirementFactory factory = new(sp, this);

        ILogger logger = sp.GetRequiredService<ILogger>();
        PlayerReader playerReader = sp.GetRequiredService<PlayerReader>();

        RecordInt globalTime = sp.GetRequiredService<AddonReader>().GlobalTime;

        var baseActionKeys = GetByType<KeyAction>();
        foreach ((string _, KeyAction keyAction) in baseActionKeys)
        {
            keyAction.Init(logger, Log, playerReader, globalTime);
            factory.Init(keyAction);
        }

        SetBaseActions(Pull,
            Interact, Approach, AutoAttack, StopAttack, PetAttack);

        SetBaseActions(Combat,
            Interact, Approach, AutoAttack, StopAttack, PetAttack);

        var groups = GetByType<KeyActions>();

        foreach ((string name, KeyActions keyActions) in groups)
        {
            if (keyActions.Sequence.Length > 0)
            {
                LogInitBind(logger, name);
            }

            keyActions.InitBinds(logger, factory);

            if (keyActions is WaitKeyActions wait &&
                wait.AutoGenerateWaitForFoodAndDrink)
                wait.AddWaitKeyActionsForFoodOrDrink(logger, groups);
        }

        foreach ((string name, KeyActions keyActions) in groups)
        {
            if (keyActions.Sequence.Length > 0)
            {
                LogInitKeyActions(logger, name);
            }

            keyActions.Init(logger, Log,
                playerReader, globalTime, factory);
        }

        GatherFindKeyConfig = new KeyAction[GatherFindKeys.Length];
        for (int i = 0; i < GatherFindKeys.Length; i++)
        {
            KeyAction newAction = new()
            {
                Key = GatherFindKeys[i],
                Name = $"Profession {i}"
            };

            newAction.Init(logger, Log, playerReader, globalTime);
            factory.Init(newAction);

            GatherFindKeyConfig[i] = newAction;
        }

        OverridePathFilename = overridePathFile;
        if (!string.IsNullOrEmpty(OverridePathFilename))
        {
            PathFilename = OverridePathFilename;
        }

        DataConfig dataConfig = sp.GetRequiredService<DataConfig>();
        if (!File.Exists(Path.Join(dataConfig.Path, PathFilename)))
        {
            if (!string.IsNullOrEmpty(OverridePathFilename))
                throw new Exception(
                    $"[{nameof(ClassConfiguration)}] " +
                    $"`{OverridePathFilename}` file does not exists!");
            else
                throw new Exception(
                    $"[{nameof(ClassConfiguration)}] " +
                    $"`{PathFilename}` file does not exists!");
        }

        if (CheckTargetGivesExp)
        {
            logger.LogWarning($"{nameof(CheckTargetGivesExp)} is enabled. " +
                $"{nameof(NPCMaxLevels_Above)} and {nameof(NPCMaxLevels_Below)} ignored!");
        }
        if (KeyboardOnly)
        {
            logger.LogWarning($"{nameof(KeyboardOnly)} " +
                $"mode is enabled. Mouse based actions ignored.");

            if (GatherCorpse)
                logger.LogWarning($"{nameof(GatherCorpse)} " +
                    $"limited to the last target. Rest going to be skipped!");
        }
    }

    private static void SetBaseActions(
        KeyActions keyActions, params KeyAction[] baseActions)
    {
        KeyAction @default = new();
        for (int i = 0; i < keyActions.Sequence.Length; i++)
        {
            KeyAction user = keyActions.Sequence[i];
            for (int d = 0; d < baseActions.Length; d++)
            {
                KeyAction baseAction = baseActions[d];

                if (user.Name != baseAction.Name)
                    continue;

                user.Key = baseAction.Key;

                //if (!string.IsNullOrEmpty(@default.Requirement))
                //    user.Requirement += " " + @default.Requirement;
                //user.Requirements.AddRange(@default.Requirements);

                if (user.AfterCastDelay == @default.AfterCastDelay)
                    user.AfterCastDelay = baseAction.AfterCastDelay;

                if (user.PressDuration == @default.PressDuration)
                    user.PressDuration = baseAction.PressDuration;

                if (user.Cooldown == @default.Cooldown)
                    user.Cooldown = baseAction.Cooldown;

                if (user.BaseAction == @default.BaseAction)
                    user.BaseAction = baseAction.BaseAction;

                if (user.Item == @default.Item)
                    user.Item = baseAction.Item;
            }
        }
    }

    public List<(string name, T)> GetByType<T>()
    {
        return GetType()
            .GetProperties(BindingFlags.Instance |
            BindingFlags.Public | BindingFlags.FlattenHierarchy)
            .Where(OfType)
            .Select(pInfo =>
            {
                return (pInfo.Name, (T)pInfo.GetValue(this)!);
            })
            .ToList();

        static bool OfType(PropertyInfo pInfo) =>
            typeof(T).IsAssignableFrom(pInfo.PropertyType);
    }

    [LoggerMessage(
        EventId = 0010,
        Level = LogLevel.Information,
        Message = "[{prefix}] Init Binds(Cost, Cooldown)")]
    static partial void LogInitBind(ILogger logger, string prefix);

    [LoggerMessage(
        EventId = 0011,
        Level = LogLevel.Information,
        Message = "[{prefix}] Init KeyActions")]
    static partial void LogInitKeyActions(ILogger logger, string prefix);

}