class Object:
    def __init__(self, id, x, y):
        self.id = id
        self.x = x
        self.y = y

        self._move_sequence = [(-1, 0), (0, -1), (0, -1), (0, 1), (1, 0), (0, 1)]
        self._move_pos = 0

    def json(self):
        return  {
            "id" : self.id,
            "x" : self.x,
            "y" : self.y
        }

    def update(self):
        pass
