class Player:

    def __init__(self, player_id, np_random):
        self.np_random = np_random
        self.player_id = player_id
        self.hand = []
        self.deck = []
        self.monsterfield = []
        self.turn_summon = 1
        self.life = 8000
        self.turn_draw = 1

    def get_player_id(self):
        ''' Return the id of the player
        '''
        return self.player_id
    
    def get_player_hand(self):
        return self.hand
    
    def get_player_deck(self):
        return self.deck
    
    def get_player_monster_field(self):
        return self.monsterfield
    
    def normal_summon(self, monster):
        if len(self.monsterfield) < 5:
            if monster.level <= 4:
                pass
            elif monster.level <= 6 and len(self.monsterfield) >= 1:
                self.monsterfield.pop()
            elif monster.level >= 7 and len(self.monsterfield) >= 2:
                self.monsterfield.pop()
                self.monsterfield.pop()
            self.monsterfield.append(monster)
            self.turn_summon-=1
        else:
            print("Monster field is full. Cannot add another monster.")