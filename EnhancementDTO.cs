using System;
using System.Collections.Generic;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.CharacterStats;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Effects;
using UnityEngine;

[Serializable]
public class EnhancementDTO
{
    public string name;
    public string LocalizedFlavor;
    public List<ModifierDTO> modifiers;
}

[Serializable]
public class ModifierDTO
{
    public string modifierName;
    public string statModType;
    public float value;
    public ItemAttributes id;
    public string label = "";
    public string itemDescriptionName = "";
    public bool showInItemDescription;
    public string unitMeasure = "";
    public bool simplifiedModAmount;
    public string simplifiedIncreaseString = "";
    public string simplifiedDecreaseString = "";
    public bool isBooleanAttribute;
    public bool isPercentageAttribute;
    public bool showPercentageAsFactor;
    public string overrideUnitName = "";
    public ProjectileDTO projEffectDefinition;
    public string customVisualsPrefab = "";
    public string applyAttributeModifier = "None +0%";
    public string explosionOnHit = "None";
    public float explosionScale = 1.0f;
    public EffectSpawnDTO spawnOnUnitHit;
    public EffectSpawnDTO spawnOnEnvironmentHit;
    public EffectSpawnDTO spawnOnStartShoot;
    public string bloodDecalOnEnvironmentHit = "Normal";
    public string replacesBloodType = "Normal";
}

public class ProjectileDTO
{
    public ProjectileEffect id;
    public bool drawDefaultBullet;
    public string mainColor;
    public string coreColor;
    public bool playImpactSounds = true;
    public float soundShotSilencedVolumeDb;
    public float innerBeamWidth = 0.5f;
    public float outerBeamWidth = 1f;
}

public class EffectSpawnDTO
{
    public string effect;
    public float procChance;
}