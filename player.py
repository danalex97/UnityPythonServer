from objects import Object

class Player(Object):
    def _move_dummy(self):
        if self._move_pos == len(self._move_sequence):
            self._move_pos = 0

        self.x += self._move_sequence[self._move_pos][0]
        self.y += self._move_sequence[self._move_pos][1]

        self._move_pos += 1

    def update(self):
        self._move_dummy()
