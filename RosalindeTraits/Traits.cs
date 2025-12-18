using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using UnityEngine;
using static Rosalinde.CustomFunctions;
using static Rosalinde.Plugin;
using UnityEngine.AI;

namespace Rosalinde
{
    [HarmonyPatch]
    internal class Traits
    {
        // list of your trait IDs
        public static string heroName = "rosalinde";

        public static string subclassname = "augur";

        public static string[] myTraitList = ["augurtrait0", "augurtrait1a", "augurtrait1b", "augurtrait2a", "augurtrait2b", "augurtrait3a", "augurtrait3b", "augurtrait4a", "augurtrait4b"];

        // public static string[] myTraitList = (string[])simpleTraitList.Select(trait=>subclassname+trait); // Needs testing

        static string trait0 = myTraitList[0];
        static string trait2a = myTraitList[3];
        static string trait2b = myTraitList[4];
        static string trait4a = myTraitList[7];
        static string trait4b = myTraitList[8];


        public static string debugBase = "Binbin - Testing " + heroName + " ";

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Trait), "DoTrait")]
        public static bool DoTrait(Enums.EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard, ref Trait __instance)
        {
            if ((UnityEngine.Object)MatchManager.Instance == (UnityEngine.Object)null)
                return false;
            if (Content.medsCustomTraitsSource.Contains(_trait) && myTraitList.Contains(_trait))
            {
                DoCustomTrait(_trait, ref __instance, ref _theEvent, ref _character, ref _target, ref _auxInt, ref _auxString, ref _castedCard);
                return false;
            }
            return true;
        }

        public static void DoCustomTrait(string _trait, ref Trait __instance, ref Enums.EventActivation _theEvent, ref Character _character, ref Character _target, ref int _auxInt, ref string _auxString, ref CardData _castedCard)
        {
            // get info you may need
            TraitData traitData = Globals.Instance.GetTraitData(_trait);
            List<CardData> cardDataList = [];
            List<string> heroHand = MatchManager.Instance.GetHeroHand(_character.HeroIndex);
            Hero[] teamHero = MatchManager.Instance.GetTeamHero();
            NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();

            if (!IsLivingHero(_character))
            {
                return;
            }

            if (_trait == trait0)
            { // Burn, Chill, and Spark Charges on enemies additionally apply -0.2% resistance to Holy Damage per charge. 
              // At the end of your turn, all heroes heal for 11% of the Burn Charges, Chill Charges, and Shock Charges in play. -This heal does not gain bonuses-
                string traitName = traitData.TraitName;
                LogDebug($"Trait {_trait}");
                int nCharges = CountAllStacks("burn", teamHero, teamNpc);
                nCharges += CountAllStacks("chill", teamHero, teamNpc);
                nCharges += CountAllStacks("spark", teamHero, teamNpc);
                LogDebug($"{traitName}: nCharges = {nCharges}");
                int amountToHeal = Mathf.RoundToInt(nCharges * 0.11f);
                for (int i = 0; i < teamHero.Length; i++)
                {
                    Hero hero = teamHero[i];
                    if (!IsLivingHero(hero))
                        continue;

                    TraitHealHero(ref _character, ref hero, amountToHeal, traitName);
                }
                // LogInfo($"Trait {_trait} end");

            }


            else if (_trait == trait2a)
            { // When you play a Mage Card, reduce the cost of the highest cost Healer Card in your hand by 1 until discarded. When you play a Healer Card, reduce the cost of the highest cost Mage Card in your hand by 1 until discarded. (3 times / per turn)
                string traitName = traitData.TraitName;
                LogDebug($"Trait {_trait}: {traitName}");
                Duality(ref _character, ref _castedCard, Enums.CardClass.Mage, Enums.CardClass.Healer, _trait);
            }



            else if (_trait == trait2b)
            { // At the start of your turn, Dispel 3 targeting yourself, 
              // reduce the cost of the highest cost card in your hand by 2 until discarded.
                string traitName = traitData.TraitName;
                LogDebug($"Trait {_trait}: {traitName}");
                // LogDebug($"Trait {_trait} pre");
                CardData highCard = GetRandomHighestCostCard(Enums.CardType.None);
                int amountToReduce = 2;
                // LogDebug($"Trait {_trait} gotcard");
                ReduceCardCost(ref highCard, _character, amountToReduce);
                // LogDebug($"Trait {_trait} postreduce");
                _character.HealCurses(2);
                // LogDebug($"Trait {_trait} end");
                // DisplayTraitScroll(ref _character, traitData);
            }

            else if (_trait == trait4a)
            { // When you play a \"Spell\" card, Dispel 1 targeting yourself. (4 times / per turn)
                string traitName = traitData.TraitName;
                LogDebug($"Trait {_trait}: {traitName}");
                if (CanIncrementTraitActivations(_trait) && _castedCard.HasCardType(Enums.CardType.Spell))
                {
                    _character.HealCurses(1);
                    IncrementTraitActivations(_trait);

                }
            }

            else if (_trait == trait4b)
            { // When you play a \"Healing Spell\" card, Apply 2 Mitigate Charges to All Heroes. (2 times / per turn)
                string traitName = traitData.TraitName;
                LogDebug($"Trait {_trait}: {traitName}");
                if (CanIncrementTraitActivations(_trait))
                {
                    ApplyAuraCurseToAll("mitigate", 2, AppliesTo.Heroes, _character, useCharacterMods: true);
                    IncrementTraitActivations(_trait);

                }


            }

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {
            // LogInfo($"GACM {subclassName}");
            // Burn, Chill, and Spark Charges on enemies additionally apply -0.2% resistance to Holy Damage per charge. 

            Character characterOfInterest = _type == "set" ? _characterTarget : _characterCaster;
            string traitOfInterest;

            switch (_acId)
            {
                case "burn":
                    traitOfInterest = trait0;
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.Monsters))
                    {
                        LogDebug($"Trait {traitOfInterest} - GACM");
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Holy, 0, -0.2f);
                    }
                    break;

                case "chill":
                    traitOfInterest = trait0;
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.Monsters))
                    {
                        LogDebug($"Trait {traitOfInterest} - GACM");
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Holy, 0, -0.2f);
                    }
                    break;

                case "spark":
                    traitOfInterest = trait0;
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.Monsters))
                    {
                        LogDebug($"Trait {traitOfInterest} - GACM");
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Holy, 0, -0.2f);
                    }
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "CreateGameContent")]
        public static void CreateGameContentPostfix()
        {
            SubClassData eve = Globals.Instance?.GetSubClassData("elementalist");
            SubClassData rosalinde = Globals.Instance?.GetSubClassData("augur");

            if (!eve)
            {
                LogDebug("CreateGameContentPostfix - Null Eve");
                return;
            }
            if (!rosalinde)
            {
                LogDebug("CreateGameContentPostfix - Null Rosalinde");
                return;
            }

            AudioClip hitSound = eve.GetHitSound();
            List<AudioClip> hitSoundRework = Traverse.Create(eve).Field("hitSoundRework").GetValue<List<AudioClip>>();

            LogDebug("Eve hitsound name: " + hitSound?.name ?? "null");
            LogDebug("Eve hitsoundrework size: " + hitSoundRework?.Count ?? "null");

            Traverse.Create(rosalinde).Field("hitSound").SetValue(hitSound);
            Traverse.Create(rosalinde).Field("hitSoundRework").SetValue(hitSoundRework);

            // LogDebug("CreateGameContentPostfix - Shifting gameObjectAnimated");
            // Vector3 lp = rosalinde.GameObjectAnimated.transform.localPosition;
            // LogDebug($"CreateGameContentPostfix - World Position {rosalinde.GameObjectAnimated.transform.position}");
            // LogDebug($"CreateGameContentPostfix - Current Local Position {lp}, shifting by {XOffset.Value}, {YOffset.Value}");

            // rosalinde.GameObjectAnimated.transform.localPosition = new Vector3(lp.x + XOffset.Value + 1, lp.y + YOffset.Value, lp.z);
            // LogDebug($"CreateGameContentPostfix - New World Position {rosalinde.GameObjectAnimated.transform.position}");
            // LogDebug($"CreateGameContentPostfix - New Local Position {lp}, shifting by {XOffset.Value}, {YOffset.Value}");


            Dictionary<string, SubClassData> _SubClass = Traverse.Create(Globals.Instance).Field("_SubClass").GetValue<Dictionary<string, SubClassData>>();
            _SubClass["augur"] = rosalinde;
            Traverse.Create(Globals.Instance).Field("_SubClass").SetValue(_SubClass);

            LogDebug("CreateGameContentPostfix - Set changes");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeroItem), "Init")]
        public static void InitPostfix(ref HeroItem __instance)
        {
            LogDebug($"Init HeroItem for {__instance.Hero.SubclassName}");
            if (__instance.Hero.SubclassName.ToLower() != "augur")
            {
                return;
            }

            Vector3 lp = __instance.animatedTransform.localPosition;
            LogDebug($"InitPostfix - World Position {__instance.animatedTransform.position}");
            LogDebug($"InitPostfix - Current Local Position {lp}, shifting by {XOffset.Value}, {YOffset.Value}");

            // rosalinde.GameObjectAnimated.transform.localPosition = new Vector3(lp.x + XOffset.Value + 1, lp.y + YOffset.Value, lp.z);

            __instance.animatedTransform.localPosition = new Vector3(lp.x + XOffset.Value * 0.01f, lp.y + YOffset.Value * 0.01f, lp.z);
            LogDebug($"InitPostfix - New World Position {__instance.animatedTransform.position}");
            LogDebug($"InitPostfix - New Local Position {lp}, shifting by {XOffset.Value}, {YOffset.Value}");
        }

    }
}
