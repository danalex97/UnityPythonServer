class Player:
    def __init__(self, id, x, y):
        self.id = id
        self.x = x
        self.y = y

        self._move_sequence = [(-1, 0), (0, -1), (1, 0), (0, 1)]
        self._move_pos = 0

    def json(self):
        return  {
            "id" : self.id,
            "x" : self.x,
            "y" : self.y
        }

    def _move_dummy(self):
        if self._move_pos == len(self._move_sequence):
            self._move_pos = 0

        self.x += self._move_sequence[self._move_pos][0]
        self.y += self._move_sequence[self._move_pos][1]

        self._move_pos += 1

    def update(self):
        self._move_dummy()

def get_players():
    return [
        Player(0, 0, 0),
        Player(1, 5, 5)
    ]

def update_players(players):
    for player in players:
        player.update()
