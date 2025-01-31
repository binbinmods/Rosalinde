# <Charactername>, the <Subclass>

A hero mod, introducing <CharacterName>, a Shaman imprisoned by Gorio. 

This currently does not include any events or quests related to <CharacterName>. This will be updated in the future with a future mod release, it will include a sub-zone or three and a few quests. 

A couple of notes:
## Notes:
- I understand that things are going to be janky at times, and there are definitely bugs that will be worked out
- **What to do if <CharacterName> is not unlocked:** Due to some jankiness of the way the code works, <CharacterName> is unlocked only for the profile that is open when you launch the game (and for new profiles). So if they aren't unlocked in the correct profile, switch to that profile, close the game and re-open it and they will be unlocked. I'll fix this in the future, but most people won't notice it. You can also just use the profile editor to fix it.
- There are **no character events** for <CharacterName> at this time beyond the ones that are available to all characters of a given class (such as pet trainers or healers being able to remove cards at Rest areas).
- <CharacterName>'s selection location (in the Hero Selection screen) is intentionally in position 5 (the far right). I have not yet automated the process of placing characters, and this is to accommodate other heroes. If you wish to change this, you can access the `<subclass>.json` file and the `OrderInList` property with whatever you wish.

This mod relies on [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).

<details>
<summary>Traits</summary>

### Level 1
- Wisdom of The Ancients: Burn, Chill, and Spark Charges on enemies additionally apply -0.2% resistance to Holy Damage per charge. At the end of your turn, all heroes heal for 12% of the Burn Charges, Chill Charges, and Shock Charges in play. -This heal does not gain bonuses-


### Level 2

![Augur's Wrath](/Storm_Channeler.png)

![Augur's Blessing](/Waters_of_Life.png)

### Level 3

- Magus Duality: When you play a Mage Card, reduce the cost of the highest cost Healer Card in your hand by 1 until discarded. When you play a Healer Card, reduce the cost of the highest cost Mage Card in your hand by 1 until discarded. (3 times / per turn)
- Animist Energy: At the start of your turn, Dispel 3 targeting yourself, and reduce the cost of the highest cost card in your hand by 2 until discarded.

### Level 4

![Elemental Focus](./Electric_Current.png)

![Bountiful Restoration](../Tears_of_the_Spririts.png)

### Level 5

- Yggdrasil's Fury: Sanctify +1. All Damage done +20%. When you play a "Spell" card, Dispel 1 targeting yourself. (4 times / per turn).
- Grove Warden's Mercy: Regeneration +1. When you play a "Healing Spell" card, Apply 2 Mitigate Charges to All Heroes. (2 times / per turn)

</details>


## Installation (manual)

1. Install [Obeliskial Essentials](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Essentials/) and [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).
2. Click _Manual Download_ at the top of the page.
3. In Steam, right-click Across the Obelisk and select _Manage_->_Browse local files_.
4. Extract the archive into the game folder. Your _Across the Obelisk_ folder should now contain a _BepInEx_ folder and a _doorstop\_libs_ folder.
5. Run the game. If everything runs correctly, you will see this mod in the list of registered mods on the main menu.
6. Press F5 to open/close the Config Manager and F1 to show/hide mod version information.
7. Note: I am not certain about these install instructions. In the worst case, just copy _TheWiseWolf.dll_ into the _BepInEx\plugins_ folder, and the _<CharacterName>_ folder (the one with the subfolders containing the json files) into _BepInEx\config\Obeliskial\_importing_

## Installation (automatic)

1. Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://across-the-obelisk.thunderstore.io/package/ebkr/r2modman/).
2. Click **Install with Mod Manager** button on top of the page.
3. Run the game via the mod manager.

## Support

This has been updated for version 1.4.

Hope you enjoy it and if have any issues, ping me in Discord or make a post in the **modding #support-and-requests** channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701).

## Donation

Please do not donate to me. If you wish to support me, I would prefer it if you just gave me feedback. 