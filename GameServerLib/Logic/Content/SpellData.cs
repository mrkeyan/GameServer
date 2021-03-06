﻿using LeagueSandbox.GameServer.Core.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSandbox.GameServer.Logic.Content
{
    public enum SpellFlag : uint
    {
        SPELL_FLAG_AutoCast = 0x00000002,
        SPELL_FLAG_InstantCast = 0x00000004,
        SPELL_FLAG_PersistThroughDeath = 0x00000008,
        SPELL_FLAG_NonDispellable = 0x00000010,
        SPELL_FLAG_NoClick = 0x00000020,
        SPELL_FLAG_AffectImportantBotTargets = 0x00000040,
        SPELL_FLAG_AllowWhileTaunted = 0x00000080,
        SPELL_FLAG_NotAffectZombie = 0x00000100,
        SPELL_FLAG_AffectUntargetable = 0x00000200,
        SPELL_FLAG_AffectEnemies = 0x00000400,
        SPELL_FLAG_AffectFriends = 0x00000800,
        SPELL_FLAG_AffectBuildings = 0x00001000,
        SPELL_FLAG_NotAffectSelf = 0x00002000,
        SPELL_FLAG_AffectNeutral = 0x00004000,
        SPELL_FLAG_AffectAllSides = 0x00004C00,
        SPELL_FLAG_AffectMinions = 0x00008000,
        SPELL_FLAG_AffectHeroes = 0x00010000,
        SPELL_FLAG_AffectTurrets = 0x00020000,
        SPELL_FLAG_AffectAllUnitTypes = 0x00038000,
        SPELL_FLAG_AlwaysSelf = 0x00040000,
        SPELL_FLAG_AffectDead = 0x00080000,
        SPELL_FLAG_AffectNotPet = 0x00100000,
        SPELL_FLAG_AffectBarracksOnly = 0x00200000,
        SPELL_FLAG_IgnoreVisibilityCheck = 0x00400000,
        SPELL_FLAG_NonTargetableAlly = 0x00800000,
        SPELL_FLAG_NonTargetableEnemy = 0x01000000,
        SPELL_FLAG_NonTargetableAll = 0x01800000,
        SPELL_FLAG_TargetableToAll = 0x02000000,
        SPELL_FLAG_AffectWards = 0x04000000,
        SPELL_FLAG_AffectUseable = 0x08000000,
        SPELL_FLAG_IgnoreAllyMinion = 0x10000000,
        SPELL_FLAG_IgnoreEnemyMinion = 0x20000000,
        SPELL_FLAG_IgnoreLaneMinion = 0x40000000,
        SPELL_FLAG_IgnoreClones = 0x80000000
    }
    
    public enum SpellTargetType : int
    {
        TARGET_SELF = 0, // teemo W ; xin Q
        TARGET_UNIT = 1, // Taric E ; Annie Q ; teemo Q ; xin E
        TARGET_LOC_AOE = 2, // Lux E, Ziggs R
        TARGET_CONE = 3, // Annie W, Kass E
        TARGET_SELF_AOE = 4, // sivir R, Gangplanck E
        TARGET_LOC = 6, // Ez Q, W, E, R ; Mundo Q
        TARGET_LOC2 = 7  // Morg Q, Cait's Q -- These don't seem to have Missile inibins, and SpawnProjectile doesn't seem necessary to show the projectiles
    };

    public class SpellData
    {
        private Game _game = Program.ResolveDependency<Game>();
        private Logger _logger = Program.ResolveDependency<Logger>();

        public string AffterEffectName { get; set; } = "";
        //AIEndOnly 
        //AILifetime 
        //AIRadius 
        //AIRange 
        //AISendEvent 
        //AISpeed
        public string AlternateName { get; set; } = "";
        public bool AlwaysSnapFacing { get; set; } = false;
        //AmmoCountHiddenInUI
        public float[] AmmoRechargeTime { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public int[] AmmoUsed { get; set; } = { 1, 1, 1, 1, 1, 1, 1 };
        //AnimationLeadOutName
        //AnimationLoopName
        //AnimationName
        //AnimationWinddownName
        //ApplyAttackDamage
        //ApplyAttackEffect
        //ApplyMaterialOnHitSound
        public bool BelongsToAvatar { get; set; } = false;
        public float BounceRadius { get; set; } = 450;
        public bool CanCastWhileDisabled { get; set; } = false;
        public float CancelChargeOnRecastTime { get; set; } = 0;
        public bool CanMoveWhileChanneling { get; set; } = false;
        public bool CannotBeSuppressed { get; set; } = false;
        public bool CanOnlyCastWhileDead { get; set; } = false;
        public bool CanOnlyCastWhileDisabled { get; set; } = false;
        public bool CantCancelWhileChanneling { get; set; } = false;
        public bool CantCancelWhileWindingUp { get; set; } = false;
        public bool CantCastWhileRooted { get; set; } = false;
        public float CastConeAngle { get; set; } = 45;
        public float CastConeDistance { get; set; } = 400;
        public float CastFrame { get; set; } = 0;
        public float[] CastRadius { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public float[] CastRadiusSecondary { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        //CastRadiusSecondaryTexture
        //CastRadiusTexture
        public float[] CastRange { get; set; } = { 400, 400, 400, 400, 400, 400, 400 };
        public float[] CastRangeDisplayOverride { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public float[] CastRangeGrowthDuration { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public float[] CastRangeGrowthMax { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        //CastRangeTextureOverrideName
        public bool CastRangeUseBoundingBoxes { get; set; } = false;
        public float CastTargetAdditionalUnitsRadius { get; set; } = 0;
        public int CastType { get; set; } = 0;
        public float[] ChannelDuration { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public float ChargeUpdateInterval { get; set; } = 0;
        public float CircleMissileAngularVelocity { get; set; } = 0;
        public float CircleMissileRadialVelocity { get; set; } = 0;
        //ClientOnlyMissileTargetBoneName
        //Coefficient
        //Coefficient2
        //ConsideredAsAutoAttack
        public float[] Cooldown { get; set; } = { 10, 10, 10, 10, 10, 10, 10 };
        //CursorChangesInGrass
        //CursorChangesInTerrain
        public int DeathRecapPriority { get; set; } = 0;
        public float DelayCastOffsetPercent { get; set; } = 0;
        public float DelayTotalTimePercent { get; set; } = 0;
        //Description
        //DisableCastBar
        //DisplayName
        public bool DoesntBreakChannels { get; set; } = false;
        public bool DoNotNeedToFaceTarget { get; set; } = false;
        //DrawSecondaryLineIndicator
        //DynamicExtended
        //string DynamicTooltip
        //EffectXLevelYAmmount
        public int Flags { get; set; } = 0;
        //FloatStaticsDecimalsX
        //FloatVarsDecimalsX
        public bool HaveAfterEffect { get; set; } = false;
        public bool HaveHitBone { get; set; } = false;
        public bool HaveHitEffect { get; set; } = false;
        public bool HavePointEffect { get; set; } = false;
        //HideRangeIndicatorWhenCasting
        public string HitBoneName { get; set; } = "";
        public string HitEffectName { get; set; } = "";
        public int HitEffectOrientType { get; set; } = 1;
        //HitEffectPlayerName
        public bool IgnoreAnimContinueUntilCastFrame { get; set; } = false;
        public bool IgnoreRangeCheck { get; set; } = false;
        //InventoryIconX
        public bool IsDisabledWhileDead { get; set; } = true;
        public bool IsToggleSpell { get; set; } = false;
        //KeywordWhenAcquired
        //LevelXDesc
        public float LineDragLength { get; set; } = 0;
        public int LineMissileBounces { get; set; } = 0;
        public bool LineMissileCollisionFromStartPoint { get; set; } = false;
        public float LineMissileDelayDestroyAtEndSeconds { get; set; } = 0;
        public bool LineMissileEndsAtTargetPoint { get; set; } = false;
        public bool LineMissileFollowsTerrainHeight { get; set; } = false;
        public float LineMissileTargetHeightAugment { get; set; } = 100;
        public float LineMissileTimePulseBetweenCollisionSpellHits { get; set; } = 0;
        public bool LineMissileTrackUnits { get; set; } = false;
        public bool LineMissileUsesAccelerationForBounce { get; set; } = false;
        //LineTargetingBaseTextureOverrideName
        //LineTargetingBaseTextureOverrideName
        public float LineWidth { get; set; } = 0;
        public float[] LocationTargettingLength { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public float[] LocationTargettingWidth { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        public bool LockConeToPlayer { get; set; } = false;
        //LookAtPolicy 
        public float LuaOnMissileUpdateDistanceInterval { get; set; } = 0;
        public float[] ManaCost { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        //Map_X_EffectYLevelZAmmount
        public int[] MaxAmmo { get; set; } = { 0, 0, 0, 0, 0, 0, 0 };
        //MaxGrowthRangeTextureName 
        //MinimapIcon
        //MinimapIconDisplayFlag
        //MinimapIconRotation
        public float MissileAccel { get; set; } = 0;
        public string MissileBoneName { get; set; } = "";
        public string MissileEffect { get; set; } = "";
        public string MissileEffectPlayer { get; set; } = "";
        public float MissileFixedTravelTime { get; set; } = 0;
        public bool MissileFollowsTerrainHeight { get; set; } = false;
        public float MissileGravity { get; set; } = 0;
        public float MissileLifetime { get; set; } = 0;
        public float MissileMaxSpeed { get; set; } = 0;
        public float MissileMinSpeed { get; set; } = 0;
        public float MissileMinTravelTime { get; set; } = 0;
        public float MissilePerceptionBubbleRadius { get; set; } = 0;
        public bool MissilePerceptionBubbleRevealsStealth { get; set; } = false;
        public float MissileSpeed { get; set; } = 500;
        public float MissileTargetHeightAugment { get; set; } = 100;
        public bool MissileUnblockable { get; set; } = false;
        public bool NoWinddownIfCancelled { get; set; } = false;
        //NumSpellTargeters
        //OrientRadiusTextureFromPlayer
        //OrientRangeIndicatorToCursor
        //OrientRangeIndicatorToFacing
        public float OverrideCastTime { get; set; } = 0;
        public Vector3 ParticleStartOffset { get; set; } = new Vector3(0, 0, 0);
        //PlatformEnabled
        public string PointEffectName { get; set; } = "";
        //RangeIndicatorTextureName
        public string RequiredUnitTags { get; set; } = "";
        public string SelectionPreference { get; set; } = "";
        //Sound_CastName
        //Sound_HitName
        //Sound_VOEventCategory
        public float SpellCastTime { get; set; } = 0;
        public float SpellRevealsChampion { get; set; } = 0;
        public float SpellTotalTime { get; set; } = 0;
        public float StartCooldown { get; set; } = 0;
        public bool SubjectToGlobalCooldown { get; set; } = true;
        //TargeterConstrainedToRange
        public int TargettingType { get; set; } = 1;
        public string TextFlags { get; set; } = "";
        public bool TriggersGlobalCooldown { get; set; } = true;
        public bool UpdateRotationWhenCasting { get; set; } = true;
        public bool UseAnimatorFramerate { get; set; } = false;
        public bool UseAutoattackCastTime { get; set; } = false;
        public bool UseChargeChanneling { get; set; } = false;
        public bool UseChargeTargeting { get; set; } = false;
        public bool UseGlobalLineIndicator { get; set; } = false;
        public bool UseMinimapTargeting { get; set; } = false;
        //Version
        //x1,x2,x3,x4,x5
        
        public float GetCastTime()
        {
            return (1.0f + DelayCastOffsetPercent) / 2.0f;
        }

        public void Load(string name)
        {
            ContentFile file;
            try
            {
                var path = _game.Config.ContentManager.GetSpellDataPath(name);
                _logger.LogCoreInfo($"Loading spell {name} data from path: {Path.GetFullPath(path)}!");
                var text = File.ReadAllText(Path.GetFullPath(path));
                file = JsonConvert.DeserializeObject<ContentFile>(text);
            }
            catch (ContentNotFoundException notfound)
            {
                _logger.LogCoreWarning($"Spell data for {name} was not found.");
                return;
            }
            AffterEffectName = file.GetString("SpellData", "AffterEffectName", AffterEffectName);
            //AIEndOnly 
            //AILifetime 
            //AIRadius 
            //AIRange 
            //AISendEvent 
            //AISpeed
            AlternateName = file.GetString("SpellData", "AlternateName", name);
            AlwaysSnapFacing = file.GetBool("SpellData", "AlwaysSnapFacing", AlwaysSnapFacing);
            //AmmoCountHiddenInUI
            AmmoRechargeTime = file.GetMultiFloat("SpellData", "AmmoRechargeTime", 6, AmmoRechargeTime[0]);
            AmmoUsed = file.GetMultiInt("SpellData", "AmmoUsed", 6, AmmoUsed[0]);
            //AnimationLeadOutName
            //AnimationLoopName
            //AnimationName
            //AnimationWinddownName
            //ApplyAttackDamage
            //ApplyAttackEffect
            //ApplyMaterialOnHitSound
            BelongsToAvatar = file.GetBool("SpellData", "BelongsToAvatar", BelongsToAvatar);
            BounceRadius = file.GetFloat("SpellData", "BounceRadius", BounceRadius);
            CanCastWhileDisabled = file.GetBool("SpellData", "CanCastWhileDisabled", CanCastWhileDisabled);
            CancelChargeOnRecastTime = file.GetFloat("SpellData", "CancelChargeOnRecastTime", CancelChargeOnRecastTime);
            CanMoveWhileChanneling = file.GetBool("SpellData", "CanMoveWhileChanneling", CanMoveWhileChanneling);
            CannotBeSuppressed = file.GetBool("SpellData", "CannotBeSuppressed", CannotBeSuppressed);
            CanOnlyCastWhileDead = file.GetBool("SpellData", "CanOnlyCastWhileDead", CanOnlyCastWhileDead);
            CanOnlyCastWhileDisabled = file.GetBool("SpellData", "CanOnlyCastWhileDisabled", CanOnlyCastWhileDisabled);
            CantCancelWhileChanneling = file.GetBool("SpellData", "CantCancelWhileChanneling", CantCancelWhileChanneling);
            CantCancelWhileWindingUp = file.GetBool("SpellData", "CantCancelWhileWindingUp", CantCancelWhileWindingUp);
            CantCastWhileRooted = file.GetBool("SpellData", "CantCastWhileRooted", CantCastWhileRooted);
            CastConeAngle = file.GetFloat("SpellData", "CastConeAngle", CastConeAngle);
            CastConeDistance = file.GetFloat("SpellData", "CastConeDistance", CastConeDistance);
            CastFrame = file.GetFloat("SpellData", "CastFrame", CastFrame);
            CastRadius = file.GetMultiFloat("SpellData", "CastRadius", 6, CastRadius[0]);
            CastRadiusSecondary = file.GetMultiFloat("SpellData", "CastRadiusSecondary", 6, CastRadiusSecondary[0]);
            //CastRadiusSecondaryTexture
            //CastRadiusTexture
            CastRange = file.GetMultiFloat("SpellData", "CastRange", 6, CastRange[0]);
            CastRangeDisplayOverride = file.GetMultiFloat("SpellData", "CastRangeDisplayOverride", 6, CastRangeDisplayOverride[0]);
            CastRangeGrowthDuration = file.GetMultiFloat("SpellData", "CastRangeGrowthDuration", 6, CastRangeGrowthDuration[0]);
            CastRangeGrowthMax = file.GetMultiFloat("SpellData", "CastRangeGrowthMax", 6, CastRangeGrowthMax[0]);
            //CastRangeTextureOverrideName
            CastRangeUseBoundingBoxes = file.GetBool("SpellData", "CastRangeUseBoundingBoxes", CastRangeUseBoundingBoxes);
            CastTargetAdditionalUnitsRadius = file.GetFloat("SpellData", "CastTargetAdditionalUnitsRadius", CastTargetAdditionalUnitsRadius);
            CastType = file.GetInt("SpellData", "CastType", CastType);
            ChannelDuration = file.GetMultiFloat("SpellData", "ChannelDuration", 6, ChannelDuration[0]);
            ChargeUpdateInterval = file.GetFloat("SpellData", "ChargeUpdateInterval", ChargeUpdateInterval);
            CircleMissileAngularVelocity = file.GetFloat("SpellData", "CircleMissileAngularVelocity", CircleMissileAngularVelocity);
            CircleMissileRadialVelocity = file.GetFloat("SpellData", "CircleMissileRadialVelocity", CircleMissileRadialVelocity);
            //ClientOnlyMissileTargetBoneName
            //Coefficient
            //Coefficient2
            //ConsideredAsAutoAttack
            Cooldown = file.GetMultiFloat("SpellData", "Cooldown", 6, Cooldown[0]);
            //CursorChangesInGrass
            //CursorChangesInTerrain
            DeathRecapPriority = file.GetInt("SpellData", "DeathRecapPriority", DeathRecapPriority);
            DelayCastOffsetPercent = file.GetFloat("SpellData", "DelayCastOffsetPercent", DelayCastOffsetPercent);
            DelayTotalTimePercent = file.GetFloat("SpellData", "DelayTotalTimePercent", DelayTotalTimePercent);
            //Description
            //DisableCastBar
            //DisplayName
            DoesntBreakChannels = file.GetBool("SpellData", "DoesntBreakChannels", DoesntBreakChannels);
            DoNotNeedToFaceTarget = file.GetBool("SpellData", "DoNotNeedToFaceTarget", DoNotNeedToFaceTarget);
            //DrawSecondaryLineIndicator
            //DynamicExtended
            //string DynamicTooltip
            //EffectXLevelYAmmount
            Flags = file.GetInt("SpellData", "Flags", Flags);
            //FloatStaticsDecimalsX
            //FloatVarsDecimalsX
            HaveAfterEffect = file.GetBool("SpellData", "HaveAfterEffect", HaveAfterEffect);
            HaveHitBone = file.GetBool("SpellData", "HaveHitBone", HaveHitBone);
            HaveHitEffect = file.GetBool("SpellData", "HaveHitEffect", HaveHitEffect);
            HavePointEffect = file.GetBool("SpellData", "HavePointEffect", HavePointEffect);
            //HideRangeIndicatorWhenCasting
            HitBoneName = file.GetString("SpellData", "HitBoneName", HitBoneName);
            HitEffectName = file.GetString("SpellData", "HitEffectName", HitEffectName);
            HitEffectOrientType = file.GetInt("SpellData", "HitEffectOrientType", HitEffectOrientType);
            //HitEffectPlayerName
            IgnoreAnimContinueUntilCastFrame = file.GetBool("SpellData", "IgnoreAnimContinueUntilCastFrame", IgnoreAnimContinueUntilCastFrame);
            IgnoreRangeCheck = file.GetBool("SpellData", "IgnoreRangeCheck", IgnoreRangeCheck);
            //InventoryIconX
            IsDisabledWhileDead = file.GetBool("SpellData", "IsDisabledWhileDead", IsDisabledWhileDead);
            IsToggleSpell = file.GetBool("SpellData", "IsToggleSpell", IsToggleSpell);
            //KeywordWhenAcquired
            //LevelXDesc
            LineDragLength = file.GetFloat("SpellData", "LineDragLength", LineDragLength);
            LineMissileBounces = file.GetInt("SpellData", "LineMissileBounces", LineMissileBounces);
            LineMissileCollisionFromStartPoint = file.GetBool("SpellData", "LineMissileCollisionFromStartPoint", LineMissileCollisionFromStartPoint);
            LineMissileDelayDestroyAtEndSeconds = file.GetFloat("SpellData", "LineMissileDelayDestroyAtEndSeconds", LineMissileDelayDestroyAtEndSeconds);
            LineMissileEndsAtTargetPoint = file.GetBool("SpellData", "LineMissileEndsAtTargetPoint", LineMissileEndsAtTargetPoint);
            LineMissileFollowsTerrainHeight = file.GetBool("SpellData", "LineMissileFollowsTerrainHeight", LineMissileFollowsTerrainHeight);
            LineMissileTargetHeightAugment = file.GetFloat("SpellData", "LineMissileTargetHeightAugment", LineMissileTargetHeightAugment);
            LineMissileTimePulseBetweenCollisionSpellHits = file.GetFloat("SpellData", "LineMissileTimePulseBetweenCollisionSpellHits", LineMissileTimePulseBetweenCollisionSpellHits);
            LineMissileTrackUnits = file.GetBool("SpellData", "LineMissileTrackUnits", LineMissileTrackUnits);
            LineMissileUsesAccelerationForBounce = file.GetBool("SpellData", "LineMissileUsesAccelerationForBounce", LineMissileUsesAccelerationForBounce);
            //LineTargetingBaseTextureOverrideName
            //LineTargetingBaseTextureOverrideName
            LineWidth = file.GetFloat("SpellData", "LineWidth", LineWidth);
            LocationTargettingLength = file.GetMultiFloat("SpellData", "LocationTargettingLength", 6, LocationTargettingLength[0]);
            LocationTargettingWidth = file.GetMultiFloat("SpellData", "LocationTargettingWidth", 6, LocationTargettingWidth[0]);
            LockConeToPlayer = file.GetBool("SpellData", "LockConeToPlayer", LockConeToPlayer);
            //LookAtPolicy 
            LuaOnMissileUpdateDistanceInterval = file.GetFloat("SpellData", "LuaOnMissileUpdateDistanceInterval", LuaOnMissileUpdateDistanceInterval);
            ManaCost = file.GetMultiFloat("SpellData", "ManaCost",6, ManaCost[0]);
            //Map_X_EffectYLevelZAmmount
            MaxAmmo = file.GetMultiInt("SpellData", "MaxAmmo", 6, MaxAmmo[0]);
            //MaxGrowthRangeTextureName 
            //MinimapIcon
            //MinimapIconDisplayFlag
            //MinimapIconRotation
            MissileSpeed = file.GetFloat("SpellData", "MissileSpeed", MissileSpeed);
            MissileAccel = file.GetFloat("SpellData", "MissileAccel", MissileAccel);
            MissileBoneName = file.GetString("SpellData", "MissileBoneName", MissileBoneName);
            MissileEffect = file.GetString("SpellData", "MissileEffect", MissileEffect);
            MissileEffectPlayer = file.GetString("SpellData", "MissileEffectPlayer", MissileEffectPlayer);
            MissileFixedTravelTime = file.GetFloat("SpellData", "MissileFixedTravelTime", MissileFixedTravelTime);
            MissileFollowsTerrainHeight = file.GetBool("SpellData", "MissileFollowsTerrainHeight", MissileFollowsTerrainHeight);
            MissileGravity = file.GetFloat("SpellData", "MissileGravity", MissileGravity);
            MissileLifetime = file.GetFloat("SpellData", "MissileLifetime", MissileLifetime);
            MissileMaxSpeed = file.GetFloat("SpellData", "MissileMaxSpeed", MissileSpeed);
            MissileMinSpeed = file.GetFloat("SpellData", "MissileMinSpeed", MissileSpeed);
            MissileMinTravelTime = file.GetFloat("SpellData", "MissileMinTravelTime", MissileMinTravelTime);
            MissilePerceptionBubbleRadius = file.GetFloat("SpellData", "MissilePerceptionBubbleRadius", MissilePerceptionBubbleRadius);
            MissilePerceptionBubbleRevealsStealth = file.GetBool("SpellData", "MissilePerceptionBubbleRevealsStealth", MissilePerceptionBubbleRevealsStealth);
            //MissileSpeed = file.GetFloat("SpellData", "MissileSpeed", MissileSpeed);
            MissileTargetHeightAugment = file.GetFloat("SpellData", "MissileTargetHeightAugment", MissileTargetHeightAugment);
            MissileUnblockable = file.GetBool("SpellData", "MissileUnblockable", MissileUnblockable);
            NoWinddownIfCancelled = file.GetBool("SpellData", "NoWinddownIfCancelled", NoWinddownIfCancelled);
            //NumSpellTargeters
            //OrientRadiusTextureFromPlayer
            //OrientRangeIndicatorToCursor
            //OrientRangeIndicatorToFacing
            OverrideCastTime = file.GetFloat("SpellData", "OverrideCastTime", OverrideCastTime);
            //public Vector3 ParticleStartOffset { get; set; } = new Vector3(0, 0, 0);
            var _ParticleStartOffset = file.GetArray("SpellData", "ParticleStartOffset", new float[] { ParticleStartOffset.X, ParticleStartOffset.Y, ParticleStartOffset.Z});
            ParticleStartOffset = new Vector3(_ParticleStartOffset[0], _ParticleStartOffset[1], _ParticleStartOffset[2]);
            //PlatformEnabled
            PointEffectName = file.GetString("SpellData", "PointEffectName", PointEffectName);
            //RangeIndicatorTextureName
            RequiredUnitTags = file.GetString("SpellData", "RequiredUnitTags", RequiredUnitTags);
            SelectionPreference = file.GetString("SpellData", "SelectionPreference", SelectionPreference);
            //Sound_CastName
            //Sound_HitName
            //Sound_VOEventCategory
            SpellCastTime = file.GetFloat("SpellData", "SpellCastTime", SpellCastTime);
            SpellRevealsChampion = file.GetFloat("SpellData", "SpellRevealsChampion", SpellRevealsChampion);
            SpellTotalTime = file.GetFloat("SpellData", "SpellTotalTime", SpellTotalTime);
            StartCooldown = file.GetFloat("SpellData", "StartCooldown", StartCooldown);
            SubjectToGlobalCooldown = file.GetBool("SpellData", "SubjectToGlobalCooldown", SubjectToGlobalCooldown);
            //TargeterConstrainedToRange
            TargettingType = file.GetInt("SpellData", "TargettingType", TargettingType);
            TextFlags = file.GetString("SpellData", "TextFlags", TextFlags);
            TriggersGlobalCooldown = file.GetBool("SpellData", "TriggersGlobalCooldown", TriggersGlobalCooldown);
            UpdateRotationWhenCasting = file.GetBool("SpellData", "UpdateRotationWhenCasting", UpdateRotationWhenCasting);
            UseAnimatorFramerate = file.GetBool("SpellData", "UseAnimatorFramerate", UseAnimatorFramerate);
            UseAutoattackCastTime = file.GetBool("SpellData", "UseAutoattackCastTime", UseAutoattackCastTime);
            UseChargeChanneling = file.GetBool("SpellData", "UseChargeChanneling", UseChargeChanneling);
            UseChargeTargeting = file.GetBool("SpellData", "UseChargeTargeting", UseChargeTargeting);
            UseGlobalLineIndicator = file.GetBool("SpellData", "UseGlobalLineIndicator", UseGlobalLineIndicator);
            UseMinimapTargeting = file.GetBool("SpellData", "UseMinimapTargeting", UseMinimapTargeting);
        }
    }
}
