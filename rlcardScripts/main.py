import numpy as np
import torch
from collections import OrderedDict
import json
import os
import time

from rlcard.agents import DQNAgent
from rlcard.games.custom.utils import cards2list, encode_hand, encode_field, encode_life, encode_page, ACTION_SPACE
from rlcard.games.custom.player import Player
from rlcard.games.custom.card import Monster

class PlayerWithUnity:
    def __init__(self):
        # load saved agent
        checkpoint = torch.load('SavedDQNAgent.pth', map_location=torch.device('cpu'))
        self.agent = DQNAgent.from_checkpoint(checkpoint)

        self.first_turn = True
        self.page = "Main"
        self.action_recorder = []
        self.is_over = False
        self.datapath = r"C:\Users\proto\AppData\LocalLow\DefaultCompany\Yu-gi-oh"
        self.signle2unity = 1
        self.signleFromUnity = 0
        np_random_instance = np.random.RandomState(seed=42)

        # get dqnagent player
        self.dqnPlayer = Player(0, np_random_instance)
        self.get_player_data(self.dqnPlayer)

        # get human player
        self.humanPlayer = Player(0, np_random_instance)
        self.get_player_data(self.humanPlayer)
        
    def run_game(self):
        print("run game")
        # Loop until the game ends
        while not self.is_over:
            # Wait until unity signle the agent the true
            while self.signleFromUnity >= self.load_file("SignleAi.json"):
                time.sleep(1)
            self.signleFromUnity = self.load_file("SignleAi.json")
            self.signle2unity += 1
            
            # Check if Game is over
            loaded_data = self.load_file("IsOver.json")
            value = loaded_data["Value"]
            self.is_over = value

            # Get players from unity
            self.get_player_data(self.dqnPlayer)
            self.get_player_data(self.humanPlayer)

            # Get data from unity
            self.first_turn = self.load_file("isFirstTurn.json")
            self.page = self.load_file("GamePage.json")
            
            # Get state from players
            extracted_state = self._extract_state(self.get_state(self.dqnPlayer, self.humanPlayer))

            # Run agent and get action
            action, _ = self.agent.eval_step(extracted_state)

            # Send action to unity
            self.save_file('DqnAction.json', action)
            self.save_file('SignleGame.json', self.signle2unity)
            

            # Get is_over from unity
            loaded_data = self.load_file("IsOver.json")
            value = loaded_data["Value"]
            self.is_over = value

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
        
        legal_action_id = self._get_legal_actions(state['legal_actions'])

        extracted_state = {'obs': obs, 'legal_actions': legal_action_id}
        extracted_state['raw_obs'] = state
        
        extracted_state['raw_legal_actions'] = [a for a in state['legal_actions']]
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
        elif self.page == "Main" or self.page == "Main1":
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

        if self.page == "Main" or self.page == "Battle" or self.page == "Main1":
            legal_actions.add("endpage")
        
        return legal_actions

    def _get_legal_actions(self, legal_actions):
        ''' Get all leagal actions
        Returns:
            OrderedDict (list): return encoded legal action list
        '''
        legal_ids = {ACTION_SPACE[action]: None for action in legal_actions}
        return OrderedDict(legal_ids)
    
    def save_file(self, file_name, data):
        file_path = os.path.join(self.datapath, file_name)
        with open(file_path, 'w') as f:
            json.dump(data, f, default=self.convert_to_json_type)
    
    def load_file(self, file_name):
        file_path = os.path.join(self.datapath, file_name)
        with open(file_path, 'r') as f:
            loaded_data = json.load(f)
            return loaded_data
    
    def jsonCard2PythonCard(self, player_hand_data:list):
        playerHand = []
        for card in player_hand_data:
            card_info = card.get("Dictionary", {})
            card_id = card_info.get("cardId")
            card_name = card_info.get("cardName")
            card_atk = card_info.get("cardAtk")
            card_def = card_info.get("cardDef")
            card_level = card_info.get("cardLevel")
            monster_card = Monster(card_id, card_name, card_atk, card_def, card_level)
            playerHand.append(monster_card)
        return playerHand
    
    def get_player_data(self, player):
        fileName = "HumanPlayer.json"
        if player == self.dqnPlayer:
            fileName = "DqnPlayer.json"
        # read json file
        loaded_data = self.load_file(fileName).get("Dictionary", {})
        # get cards info in hand
        playerHand = self.jsonCard2PythonCard(loaded_data.get("playerHand", {}).get("List", []))
        playerMonsterField = self.jsonCard2PythonCard(loaded_data.get("playerMonsterField", {}).get("List", []))
        playerDeck = self.jsonCard2PythonCard(loaded_data.get("playerDeck", {}).get("List", []))

        player.setVariables(
            loaded_data.get("playerId"), playerHand, 
            playerMonsterField, loaded_data.get("playerLife"), 
            loaded_data.get("playerDraw"),loaded_data.get("playerTurnSummon"), 
            playerDeck
        )
    
    @staticmethod
    def convert_to_json_type(obj):
        if isinstance(obj, np.integer):
            return int(obj)
        if isinstance(obj, np.floating):
            return float(obj)
        if isinstance(obj, np.ndarray):
            return obj.tolist()

game = PlayerWithUnity()
game.run_game()
