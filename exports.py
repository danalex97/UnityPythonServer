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
    return [
        Object(10, 4, -4),
        Object(11, -4, 4)
    ]

def update_objects(objects):
    for obj in objects:
        obj.update()
