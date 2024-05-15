import numpy as np
import torch
from collections import OrderedDict

from rlcard.agents import DQNAgent
from rlcard.games.custom.utils import cards2list, encode_hand, encode_field, encode_life, encode_page, ACTION_SPACE
from rlcard.games.custom.player import Player


class PlayerWithUnity:
    def __init__(self):
        # load saved agent
        checkpoint = torch.load('SavedDQNAgent.pth')
        self.agent = DQNAgent.from_checkpoint(checkpoint)

        self.first_turn = True
        self.page = "Main"
        self.action_recorder = []
        self.is_over = False

        # TODO get agent ID
        self.dqnPlayer = Player(0, 0)

        # TODO get player ID
        self.humanPlayer = Player(0, 0)

    def run_game(self):
        # Loop until the game ends
        while not self.is_over:
            # TODO: Get players from unity
            self.dqnPlayer
            self.humanPlayer

            # TODO : Get data from unity
            self.first_turn = True
            self.page = "Main"

            # TODO : Get human action from unity
            action = 0
            self.action_recorder.append((self.humanPlayer.player_id, action))
            
            # Get state from players
            extracted_state = self._extract_state(self.get_state(self.dqnPlayer, self.humanPlayer))

            # Run agent and return action
            action = self.agent.eval_step(extracted_state['obs'])

            # Record action of agent
            self.action_recorder.append((self.dqnPlayer.player_id, action))

            # TODO: Send action to unity

            # TODO: Get is_over from unity
            self.is_over = False

    def get_state(self, player, enemy):
        state = {}
        # 각 카드의 id, faceup 정보리스트
        state['hand'] = cards2list(player.hand) # card id list
        # 각 카드의 id, faceup 정보리스트
        state['player_monsterfield'] = cards2list(player.monsterfield)
        # legal_action들의 string값 정보
        state['legal_actions'] = self.get_legal_actions(self.dqnPlayer, self.humanPlayer)
        # 생명력 int값
        state['life'] = player.life

        # 각 카드의 id, faceup 정보리스트
        state['enemy_monsterfield'] = cards2list(enemy.monsterfield)
        # 생명력 int값
        state['enemy_life'] = enemy.life
        #  ['Draw', 'Main', 'Battle', 'End'] string 정보
        state['page'] = self.page

        return state

    def _extract_state(self, state):
        obs = np.zeros((6, 7, 4), dtype=int)

        encode_hand(obs[0], state['hand'])
        encode_field(obs[1], state['player_monsterfield'])
        encode_field(obs[2], state['enemy_monsterfield'])
        encode_life(obs[3], state['life'])
        encode_life(obs[4], state['enemy_life'])
        encode_page(obs[5], state['page'])
        
        legal_action_id = self._get_legal_actions(self.dqnPlayer, self.humanPlayer)

        extracted_state = {'obs': obs, 'legal_actions': legal_action_id}
        extracted_state['raw_obs'] = state
        
        extracted_state['raw_legal_actions'] = [a for a in state['legal_actions']]
        extracted_state['action_record'] = self.action_recorder
        return extracted_state

    def get_legal_actions(self, player, opponent):
        legal_actions = set()
        hand = player.hand
        player_monsterfield = player.monsterfield
        opponent_monsterfield = opponent.monsterfield

        if self.page == "Draw":
            if player.turn_draw >= 1:
                legal_actions.add("draw")
            else:
                legal_actions.add("endpage")
        elif self.page == "Main":
            used_places = set()
            for i in range(len(player_monsterfield)):
                if player_monsterfield[i] is not None:
                    used_places.add(i)
            if player.turn_summon >= 1:
                for hand_idx in range(len(hand)):
                    monster = hand[hand_idx]
                    if monster is not None:
                        if monster.level <= 4 and len(used_places) < 5:
                            legal_actions.add(f"summon-0-{hand_idx}-faceup")
                            legal_actions.add(f"summon-0-{hand_idx}-facedown")
                        elif monster.level <= 6 and len(used_places) >= 1:
                            for sacrifice_idx in used_places:
                                legal_actions.add(f"summon-1-{hand_idx}-{sacrifice_idx}-faceup")
                                legal_actions.add(f"summon-1-{hand_idx}-{sacrifice_idx}-facedown")
                        elif monster.level >= 7 and len(used_places) >= 2:
                            for sacrifice_idx1 in used_places:
                                for sacrifice_idx2 in used_places:
                                    if sacrifice_idx2 != sacrifice_idx1:
                                        legal_actions.add(f"summon-2-{hand_idx}-{sacrifice_idx1}-{sacrifice_idx2}-faceup")
                                        legal_actions.add(f"summon-2-{hand_idx}-{sacrifice_idx1}-{sacrifice_idx2}-facedown")
        elif self.page == "Battle":
            if not self.first_turn:
                for player_field_idx in range(len(player_monsterfield)):
                    if player_monsterfield[player_field_idx] is not None:
                        monster = player_monsterfield[player_field_idx]
                        if monster.atk_chance >= 1 and monster.faceup:
                            if all(card is None for card in opponent_monsterfield):
                                legal_actions.add(f"attack-{player_field_idx}-direct")
                            else:
                                for enemy_field_idx in range(len(opponent_monsterfield)):
                                    if opponent_monsterfield[enemy_field_idx] is not None:
                                        legal_actions.add(f"attack-{player_field_idx}-{enemy_field_idx}")
        elif self.page == "End":
            if len(hand) >= 7:
                for hand_idx in range(len(hand)):
                    card = hand[hand_idx]
                    if card is not None:
                        legal_actions.add(f"discard-{hand_idx}")
            else:
                legal_actions.add("endpage")

        if self.page == "Main" or self.page == "Battle":
            legal_actions.add("endpage")
        
        legal_actions = list(legal_actions)
        legal_ids = {ACTION_SPACE[action]: None for action in legal_actions }
        return OrderedDict(legal_ids)

# Instantiate YourGameClass and run the game
game = PlayerWithUnity()
game.run_game()