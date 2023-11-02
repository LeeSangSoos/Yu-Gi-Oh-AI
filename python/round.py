from card import Monster
from card import Card
import utils


class Round:
    page = ['Draw', 'Main', 'Battle', 'End']
    Turn = [0, 1]

    def __init__(self, np_random, players):
        ''' Initialize the round class
        '''
        self.np_random = np_random
        self.players = players
        self.current_player = self.np_random.randint(0, 2)
        self.num_players = 2
        self.is_over = False
        self.winner = None
        self.page = "Draw"
        self.firstturn = True
        self.turn = self.current_player

    def get_legal_actions(self, players, player_id):
        legal_actions = []
        player = players[player_id]
        hand = players[player_id].hand
        player_monsterfield = players[player_id].monsterfield
        if self.turn == player_id:
            if self.page == "Draw":
                if player.turn_draw>=1:
                    legal_actions = ["draw"]
                else:
                    legal_actions = ["endpage"]
            elif self.page == "Main":
                if player.turn_summon >= 1 and len(player_monsterfield) <= 5:
                    for i in range(len(hand)):
                        card = hand[i]
                        if isinstance(card, Monster):
                            monster = card
                            if monster.level <= 4:
                                legal_actions.append(f"summon {monster.name}")
                            elif monster.level <= 6 and len(player_monsterfield) >= 1:
                                legal_actions.append(f"summon {monster.name}")
                            elif monster.level >= 7 and len(player_monsterfield) >= 2:
                                legal_actions.append(f"summon {monster.name}")
                legal_actions.append("endpage")
            elif self.page == 'Battle':
                for i in range(len(player_monsterfield)):
                    card = player_monsterfield[i]
                    if isinstance(card, Monster):
                        monster = card
                        if monster.atk_chance >=1 :
                            if monster.level <= 4:
                                legal_actions.append(f"attack {monster.name}")
                            elif monster.level <= 6 and len(player_monsterfield) >= 1:
                                legal_actions.append(f"attack {monster.name}")
                            elif monster.level >= 7 and len(player_monsterfield) >= 2:
                                legal_actions.append(f"attack {monster.name}")
                legal_actions.append("endpage")
            elif self.page == 'End':
                if len(hand) >=7 :
                    for i in range(len(hand)):
                        card = hand[i]
                        legal_actions.append(f"discard {card.name}")
                else:
                    legal_actions = ["endturn"]

        return legal_actions

    def get_state(self, players, player_id):
        ''' Get game state
        '''
        state = {}
        player = players[player_id]
        opponent = players[1-player_id]
        state['hand'] = utils.cards2list(player.hand)
        state['deck'] = utils.cards2list(player.deck)
        state['player_monsterfield'] = utils.cards2list(player.monsterfield)
        state['opponent_monsterfield'] = utils.cards2list(opponent.monsterfield)
        state['legal_actions'] = self.get_legal_actions(players, player_id)
        state['player_life'] = player.life
        state['opponent_life'] = opponent.life
        return state

    def _perform_draw_action(self, players, player_id, n):
        # replace deck if there is no card in draw pile
        player = players[player_id]
        for _ in range(n):
            if not player.deck:
                self.is_over = True
                self.winner = players[1-player_id]
                break
            else:
                player.hand.append(player.deck.pop())
                player.turn_draw-=1

    def proceed_round(self, action):
        if self.page == 'Draw':
            if action == 'draw':
                self._perform_draw_action(self.players, self.current_player,1)
            elif action == 'endpage':
                self.page = 'Main'
        elif self.page == 'Main':
            if action == 'endpage':
                if self.firstturn :
                    self.page = 'End'
                    self.firstturn = False
                else:
                    self.page = 'Battle'
            else:
                action_parts = action.split()
                monster_name = ' '.join(action_parts[1:])
                
                monster = next((m for m in self.players[self.current_player].hand if m.name == monster_name), None)
                if monster:
                    self.players[self.current_player].hand.remove(monster)
                    self.players[self.current_player].normal_summon(monster)
                else:
                    print(f"No monster with the name {monster_name} was found in the hand.")
        elif self.page == 'Battle':
            if action == 'endpage':
                self.page = 'End'
            else:
                action_parts = action.split()
                monster_name = ' '.join(action_parts[1:])
                monster = next((m for m in self.players[self.current_player].monsterfield if m.name == monster_name), None)
                if monster:
                    self.battle(monster)
                else:
                    print(f"No monster with the name {monster_name} was found in the field.")
        elif self.page == 'End':
            if action == 'endturn':
                self.turn = 1 - self.current_player
                self.current_player = 1 - self.current_player
                self.page = 'Draw'
            else:
                action_parts = action.split()
                card_name = ' '.join(action_parts[1:])
                card = next((c for c in self.players[self.current_player].hand if c.name == card_name), None)
                if card:
                    self.players[self.current_player].hand.remove(card)
    
    def battle(self, monster):
        player = self.players[self.current_player]
        opponent = self.players[1 - self.current_player]
        if len(opponent.monsterfield) == 0:
            opponent.life -= monster.atk
        else:
            target = min(opponent.monsterfield, key=lambda monster: monster.atk)
            if target.atk < monster.atk:
                opponent.monsterfield.remove(target)
                opponent.life -= (monster.atk - target.atk)
            elif target.atk == monster.atk:
                opponent.monsterfield.remove(target)
                player.monsterfield.remove(monster)
            elif target.atk > monster.atk:
                player.monsterfield.remove(monster)
                player.life -= (target.atk - monster.atk)
        if opponent.life <= 0:
            self.is_over = True
            self.winner = self.players[self.current_player]
        elif player.life <=0:
            self.is_over = True
            self.winner = self.players[1-self.current_player]