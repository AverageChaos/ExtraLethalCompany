# Extra Lethal Company
A work in progress mod that aims to do two main tasks: adding extra, missing, or needed feature/changes to Lethal Company, and making the company **EXTRA LETHAL!**  
[Report A Bug](https://github.com/AverageChaos/ExtraLethalCompany/issues)

# Features
- ***Modifications To Existing Mods:***
    - Bigger Battery (Increases charger explosion chance)
    - Minimap        (Hides enemies)
    - NoPenalty      (Disables and increases penalty)
    - RouteRandom    (Disabled)
- ***Fines:***
    - Fines UI now displays the percentage cost of casualties and injuries.
    - Fines larger than the player's balance are added to the quota.
    - Fines UI will display the amount paid, and the amount DUE (added to your quota).
- ***Screens:***
    - Weather conditions have been replaced with "{Sensor Err}"
- ***Ship:***
    - Has a 10% chance to leave a hour after having a power surge (struck by lightning)
- ***Chargers:***
    - Charging animation now plays on all clients
    - Have a chance to explode when charging an item  and cause a power surge **(see table)**
    - Conductive items can be placed in the charger
    - Chargers break for the round after exploding

| Battery % | Explosion Chance |
| :-------: | :--------------: |
|     0%    |        9%        |
|    10%    |       16%        |
|    20%    |       23%        |
|    30%    |       28%        |
|    40%    |       33%        |
|    50%    |       37%        |
|    60%    |       41%        |
|    70%    |       44%        |
|    80%    |       47%        |
|    90%    |       50%        |
|   100%    |       52%        |
- ***Quota:***
    - Buying rate starts at 115% (1.15), decreases by `.45 / TotalDaysToFulfillQuota` per day **(see table for example)**

| Days Left | Sell Price |
| :-------: | :--------: |
|     3     |     115%   |
|     2     |     100%   |
|     1     |      85%   |
|     0     |      70%   |

- ***Monsters:***
    - Slime grows **1.05x** per scrap eaten, and **1.1x** per player eaten
    - Spore lizard's spores now deal DoT

# Installation
1. Install **BepInEx** (see [BepInEx Installation Guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html))

2. Launch **Lethal Company** _once_ with **BepInEx** installed to generate necessary mod folders and files

3. Navigate to your **Lethal Company** game directory and go to `./BepInEx/plugins`
    - The `BepInEx` and `plugins` folder should have been generated in the previous step
    - **Example:** `C:\Program Files\Steam\steamapps\common\Lethal Company\BepInEx\plugins`

4. Download the mod, unzip it, and navigate to `./BepInEx/plugins`

5. Copy the **DLL file** to the `plugins` folder

6. Launch **Lethal Company** and enjoy the mod

# Changelog
**For a full list of changes see our [Github Commits](https://github.com/AverageChaos/ExtraLethalCompany/commits/master/)**

***v1.2.3***
- Updated chargers
