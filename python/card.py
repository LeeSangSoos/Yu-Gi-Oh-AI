class Card:
    info = {'type':  ['monster', 'magic', 'trap']
            }

    def __init__(self, card_type, card_id, card_name):
        self.type = card_type
        self.name = card_name
        self.id = card_id

    def get_name(self):
        return self.name

    @staticmethod
    def print_cards(cards):
        if isinstance(cards, str):
            cards = [cards]
        for i, card in enumerate(cards):

            print(card.name, card.type, end='')

            if i < len(cards) - 1:
                print(', ', end='')

class Monster(Card):

    def __init__(self, card_id, card_name, atk, defence, level):
        super().__init__('monster', card_id, card_name)
        self.atk = atk
        self.defence = defence
        self.level = level
        self.atk_chance = 1
        self.info = super().info.copy()
        self.info.update({
            'atk': atk,
            'def': defence,
            'level': level,
        })