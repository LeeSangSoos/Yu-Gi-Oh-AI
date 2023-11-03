import numpy as np

##from rlcard.games.uno.card 
from card import Card, Monster

def init_deck():

    SilverFang = Monster(1, 'SilverFang', 1200, 800, 3)
    MammothGraveyard = Monster(2, 'MammothGraveyard', 1200, 800, 3)
    Hitotsu_MeGiant = Monster(3, 'Hitotsu_MeGiant', 1200, 1000, 4)
    GaiaTheFierceKnight = Monster(4, 'GaiaTheFierceKnight', 2300, 2100, 7)
    DarkMagician = Monster(5, 'DarkMagician', 2500, 2100, 7)
    CurtainOfTheDarkOnes = Monster(6, 'CurtainOfTheDarkOnes', 600, 500, 2)
    Nemuriko = Monster(7, 'Nemuriko', 800, 700, 3)
    FireReaper = Monster(8, 'FireReaper', 700, 500, 2)
    FiendsHand = Monster(9, 'FiendsHand', 600, 600, 2)
    Hitodenchak = Monster(10, 'Hitodenchak', 600, 700, 2)
    TheFuriousSeaKing = Monster(11, 'TheFuriousSeaKing', 800, 700, 3)
    SectarianOfSecrets = Monster(12, 'SectarianOfSecrets', 700, 500, 2)
    KagemushaOfTheBlueFlame = Monster(13, 'KagemushaOfTheBlueFlame', 800, 400, 2)
    DarkGray = Monster(14, 'DarkGray', 800, 900, 3)

    cards = [SilverFang, MammothGraveyard, Hitotsu_MeGiant, GaiaTheFierceKnight, DarkMagician, 
             CurtainOfTheDarkOnes, Nemuriko, FireReaper, FiendsHand, Hitodenchak, 
             TheFuriousSeaKing, SectarianOfSecrets, KagemushaOfTheBlueFlame, DarkGray]

    deck = []
    for card in cards:
        deck.extend([card] * 3)

    return deck

def cards2list(cards):
    ''' Get the corresponding string representation of cards

    Args:
        cards (list): list of UnoCards objects

    Returns:
        (string): string representation of cards
    '''
    cards_list = []
    for card in cards:
        cards_list.append(card.get_str())
    return cards_list

def draw_card(player, num=1):
    for _ in range(num):
        if player.deck:  
            card = player.deck.pop(0)  
            player.hand.append(card)  
        else:
            print("The deck is empty. No cards to draw.")
            break

def encode_hand(plane, hand):
    ''' Encode hand and represerve it into plane

    Args:
        plane (array): 3*4*15 numpy array
        hand (list): list of string of hand's card

    Returns:
        (array): 3*4*15 numpy array
    '''
    # plane = np.zeros((3, 4, 15), dtype=int)
    plane[0] = np.ones((4, 15), dtype=int)
    hand = hand2dict(hand)
    for card, count in hand.items():
        card_info = card.split('-')
        color = COLOR_MAP[card_info[0]]
        trait = TRAIT_MAP[card_info[1]]
        if trait >= 13:
            if plane[1][0][trait] == 0:
                for index in range(4):
                    plane[0][index][trait] = 0
                    plane[1][index][trait] = 1
        else:
            plane[0][color][trait] = 0
            plane[count][color][trait] = 1
    return plane

def encode_target(plane, target):
    ''' Encode target and represerve it into plane

    Args:
        plane (array): 1*4*15 numpy array
        target(str): string of target card

    Returns:
        (array): 1*4*15 numpy array
    '''
    target_info = target.split('-')
    color = COLOR_MAP[target_info[0]]
    trait = TRAIT_MAP[target_info[1]]
    plane[color][trait] = 1
    return plane