import time
import random
from player import Player
from objects import Object

#make factory...

def get_players():
    return [
        Player(0, 0, 0),
        Player(1, 2, 2)
    ]

def update_players(players):
    for player in players:
        player.update()

def get_objects():
	random.seed(time.gmtime)
	objects = [];

	for i in range(10):
		x = random.randint(-10, 10)
		y = random.randint(-10, 10)

		if not (x == 2 and y == 2) and not (x == 0 and y == 0):
			objects.append(Object(i, x, y))

	return objects

def update_objects(objects):
    for obj in objects:
        obj.update()
