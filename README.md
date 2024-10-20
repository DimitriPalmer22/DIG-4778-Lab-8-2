### How the design patterns are implemented
- Object Pool Pattern: Each time the player shoots, a projectile object is pulled from the object pool and enabled instead of being instantiated. When the projectiles are no longer being used, they are disabled.
- Builder Pattern: Instead of using a list of prefabs, the enemies are built. Each build has its own sprite, color, size, speed, and health. The enemies can also be built completely randomly.
- Observer Pattern: There is an event in the Enemy script that is called whenever the enemy takes damage. The score manager is subscribed to this event so that the score value increases and the score UI updates.
