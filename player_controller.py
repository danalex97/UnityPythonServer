from player import Player
from objects import Object

class PlayerController:
    def __init__(self):
        self.new = []
        self.deleted = []

    def get_new_players(self):
        to_ret = self.new[:]
        self.new = []
        return to_ret

    def get_deleted_players(self):
        to_ret = self.new[:]
        self.new = []
        return to_ret

    def add_player(self, player):
        self.new.append(player)

    def delete_player(self, id):
        self.deleted.append(id)
