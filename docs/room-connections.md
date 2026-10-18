# Room connections

Read from `map-with-names.png`. Stairways and corridors are rooms of their own (see `RoomEnum`).
Every connection is two-way, but `Room.AdjacentRooms` is directed, so both sides must be added (`RoomFactory.Connect` does this).

## Top deck
- Crow's Nest: Main Deck
- Main Deck: Crow's Nest, Forecastle, Quarterdeck, Charts Room, Front Stairway, Rear Stairway
- Forecastle: Main Deck
- Quarterdeck: Main Deck
- Charts Room: Main Deck, Front Stairway
- Captain's Quarters: Rear Stairway
- Front Stairway: Main Deck, Charts Room, Mess
- Rear Stairway: Main Deck, Captain's Quarters, Middle Corridor

No door: Forecastle / Charts Room, Quarterdeck / Captain's Quarters (solid walls).

## Middle deck
- Mess: Front Stairway, Sick Bay, Galley, Crew's Quarters, Middle Corridor
- Sick Bay: Mess, Brig
- Brig: Sick Bay
- Crew's Quarters: Mess
- Magazine: Middle Corridor
- Galley: Mess, Middle Corridor
- Middle Corridor: Rear Stairway, Mess, Galley, Magazine, Officers' Quarters, Laundry Room, Lower Stairway
- Officers' Quarters: Middle Corridor
- Laundry Room: Middle Corridor
- Lower Stairway: Middle Corridor, Lower Corridor

## Lower deck
- Lower Corridor: Lower Stairway, Workshop, Armory, Engine Room, Boiler Room, Hold
- Workshop, Armory, Engine Room, Boiler Room: Lower Corridor
- Hold: Lower Corridor, Coal
- Coal: Hold, Food
- Food: Coal

## Needs confirmation
Door that was hard to read at map resolution:
- Coal / Food
