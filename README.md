# Tactical FPS

A tactical first-person shooter built in Unity. The objective is to eliminate all enemy AIs, which come in two types: Sniper and Pistol. Enemies do not stand still. They check multiple predefined angles and positions, so movement and positioning matter.

## Features

- Two enemy AI types: Sniper and Pistol
- Enemies check different predefined angles and positions
- Initial map overview showing AI positions and overall map data
- Utility equipment: smoke grenades, flashbangs, night vision
- Leaning (Q / E) to look around walls and corners
- Visibility system: map lights increase visibility and bushes decrease it, which changes how easily AIs detect the player
- Healing using medkits

## Controls

| Input | Action |
|-------|--------|
| W A S D | Walk |
| Shift | Run |
| Ctrl | Crouch |
| Space | Jump |
| Q / E | Lean left / right |
| N | Toggle night vision |
| M | Open / close map |
| V | Use heal |
| Right Mause Button | Shoot |
| Left Mause Button | Aim Down Sights |
| G | Throw selected nade |
| 1 2 3 | Select nade |

## Gameplay Notes

- Check the map at the start to see AI positions before engaging.
- Staying in bushes lowers your visibility; standing in lit areas raises it.
- Smokes and flashes can be used to break enemy line of sight or disrupt enemies.
- Leaning lets you peek around walls while exposing less of the player.

## Requirements

- Unity `[version]`

## Setup

1. Clone the repository:
   ```
   git clone [repository-url]
   ```
2. Open the project in Unity.
3. Open the main scene: `[path/to/scene]`.
4. Press Play.

## Tech Stack

- Unity
- C#
- Custom AI systems
