# Project Details
## Roles and Responsibilities
Akmal
- Tower
    - Drag & drop
    - Tower placement based on map
    - Tower attributes (Health work with Kai Xiang)
    - Virtual Currency (Work with Khai Xuen)

Khai Xuen
- Enemy
    - Enemy attributes (Health work with Kai Xiang)
    - Integrate with pathfinding (Work with Kai Xiang)
    - Enemy animations (Interpolation)
    - Waves implementation
    - Virtual Currency (Work with Akmal)

Kai Xiang
- UI
  - Camera
  - Health bar (Health work with Khai Xuen, Akmal), UI for virtual currency
  - Main Menu
  - Pathfinding (Work with Khai Xuen)
  - UI animations (Interpolation)
 
## Variables
### Tower (Akmal)
- int projectileDmg;
- float projectileSpd;
  
  
### Enemy (Khai Xuen)
- int enemyHealth;
- int enemySpd;
  
### UI (Kai Xiang)
- int playerHealth = 100;
- int coins = 0;

## Universal
- vector3 p0 - p(n);
