# Move To Live
Unity Game

Span of Project: 10/14-20/19

**Game Background**  
Collect items to kill enemies. Stay alive as long as you can!

**Where To Play**  
https://simmer.io/@apkirito/movetolive

**How To Play**
- WASD to move
- Space while playing to clear all enemies
- Space when dead to restart

**Script Accomplishments**
- Used a Unity's Event system to orchestrate movements.

**Notes**
- Chaining resolution simulates a Linked List where collided nodes are linked next to each other.
- Probing resolution follows the rule of incrementing an index until an empty index is found.
- The Load Capacity is set to .75, so the Hashset will rehash when #nodes/#buckets > .75.
- The bucket size increases linearly.
