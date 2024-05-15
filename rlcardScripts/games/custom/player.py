from rlcard.games.custom.utils import init_deck

class Player:

    def __init__(self, player_id, np_random):
        self.np_random = np_random
        self.player_id:int = player_id
        self.hand:list = []
        self.monsterfield:list = [None]*5
        self.life:int = 8000

        self.deck:list = init_deck()
        self.shuffle()

        self.turn_draw = 1
        self.turn_summon = 1
    
    def shuffle(self):
        ''' Shuffle the deck
        '''
        self.np_random.shuffle(self.deck)

    def get_player_id(self):
        ''' Return the id of the player
        '''

        return self.player_id
    
    def draw(self, n):
        for i in range(n):
            self.hand.append(self.deck.pop())

    def setVariables(self, player_id, hand=None, monsterfield=None, life=8000, turn_draw=1, turn_summon=1, deck=None):
        self.player_id = player_id
        self.hand = hand
        self.monsterfield = monsterfield
        self.life = life
        self.turn_draw = turn_draw
        self.turn_summon = turn_summon
        self.deck = deck
        