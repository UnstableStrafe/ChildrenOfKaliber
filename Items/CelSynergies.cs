using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    internal class CelSynergies
    {
        public class ExtravaganceSyn : AdvancedSynergyEntry
        {
            public ExtravaganceSyn()
            {
                this.NameKey = "Extravagance";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Auric Vial"].PickupObjectId,
                    532
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class RulebookSyn : AdvancedSynergyEntry
        {

            public RulebookSyn()
            {
                this.NameKey = "Rulebook";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Mininomocon"].PickupObjectId,
                    521
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class TrueDragunfireSyn : AdvancedSynergyEntry
        {

            public TrueDragunfireSyn()
            {
                this.NameKey = "True Dragunfire";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Dragun Skull"].PickupObjectId,
                    ETGMod.Databases.Items["Dragun Heart"].PickupObjectId,
                    ETGMod.Databases.Items["Dragun Wing"].PickupObjectId,
                    ETGMod.Databases.Items["Dragun Claw"].PickupObjectId
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
                this.NumberObjectsRequired = 4;
                
            }
        }

        public class SturdyAmmoSyn : AdvancedSynergyEntry
        {
            public SturdyAmmoSyn()
            {
                this.NameKey = "Sturdy Ammo Bag";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Terrible Ammo Bag"].PickupObjectId,
                    116
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class LeadWeightSyn : AdvancedSynergyEntry
        {
            public LeadWeightSyn()
            {
                this.NameKey = "Lead Weight";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Gravity Well Module"].PickupObjectId,
                    111
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class TippedPoiSyn : AdvancedSynergyEntry
        {
            public TippedPoiSyn()
            {
                this.NameKey = "Tipped Arrows: Poison";
                this.MandatoryItemIDs = new List<int>
                {
                    
                    204
                };
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["Dispenser"].PickupObjectId

                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class DragunRoarSyn : AdvancedSynergyEntry
        {
            public DragunRoarSyn()
            {
                this.NameKey = "Dragun Roar";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Dragun Skull"].PickupObjectId
                    
                };
                this.MandatoryGunIDs = new List<int>
                {
                    146

                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class IronManSyn : AdvancedSynergyEntry
        {
            public IronManSyn()
            {
                this.NameKey = "Iron Man";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Iron Heart"].PickupObjectId,
                    314
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class EldritchLoveSyn : AdvancedSynergyEntry
        {
            public EldritchLoveSyn()
            {
                this.NameKey = "Eldritch Love";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Void Bottle"].PickupObjectId,
                    631
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class TwoForOneSyn : AdvancedSynergyEntry
        {
            public TwoForOneSyn()
            {
                this.NameKey = "Two For One";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Cup Of Water"].PickupObjectId
                };
                this.OptionalItemIDs = new List<int>
                {
                    170,
                    278
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();

            }
        }

        public class CriticalMassSyn : AdvancedSynergyEntry
        {
            public CriticalMassSyn()
            {
                this.NameKey = "Critical Mass";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Gravity Well Module"].PickupObjectId
                };
                this.OptionalItemIDs = new List<int>
                {
                    155,
                    169
                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();

            }
        }
        public class RerollSyn : AdvancedSynergyEntry
        {
            public RerollSyn()
            {
                this.NameKey = "Reroll!";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["D6"].PickupObjectId

                };
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["R.G.G."].PickupObjectId

                };
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }

        public class OverclockedSyn : AdvancedSynergyEntry
        {
            public OverclockedSyn()
            {
                this.NameKey = "Overclocked";
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["Incremental"].PickupObjectId
                };
                this.OptionalItemIDs = new List<int>
                {
                    280,
                    134
                };
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class SovietSkillsSyn : AdvancedSynergyEntry
        {
            public SovietSkillsSyn()
            {
                this.NameKey = "Soviet Skills";
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["russian_revolver"].PickupObjectId,
                    2
                };

                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class CommunistIdealsSyn : AdvancedSynergyEntry
        {
            public CommunistIdealsSyn()
            {
                this.NameKey = "Communist Ideals";
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["russian_revolver"].PickupObjectId,
                    
                };
                this.OptionalItemIDs = new List<int>
                {
                    116,
                    134,
                    131
                };
                this.NumberObjectsRequired = 2;
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class LuckySpinSyn : AdvancedSynergyEntry
        {
            public LuckySpinSyn()
            {
                this.NameKey = "Lucky Spin";
                this.MandatoryGunIDs = new List<int>
                {
                    ETGMod.Databases.Items["russian_revolver"].PickupObjectId,
                    
                };
                MandatoryItemIDs = new List<int>
                {
                    289
                };
                this.IgnoreLichEyeBullets = false;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
        public class TestGunSyn : AdvancedSynergyEntry
        {
            public TestGunSyn()
            {
                this.NameKey = "etgmod:test_gun";
                this.MandatoryItemIDs = new List<int>
                {
                    ETGMod.Databases.Items["Mininomocon"].PickupObjectId

                };
                this.OptionalGunIDs = new List<int>
                {
                    31,
                    690
                };
                this.RequiresAtLeastOneGunAndOneItem = true;
                this.NumberObjectsRequired = 2;
                this.IgnoreLichEyeBullets = true;
                this.statModifiers = new List<StatModifier>(0);
                this.bonusSynergies = new List<CustomSynergyType>();
            }
        }
    }
}
