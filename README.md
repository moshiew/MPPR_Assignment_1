# Project Details
## Roles and Responsibilities
Akmal
- Tower
    - Drag & drop
    - Tower placement based on map
    - Tower attributes
    - Virtual Currency (Work with Khai Xuen)

Khai Xuen
- Enemy
    - Enemy attributes
    - Integrate with pathfinding (Work with Kai Xiang)
    - Enemy animations (Interpolation)
    - Waves implementation
    - Virtual Currency (Work with Akmal)

Kai Xiang
- UI
  - Camera
  - Health bar, UI for virtual currency
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
- int playerHealth;
- int coins;

## Universal
- vector3 pStart;
- vector3 pEnd;
- vector3 pCtrl{number};
