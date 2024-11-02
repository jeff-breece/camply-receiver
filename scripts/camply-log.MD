### POC Research
- https://www.freecodecamp.org/news/python-requirementstxt-explained/
- https://learn.microsoft.com/en-us/visualstudio/python/managing-required-packages-with-requirements-txt


### Basic Queries

camply campgrounds --search "Scioto Trail State Park" --provider OhioStateParks

camply list-campsites --campground 553 --provider OhioStateParks

camply campsites --campground 553 --provider OhioStateParks --start-date 2024-10-31 --end-date 2024-11-30

camply campsites \
  --provider OhioStateParks \
  --campground 554 \
  --campsite 23739 \
  --start-date 2024-09-14 \
  --end-date 2024-09-15

camply campsites --provider OhioStateParks --campground 554 --start-date 2024-09-14 --end-date 2024-09-15