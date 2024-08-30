# Setup The Job
I will be using CRON on a Linux VM to initially test this locally then figuring out an architecture for a bot to initiate this script run with parameters.

crontab -e
0 13 * * * /usr/bin/python3 /home/jeff/src/Camply.Receiver/scripts/call-webhook.py
0 20 * * * /usr/bin/python3 /home/jeff/src/Camply.Receiver/scripts/call-webhook.py

# Providers
I am using the Reserve Ohio for my searches however CAMPLY supports a full range of options.

# Search For Park Info
```bash
camply campgrounds --search "Scioto Trail State Park" --provider OhioStateParks
```

## Site List for a Park
```bash
camply list-campsites --campground 554 --provider OhioStateParks
```

### Output
```bash
[2024-08-29 12:11:55] INFO     🏕  Stewart Lake Campground - (#554)              
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #056 - (#23732)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #057 - (#23733)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #058 - (#23734)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #059 - (#23735)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #060 - (#23736)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #061 - (#23737)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #062 - (#23738)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #063 - (#23739)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #064 - (#23740)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #065 - (#23741)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #066 - (#23742)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #067 - (#23743)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #068 - (#23744)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #069 - (#23745)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #070 - (#23746)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #071 - (#23747)     
[2024-08-29 12:11:55] INFO         ⛺️ Campsite non-electric #072 - (#23748)
```

# Search For a Specific Site
Command Line
```bash
camply campsites   --provider OhioStateParks   --campground 554   --campsite 23738   --start-date 2024-08-31   --end-date 2024-09-01
```

Pretty View
```bash
camply campsites \
  --provider OhioStateParks \
  --campground 554 \
  --campsite 23738 \
  --start-date 2024-08-31 \
  --end-date 2024-09-01
```

## Slack Example
```bash
camply campsites   --provider OhioStateParks   --campground 554   --campsite 23738   --start-date 2024-08-31   --end-date 2024-09-01 --notifications slack
```

## Webhook Example
Raw Output
```json
{"stdout": "[2024-08-30 16:29:54] CAMPLY   camply, the campsite finder \u26fa\ufe0f                                                        \n[2024-08-30 16:29:54] INFO     Using Camply Provider: \"OhioStateParks\"                                               \n[2024-08-30 16:29:54] INFO     1 booking nights selected for search, ranging from 2024-08-31 to 2024-08-31           \n[2024-08-30 16:29:54] INFO     Searching Specific campsite: Campsite non-electric #062                               \n[2024-08-30 16:29:54] INFO     Searching across 1 campgrounds                                                        \n[2024-08-30 16:29:54] INFO         \u26f0  Scioto Trail State Park (#375) - \ud83c\udfd5  Stewart Lake Campground (#554)             \n[2024-08-30 16:29:54] INFO     Searching Stewart Lake Campground, Scioto Trail State Park (554) for availability:    \n                               August, 2024                                                                          \n[2024-08-30 16:29:55] INFO             \u26fa\ufe0f      1 total sites found in month of August                                \n[2024-08-30 16:29:55] INFO     \u26fa\ufe0f \u26fa\ufe0f \u26fa\ufe0f \u26fa\ufe0f 1 Reservable Campsites Matching Search Preferences                        \n[2024-08-30 16:29:55] INFO     \ud83d\udcc5 Sat, August 31 \ud83c\udfd5  1 sites                                                          \n[2024-08-30 16:29:55] INFO             \u26f0\ufe0f  Scioto Trail State Park  \ud83c\udfd5  Stewart Lake Campground: \u26fa 1 sites            \n[2024-08-30 16:29:55] INFO                     \ud83d\udd17 https://www.OhioStateParks.com/OhioCampWeb#!park/375/554 (1 night) \n[2024-08-30 16:29:55] CAMPLY   Exiting camply \ud83d\udc4b                                                                     \n", "stderr": "", "returncode": 0}
```