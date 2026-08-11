using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.CharacterStats;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Stats;
using UnityEngine;

namespace OilGrabber;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private IEnumerator Start()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        while (!StaticInstance<AsyncAssetLoading>.Instance.loadingDone)
        {
            yield return null;
        }

        var database = StaticInstance<AsyncAssetLoading>.Instance.itemDatabase;
        if (database == null)
        {
            Logger.LogError("Database not found");
            throw new ArgumentNullException(nameof(database));
        }

        ExportDB(database);
        Logger.LogMessage("Finished!");
    }

    public void ExportDB(ItemDatabase db)
    {
        var OilList = new List<EnhancementDTO>();
        var scrollList = new List<EnhancementDTO>();
        foreach (var item in db.GetRawList())
        {
            if (!item)
            {
                continue;
            }
            if (item.ItemType != ItemType.Enchantment)
            {
                continue;
            }
            if (!item.includedInEarlyAccess)
            {
                continue;
            }
            if (item.LocalizedDisplayName.StartsWith("Test"))
            {
                continue;
            }
            Logger.LogInfo($"[Mod] Item found: {item.LocalizedDisplayName}");

            var enhancement = AssetAccess.GetAsset(item.appliesEnchantment);
            
            List<ModifierDTO> itemModifiers = new();
            foreach (var mod in enhancement.modifiersApplied)
            {
                var attributeExpanded = AssetAccess.GetAsset(mod.attribute);
                itemModifiers.Add(new ModifierDTO
                { 
                    modifierName = mod.attribute.ToString(),
                    statModType = FromStatModTypeToString(mod.modType),
                    value = mod.value,
                    id = attributeExpanded.id,
                    label = attributeExpanded.label,
                    itemDescriptionName = attributeExpanded.itemDescriptionName,
                    showInItemDescription = attributeExpanded.showInItemDescription,
                    unitMeasure = attributeExpanded.unitMeasure,
                    simplifiedModAmount = attributeExpanded.simplifiedModAmount,
                    simplifiedIncreaseString = attributeExpanded.simplifiedIncreaseString,
                    simplifiedDecreaseString = attributeExpanded.simplifiedDecreaseString,
                    isBooleanAttribute = attributeExpanded.isBooleanAttribute,
                    isPercentageAttribute = attributeExpanded.isPercentageAttribute,
                    showPercentageAsFactor = attributeExpanded.showPercentageAsFactor,
                    overrideUnitName = attributeExpanded.overrideUnitName,
                    projEffectDefinition = GetProjDTO(attributeExpanded?.projEffectDefinition),
                    customVisualsPrefab = attributeExpanded.customVisualsPrefab?.ToString(),
                    applyAttributeModifier = attributeExpanded.applyAttributeModifier?.ToString(),
                    explosionOnHit = attributeExpanded.explosionOnHit.ToString(),
                    explosionScale = attributeExpanded.explosionScale,
                    spawnOnUnitHit = GetEffectSpawnDTO(attributeExpanded?.spawnOnUnitHit),
                    spawnOnEnvironmentHit = GetEffectSpawnDTO(attributeExpanded?.spawnOnEnvironmentHit),
                    spawnOnStartShoot = GetEffectSpawnDTO(attributeExpanded?.spawnOnStartShoot),
                    bloodDecalOnEnvironmentHit = attributeExpanded.bloodDecalOnEnvironmentHit.ToString(),
                    replacesBloodType = attributeExpanded.replacesBloodType.ToString()
                });
            }
            var enhancementDTO = new EnhancementDTO
            {
                name = item.LocalizedDisplayName,
                modifiers = itemModifiers
            };
            if (enhancementDTO.modifiers.Count == 0)
            {
                continue;
            }

            if (enhancementDTO.name.Contains("Oil"))
            {
                OilList.Add(enhancementDTO);
                ImageHelpers.SaveBaseImage(item, "Oils");
            }
            else
            {
                scrollList.Add(enhancementDTO);
                ImageHelpers.SaveBaseImage(item, "Scrolls");
            }
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new IgnoreUnchangedDefaultsResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        Logger.LogMessage($"Number of Oils: {OilList.Count}");
        Logger.LogMessage($"Number of Scrolls: {scrollList.Count}");

        string json = JsonConvert.SerializeObject(OilList, settings);
        string rootDir = Paths.GameRootPath;
        string folderPath = Path.Combine(rootDir, "Extracted Data\\Oils\\");
        Directory.CreateDirectory(folderPath);
        string path = Path.Combine(folderPath, "oils.json");
        File.WriteAllText(path, json);

        string json2 = JsonConvert.SerializeObject(scrollList, settings);
        string rootDir2 = Paths.GameRootPath;
        string folderPath2 = Path.Combine(rootDir2, "Extracted Data\\Scrolls\\");
        Directory.CreateDirectory(folderPath2);
        string path2 = Path.Combine(folderPath2, "scrolls.json");
        File.WriteAllText(path2, json2);
    }

    
    public string FromStatModTypeToString(StatModType modtype) => modtype switch
    {
        StatModType.Flat        => "Flat",
        StatModType.PercentAdd  => "PercentAdd",
        StatModType.PercentMult => "PercentMult",
        _ => throw new ArgumentOutOfRangeException(nameof(modtype), $"Not expected direction value: {modtype}"),
    };

    public ProjectileDTO GetProjDTO(ProjectileEffectDefinition projEffectDefinition)
    {
        if (!projEffectDefinition)
        {
            return null;
        }
        return new ProjectileDTO
        {
            drawDefaultBullet = projEffectDefinition.drawDefaultBullet,
            mainColor = projEffectDefinition.mainColor.ToString(),
            coreColor = projEffectDefinition.coreColor.ToString(),
            playImpactSounds = projEffectDefinition.playImpactSounds,
            soundShotSilencedVolumeDb = projEffectDefinition.soundShotSilencedVolumeDb,
            innerBeamWidth = projEffectDefinition.innerBeamWidth,
            outerBeamWidth = projEffectDefinition.outerBeamWidth
        };
    }
    
    public EffectSpawnDTO GetEffectSpawnDTO(EffectSpawnEntry effect)
    {
        if (!effect.effect)
        {
            return null;
        }
        return new EffectSpawnDTO
        {
            effect = effect?.effect?.ToString() ?? "",
            procChance = effect.procChance
        };
    }
}
